using Kirurobo;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using static Kirurobo.UniWindowController;

public class InitializerManager : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] int heightScreen = 450;
    [SerializeField] UniWindowController windowController;

    [Space(10)]
    [SerializeField] float offsetBound = 200f;

    [Header("Scene Loader")]
    [SerializeField] SceneLoaderManager sceneLoaderManager;




    private IDataService jsonService = new JsonDataService();



    private bool isInit;

    private bool hasCheckFiles;

    private bool hasSaveFile = true;



    public bool HasCheckFiles => hasCheckFiles;
    public bool HasSaveFile => hasSaveFile;





    public static InitializerManager Instance { get; private set; }

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

        windowController.OnStateChanged += Setup;
    }


    private void OnDestroy()
    {
        if (Instance != this) return;

        // if a save is never created but the game has been opened, delete newly created save files
        if (!hasSaveFile)
        {
            string persistent = Application.persistentDataPath + "/";

            Directory.Delete(persistent + UtilsSave.ROOT_FOLDER, true);
        }
    }


    private void Setup(WindowStateEventType type)
    {
        if (isInit) return;
        isInit = true;

        windowController.windowSize = new Vector2(Screen.currentResolution.width, heightScreen);

        // first set
        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);

        windowController.windowPosition = new Vector2(0, Display.displays[0].systemHeight - displays[0].workArea.height - 1);

        //Debug.Log("win init pos: " + windowController.windowPosition);

        HandleOtherSetups();

        //Debug.Log("Screen: " + windowController.windowSize);
        //Debug.Log("Screen pos: " + windowController.windowPosition);
        //Debug.Log("taskbar size: " + usableScreen.y);
    }

    public IEnumerator CoChangeMonitor(int monitorIndex)
    {
        //Debug.Log("monitor index: " + monitorIndex);

        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);
        AsyncOperation moveScreenOp = Screen.MoveMainWindowTo(displays[monitorIndex], Vector2Int.zero); //RoundToInt(Vector2.zero)

        //Debug.Log("working area: " + displays[monitorIndex].workArea.ToString());

        yield return moveScreenOp;

        // if null the scene changed from last time
        if(windowController == null)
        {
            windowController = FindFirstObjectByType<UniWindowController>();
        }

        //Debug.Log("win pos after default move: " + windowController.windowPosition);

        // get new window pos, y set from top to bottom, so the difference is necessary to set at the bottom
        Vector2 windowPos = new Vector2(windowController.windowPosition.x, Display.displays[monitorIndex].systemHeight - displays[monitorIndex].workArea.height - 1);
        //Debug.Log("expected pos: " + windowPos);
        //Debug.Log("system h: " + Display.displays[monitorIndex].systemHeight + ", usable screen h: " + displays[monitorIndex].workArea.height + ", pos y: " + windowPos.y);
        
        // get new window size
        Vector2 windowSize = new Vector2(Display.displays[monitorIndex].systemWidth, heightScreen);
        //Debug.Log("expected win size: " + windowSize);

        // set window
        // should put the window at the start of the monitor
        windowController.windowSize = windowSize;
        windowController.windowPosition = windowPos;

        //Debug.Log("actual win pos: " + windowController.windowPosition);
        //Debug.Log("actual win size: " + windowController.windowSize);
        //
        //Debug.Log("---------------------------------------------------");
    }




    private void HandleOtherSetups()
    {
        // utils setups
        UtilsPlayer.Initialize();
        UtilsItem.Initialize();
        UtilsEnemy.Initialize();
        UtilsCombatMap.Initialize();
        UtilsQuest.Initialize();
        UtilsGather.Initialize();
        UtilsShop.Initialize();

        // load files
        HandleSaves();

        // call loader scene setup - set material
        sceneLoaderManager.Setup();

        // set checked files
        hasCheckFiles = true;

        //Debug.Log(SettingsManager.Instance.LastSceneSettings.lastSceneName);

        // check save for last scene - loading scene manager should handle the alpha
    }

    public void HandleSaves()
    {
        string persistent = Application.persistentDataPath + "/";

        // Create folder if never opened
        if (!Directory.Exists(persistent + UtilsSave.ROOT_FOLDER))
        {
            hasSaveFile = false;

            Directory.CreateDirectory(persistent + UtilsSave.ROOT_FOLDER);

            Directory.CreateDirectory(persistent + UtilsSave.GetPlayerFolder());
            Directory.CreateDirectory(persistent + UtilsSave.GetSettingsFolder());
            Directory.CreateDirectory(persistent + UtilsSave.GetCombatMapsFolder());
            Directory.CreateDirectory(persistent + UtilsSave.GetQuestsFolder());
            Directory.CreateDirectory(persistent + UtilsSave.GetShopFolder());
        }
        else
        {
            hasSaveFile = true;
        }

        SettingsManager.Instance.Setup(jsonService);
        PlayerManager.Instance.Setup(jsonService);
        QuestManager.Instance.Setup(jsonService);
        ShopManager.Instance.Setup(jsonService);
    }


    public void EraseAllSaves()
    {
        string persistent = Application.persistentDataPath + "/";

        // delete all
        Directory.Delete(persistent + UtilsSave.ROOT_FOLDER, true);
    }



    public static float GetScreenWidth()
    {
        return Screen.currentResolution.width;
    }

    public float GetScreenOffsetBound()
    {
        return offsetBound;
    }




    public void SetHasSaveFile()
    {
        hasSaveFile = true;
    }
}
