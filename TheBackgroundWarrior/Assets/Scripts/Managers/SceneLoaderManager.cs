using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    public enum SceneType { Home, CombatMap, Miner, Blacksmith, Fisher, Farmer }


    [SerializeField] Material fadeMaterial;

    [Space(10)]
    [SerializeField] float onFadeInStart = 45f;
    [SerializeField] float onFadeInEnd = 60f;

    [Space(10)]
    [SerializeField] float onFadeOutStart = -60f;
    [SerializeField] float onFadeOutEnd = -45f;

    [Space(10)]
    [SerializeField] float fadeTime = 3f;

    [Space(10)]
    [SerializeField] ParticleSystem particleVFX;

    private bool isInit;
    private bool isSetup;

    private int fadeStart = Shader.PropertyToID("_FadeStart");
    private int fadeEnd = Shader.PropertyToID("_FadeEnd");


    private UIBarrier currentSceneBarrier;

    public static SceneLoaderManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (Instance != this) return;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (Instance != this) return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Load home and not overwrite last scene, so when you click continue from home actually restarts from correct scene
    /// </summary>
    public void LoadHome()
    {
        // if the time was stopped before changing scene, resume it
        UtilsTime.Resume();

        // handle current scene hide objects

        if (SceneManager.GetActiveScene().name == "HomeScene")
        {
            HomeWorldManager.Instance.HandleSwitchScene();
        }
        else
        {
            switch (SettingsManager.Instance.LastSceneSettings.lastSceneType)
            {
                default: Debug.Log("Current scene type isn't allowed"); break;
                case SceneType.CombatMap: CombatManager.Instance.HandleSwitchScene(); break;
                case SceneType.Miner: SmashManager.Instance.HandleSwitchScene(); break;
                case SceneType.Blacksmith: FindFirstObjectByType<PlayerBlacksmith>().HandleSwitchScene(); break;
                case SceneType.Fisher: break;
                case SceneType.Farmer: FindFirstObjectByType<PlayerFarmer>().HandleSwitchScene(); break;
            }
        }

        StartCoroutine(CoLoadHome());
    }

    private IEnumerator CoLoadHome()
    {
        // enable scene barrier before loading
        currentSceneBarrier = FindFirstObjectByType<UIBarrier>();
        if (currentSceneBarrier != null)
        {
            currentSceneBarrier.EnableBarrier(true);
        }


        StartCoroutine(CoFadeOut());

        yield return new WaitForSecondsRealtime(fadeTime + 0.5f);

        SceneManager.LoadScene("HomeScene");
    }

    public void LoadScene(LastSceneSettings settings)
    {
        // if the time was stopped before changing scene, resume it
        UtilsTime.Resume();

        // handle current scene hide objects

        if(SceneManager.GetActiveScene().name == "HomeScene")
        {
            HomeWorldManager.Instance.HandleSwitchScene();
        }
        else
        {
            switch (SettingsManager.Instance.LastSceneSettings.lastSceneType)
            {
                default: Debug.Log("Current scene type isn't allowed"); break;
                case SceneType.CombatMap: CombatManager.Instance.HandleSwitchScene(); break;
                case SceneType.Miner: SmashManager.Instance.HandleSwitchScene(); break;
                case SceneType.Blacksmith: FindFirstObjectByType<PlayerBlacksmith>().HandleSwitchScene(); break;
                case SceneType.Fisher: break;
                case SceneType.Farmer: FindFirstObjectByType<PlayerFarmer>().HandleSwitchScene(); break;
            }
        }

        StartCoroutine(CoChangeScene(settings));
    }



    private IEnumerator CoChangeScene(LastSceneSettings settings)
    {
        // enable scene barrier before loading
        currentSceneBarrier = FindFirstObjectByType<UIBarrier>();
        if (currentSceneBarrier != null)
        {
            currentSceneBarrier.EnableBarrier(true);
        }


        StartCoroutine(CoFadeOut());

        yield return new WaitForSecondsRealtime(fadeTime + 0.5f);

        SettingsManager.Instance.SetSceneSettings(settings);
        SceneManager.LoadScene(settings.lastSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // put into else block whatever needs to be called always but the first time the manager is initialized
        if (!isInit)
        {
            isInit = true;
        }
        else
        {
            // set every time you change scene the correct monitor
            //StartCoroutine(InitializerManager.Instance.CoChangeMonitor(SettingsManager.Instance.CurrentMonitorIndex));
        }
            
        // disable scene barrier after loading
        currentSceneBarrier = FindFirstObjectByType<UIBarrier>();
        if (currentSceneBarrier != null)
        {
            currentSceneBarrier.EnableBarrier(false);
        }

        StartCoroutine(CoFadeIn());

        UIManager uiManager = null;

        // if home only
        if (scene.name == "HomeScene")
        {
            HomeWorldManager.Instance.Setup();
            //uiManager = FindFirstObjectByType<UIManager>();
        }
        else
        {
            LastSceneSettings settings = SettingsManager.Instance.LastSceneSettings;

            switch (settings.lastSceneType)
            {
                case SceneType.CombatMap:
                    CombatManager.Instance.Setup(UtilsCombatMap.GetMapById(settings.lastCombatMapId));
                    uiManager = FindFirstObjectByType<UIManager>();
                    break;

                case SceneType.Miner:
                    SmashManager.Instance.Setup();
                    uiManager = FindFirstObjectByType<UIManager>();
                    break;

                case SceneType.Blacksmith:
                    FindFirstObjectByType<PlayerBlacksmith>().Setup(PlayerManager.Instance.PlayerBlacksmithData);
                    uiManager = FindFirstObjectByType<UIManager>();
                    break;

                case SceneType.Fisher:
                    FindFirstObjectByType<PlayerFisher>().Setup(PlayerManager.Instance.PlayerFisherData);
                    uiManager = FindFirstObjectByType<UIManager>();
                    break;

                case SceneType.Farmer:
                    FindFirstObjectByType<PlayerFarmer>().Setup(PlayerManager.Instance.PlayerFarmerData);
                    uiManager = FindFirstObjectByType<UIManager>();
                    break;
            }
        }

        if (uiManager != null)
        {
            uiManager.Setup();
        }
    }

    private IEnumerator CoFadeIn()
    {
        particleVFX.Play();

        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fadeTime;

            float start = Mathf.Lerp(onFadeInStart, onFadeOutStart, t);
            fadeMaterial.SetFloat(fadeStart, start);

            float end = Mathf.Lerp(onFadeInEnd, onFadeOutEnd, t);
            fadeMaterial.SetFloat(fadeEnd, end);

            particleVFX.transform.position = new Vector3(start, particleVFX.transform.position.y, particleVFX.transform.position.z);

            yield return null;
        }

        fadeMaterial.SetFloat(fadeStart, onFadeOutStart);
        fadeMaterial.SetFloat(fadeEnd, onFadeOutEnd);

        particleVFX.Stop();
    }

    private IEnumerator CoFadeOut()
    {
        particleVFX.Play();

        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fadeTime;

            float start = Mathf.Lerp(onFadeOutStart, onFadeInStart, t);
            fadeMaterial.SetFloat(fadeStart, start);

            float end = Mathf.Lerp(onFadeOutEnd, onFadeInEnd, t);
            fadeMaterial.SetFloat(fadeEnd, end);

            particleVFX.transform.position = new Vector3(end, particleVFX.transform.position.y, particleVFX.transform.position.z);

            yield return null;
        }

        fadeMaterial.SetFloat(fadeStart, onFadeInStart);
        fadeMaterial.SetFloat(fadeEnd, onFadeInEnd);

        particleVFX.Stop();
    }

    public void Setup()
    {
        if (isSetup) return;
        isSetup = true;

        fadeMaterial.SetFloat(fadeStart, onFadeInStart);
        fadeMaterial.SetFloat(fadeEnd, onFadeInEnd);
    }
}


[System.Serializable]
public struct LastSceneSettings
{
    public string lastSceneName;

    public SceneLoaderManager.SceneType lastSceneType;

    // save a file with name as namemap+id, and save the infromation needed, like prestige and reached stage
    // combat map
    public int lastCombatMapId;
}