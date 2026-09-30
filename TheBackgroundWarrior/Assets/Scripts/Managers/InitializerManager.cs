using Kirurobo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

using static Kirurobo.UniWindowController;

public class InitializerManager : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] int heightScreen = 450;
    [SerializeField] UniWindowController windowController;

    public int HeightScreen => heightScreen;


    [Space(10)]
    [SerializeField] float offsetBound = 200f;

    [Header("Scene Loader")]
    [SerializeField] SceneLoaderManager sceneLoaderManager;

    [Header("Taskbar Flash")]
    [SerializeField] TaskbarFlasher _taskbarFlasher;




    private IDataService jsonService = new JsonDataService();



    private bool isInit;

    private bool hasCheckFiles;

    private bool hasSaveFile = true;



    public bool HasCheckFiles => hasCheckFiles;
    public bool HasSaveFile => hasSaveFile;


    public bool FatalError { get; private set; }




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

        //LogWindowPositions();

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

        try
        {
            // get monitors with name
            List<UtilsWindowsMonitor.MonitorData> monitors = UtilsWindowsMonitor.GetMonitorsLeftToRight();

            Dictionary<int, int> unityDisplayToWindowsIndex = new Dictionary<int, int>();

            // loop windows monitors
            for (int i = 0; i < monitors.Count; i++)
            {
                // check last character to get primary and not indexes
                string deviceName = monitors[i].DeviceName;
                string lastChar = deviceName[deviceName.Length - 1].ToString();
                if (int.TryParse(lastChar, out int indexMonitor))
                {
                    // key: windows index, value: unity index
                    unityDisplayToWindowsIndex.Add(indexMonitor - 1, i);
                }
            }

            // get the correct display info using the dictionary
            Rect monitorRect = UniWindowController.GetMonitorRect(unityDisplayToWindowsIndex[0]);

            float taskbarHeight = Display.displays[0].systemHeight - displays[0].workArea.height;

            windowController.windowPosition = new Vector2(
                0f,
                monitorRect.y + taskbarHeight - 1
            );
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());

            // defaulted to use system height if any exception occurs
            // get new window pos, y set from top to bottom, so the difference is necessary to set at the bottom
            windowController.windowPosition = new Vector2(0, Display.displays[0].systemHeight - displays[0].workArea.height - 1);
        }
#else
        windowController.windowPosition = new Vector2(0, Display.displays[0].systemHeight - displays[0].workArea.height - 1);
#endif

        HandleOtherSetups();

        //Debug.Log("Screen: " + windowController.windowSize);
        //Debug.Log("start Screen pos: " + windowController.windowPosition);
        //Debug.Log("taskbar size: " + usableScreen.y);
    }

    public IEnumerator CoChangeMonitor(int monitorIndex)
    {
        //Debug.Log("switchhing to: " + monitorIndex);

        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);
        AsyncOperation moveScreenOp = Screen.MoveMainWindowTo(displays[monitorIndex], Vector2Int.zero); //RoundToInt(Vector2.zero)

        //Debug.Log("moving...");

        yield return moveScreenOp;

        //Debug.Log("moved.");

        // if null the scene changed from last time
        if (windowController == null)
        {
            windowController = FindFirstObjectByType<UniWindowController>();
        }

        // get new window size
        Vector2 windowSize = new Vector2(Display.displays[monitorIndex].systemWidth, heightScreen);
        Vector2 windowPos = Vector2.zero;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

        try
        {
            // get monitors with name
            List<UtilsWindowsMonitor.MonitorData> monitors = UtilsWindowsMonitor.GetMonitorsLeftToRight();

            Dictionary<int, int> unityDisplayToWindowsIndex = new Dictionary<int, int>();

            // loop windows monitors
            for (int i = 0; i < monitors.Count; i++)
            {
                // check last character to get primary and not indexes
                string deviceName = monitors[i].DeviceName;
                string lastChar = deviceName[deviceName.Length - 1].ToString();

                //Debug.Log("device name: " + deviceName);
                if (int.TryParse(lastChar, out int indexMonitor))
                {
                    // key: windows index, value: unity index
                    unityDisplayToWindowsIndex.Add(indexMonitor - 1, i);
                    //Debug.Log("added key: " + (indexMonitor - 1) + ", val: " + i);
                }
            }

            // get the correct display info using the dictionary
            Rect monitorRect = UniWindowController.GetMonitorRect(unityDisplayToWindowsIndex[monitorIndex]);

            //Debug.Log("--- setting pos and size.");
            float taskbarHeight = Display.displays[monitorIndex].systemHeight - displays[monitorIndex].workArea.height;

            windowPos = new Vector2(
                monitorRect.x,
                monitorRect.y + taskbarHeight - 1
            );
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());

            // defaulted to use system height if any exception occurs
            // get new window pos, y set from top to bottom, so the difference is necessary to set at the bottom
            windowPos = new Vector2(windowController.windowPosition.x, Display.displays[monitorIndex].systemHeight - displays[monitorIndex].workArea.height - 1);
        }
#else
        // get new window pos, y set from top to bottom, so the difference is necessary to set at the bottom
        windowPos = new Vector2(windowController.windowPosition.x, Display.displays[monitorIndex].systemHeight - displays[monitorIndex].workArea.height - 1);
#endif
        // set window
        windowController.windowSize = windowSize;
        windowController.windowPosition = windowPos;

        //Debug.Log("after change montor: " + windowController.windowPosition);
        //Debug.Log("actual win size: " + windowController.windowSize);
        //
        //Debug.Log("---------------------------------------------------");
    }

    private void LogWindowPositions()
    {
        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);

        for (int i = 0; i < displays.Count; i++)
        {
            Debug.Log($"DisplayInfo {i} -> workArea: {displays[i].workArea}");
        }

        int monitorCount = UniWindowController.GetMonitorCount();
        for (int i = 0; i < monitorCount; i++)
        {
            Debug.Log($"MonitorRect {i} -> {UniWindowController.GetMonitorRect(i)}");
        }
    }



    private async void HandleOtherSetups()
    {
        try
        {
            // initialize all texts
            UtilsText.Initialize();

            // initialize text general
            UtilsGeneral.Initialize();

            // utils setups
            UtilsPlayer.Initialize();
            UtilsItem.Initialize();
            UtilsEnemy.Initialize();
            UtilsCombatMap.Initialize();
            UtilsQuest.Initialize();
            UtilsShop.Initialize();

            // load files
            await HandleSaves();

            // call loader scene setup - set material
            sceneLoaderManager.Setup();

            // set checked files
            hasCheckFiles = true;

            //Debug.Log(SettingsManager.Instance.LastSceneSettings.lastSceneName);

            // check save for last scene - loading scene manager should handle the alpha
        }
        catch (Exception e)
        {
            Debug.LogError("HandleOtherSetups failed: " + e.Message + "\n" + e.StackTrace);
            FatalError = true;
        }
    }

    public async Task HandleSaves()
    {
        string persistent = Application.persistentDataPath + "/";

        // Create folder if never opened
        if (!Directory.Exists(persistent + UtilsSave.ROOT_FOLDER))
        {
            hasSaveFile = false;

            UtilsSave.CreateAllFolders();
        }
        else
        {
            hasSaveFile = true;

            try
            {
                UtilsSave.CheckAllFolders();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + "\n" + e.StackTrace);
                FatalError = true;
            }
        }

        try
        {
            SettingsManager.Instance.Setup(jsonService);
            PlayerManager.Instance.Setup(jsonService);
            QuestManager.Instance.Setup(jsonService);
            ShopManager.Instance.Setup(jsonService);


            // Check for jobs that you should have unlocked but they aren't for some reason
            PlayerManager.Instance.InitialJobChecks();
        }
        catch(FatalLoadException e)
        {
            Debug.LogError(e.Message);
            // handle fatal error
            FatalError = true;
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message + "\n" + e.StackTrace);
            FatalError = true;
        }

        // If nothing above is genuinely async yet, this yields a frame so callers
        // can safely await this method without blocking. Remove once real awaits exist below.
        await Task.Yield();
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


    public void FlashNotification()
    {
        _taskbarFlasher.FlashTaskbarIconUntilFocused();
    }



    public void SetHasSaveFile()
    {
        hasSaveFile = true;
    }
}
