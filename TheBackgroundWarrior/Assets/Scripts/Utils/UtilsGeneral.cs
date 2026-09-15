using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public static class UtilsGeneral
{
    public const float TIMER_5MIN_IN_SECONDS = 300f;
    public const float TIMER_20SECONDS = 20f;
    public const float TIMER_1SECONDS = 1f;


    public enum Language { Eng, Ita }


    public enum DayMoment { Morning, Afternoon, Evening, Night }




    public static void Initialize()
    {
        RefreshAll();
    }

    public static void RefreshAll()
    {
        RefreshTexts();
        RefreshTutorialDictionaries();
    }

    public static void RefreshTexts()
    {
        TUTORIAL_INTRO_1 = UtilsText.AllText[UtilsText.text_tutorial_intro_1];
        TUTORIAL_INTRO_2 = UtilsText.AllText[UtilsText.text_tutorial_intro_2];
        TUTORIAL_INTRO_3 = UtilsText.AllText[UtilsText.text_tutorial_intro_3];
        TUTORIAL_INTRO_4 = UtilsText.AllText[UtilsText.text_tutorial_intro_4];
        TUTORIAL_INTRO_5 = UtilsText.AllText[UtilsText.text_tutorial_intro_5];
        TUTORIAL_INTRO_6 = UtilsText.AllText[UtilsText.text_tutorial_intro_6];
        TUTORIAL_INTRO_7 = UtilsText.AllText[UtilsText.text_tutorial_intro_7];
        TUTORIAL_INTRO_8 = UtilsText.AllText[UtilsText.text_tutorial_intro_8];
        TUTORIAL_INTRO_9 = UtilsText.AllText[UtilsText.text_tutorial_intro_9];
        TUTORIAL_INTRO_10 = UtilsText.AllText[UtilsText.text_tutorial_intro_10];
        TUTORIAL_INTRO_11 = UtilsText.AllText[UtilsText.text_tutorial_intro_11];
    }

    public static void RefreshTutorialDictionaries()
    {
        TutorialIntroDialogues = new ReadOnlyCollection<TutorialDialogueNeedPos>(
        new[]
        {
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_1, false),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_2, false),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_3, false),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_4, false),

            new TutorialDialogueNeedPos(TUTORIAL_INTRO_5, true),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_6, false),

            new TutorialDialogueNeedPos(TUTORIAL_INTRO_7, true),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_8, false),

            new TutorialDialogueNeedPos(TUTORIAL_INTRO_9, true),

            new TutorialDialogueNeedPos(TUTORIAL_INTRO_10, true),

            new TutorialDialogueNeedPos(TUTORIAL_INTRO_11, true),
        });

        DictTutorials = new Dictionary<int, IList<TutorialDialogueNeedPos>>()
        {
            { ID_INTRO_TUTORIAL, TutorialIntroDialogues }
        };
    }

    public static T GetGameDataSO<T>(int id, Dictionary<int, ListableGameDataSO> dict) where T : ListableGameDataSO
    {
        return dict.TryGetValue(id, out var entry) ? entry as T : null;
    }

    public static T GetGameDataSO<T>(string id, Dictionary<string, T> dict) where T : ListableGameDataSO
    {
        return dict.TryGetValue(id, out var entry) ? entry as T : null;
    }

    public static bool GetRandomSuccessFromValue(float value)
    {
        if (Random.value <= value) return true;
        return false;
    }

    public static float GetRandomValueBtwValues(float val1, float val2)
    {
        return Random.Range(val1, val2);
    }


    public static List<string> GetNonSharedValues(List<string> list1, List<string> list2)
    {
        List<string> result = new List<string>();

        foreach (string s in list2)
        {
            if(!list1.Contains(s))
                result.Add(s);
        }
        return result;
    }


    #region DAY MOMENT

    public static DayMoment GetDayMoment()
    {
        int hour = System.DateTime.Now.Hour;

        if (hour > 6 && hour <= 12)
        {
            return DayMoment.Morning;
        }
        else if (hour > 12 && hour <= 18)
        {
            return DayMoment.Afternoon;
        }
        else
        {
            return DayMoment.Night;
        }
    }

    public static string GetDayMomentName(DayMoment dayMoment)
    {
        switch(dayMoment)
        {
            default:
            case DayMoment.Morning: return UtilsText.AllText[UtilsText.text_name_daymoment_morning];
            case DayMoment.Afternoon: return UtilsText.AllText[UtilsText.text_name_daymoment_afternoon];
            case DayMoment.Evening: return UtilsText.AllText[UtilsText.text_name_daymoment_evening];
            case DayMoment.Night: return UtilsText.AllText[UtilsText.text_name_daymoment_night];
        }
    }

    #endregion


    #region SETTINGS

    public enum AutocloseTimerType { MINS5, MINS10, MINS20, MINS30, MINS60, MINS120 }

    public static string GetAutocloseTimerText(AutocloseTimerType timerType)
    {
        string text = string.Empty;
        switch(timerType)
        {
            case AutocloseTimerType.MINS5: text += "5 "; break;
            case AutocloseTimerType.MINS10: text += "10 "; break;
            case AutocloseTimerType.MINS20: text += "20 "; break;
            case AutocloseTimerType.MINS30: text += "30 "; break;
            case AutocloseTimerType.MINS60: text += "60 "; break;
            case AutocloseTimerType.MINS120: text += "120 "; break;
        }

        return text + UtilsText.AllText[UtilsText.text_settings_gameplay_option_minutes];
    }

    public static float GetAutocloseTimerByType(AutocloseTimerType timerType)
    {
        switch (timerType)
        {
            default:
            case AutocloseTimerType.MINS5: return 300f;
            case AutocloseTimerType.MINS10: return 600f;
            //case AutocloseTimerType.MINS10: return 20f; //  DEBUG
            case AutocloseTimerType.MINS20: return 1200f;
            case AutocloseTimerType.MINS30: return 1800f;
            case AutocloseTimerType.MINS60: return 3600f;
            case AutocloseTimerType.MINS120: return 7200f;
        }
    }

    #endregion


    #region FILE READING

    public static List<string> GetFileStrings(string filename)
    {
        var dataset = Resources.Load<TextAsset>(filename);
        var dataLines = dataset.text.Split("\r\n");

        List<string> strings = new List<string>();

        // for every line of file
        // start from 1, excluding headers
        for (int i = 1; i < dataLines.Length; i++)
        {
            string res = dataLines[i]
                .Replace("…", "...")
                .Replace("’", "'");

            strings.Add(res);
        }

        return strings;
    }

    public static Dictionary<string, string> GetDictionaryFromList(List<string> list)
    {
        Dictionary<string,string> result = new Dictionary<string, string>();

        foreach (var row in list)
        {
            string part1 = "";
            string part2 = "";
            int dividerIndex = 0;

            // get string id
            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] != ',')
                {
                    part1 += row[i];
                }
                else
                {
                    dividerIndex = i;
                    break;
                }
            }

            // skip divider index and get string value
            dividerIndex++;

            bool hasApix = false;
            if (row[dividerIndex] == '"')
            {
                dividerIndex++;
                hasApix = true;
            }

            for (; dividerIndex < row.Length; dividerIndex++)
            {
                if (!hasApix)
                {
                    part2 += row[dividerIndex];

                }
                else
                {
                    if(dividerIndex != row.Length - 1)
                    {
                        part2 += row[dividerIndex];
                    }
                }

                if (dividerIndex < row.Length - 1)
                {
                    if (row[dividerIndex] == '"' && row[dividerIndex + 1] == '"')
                        dividerIndex++;
                }
            }

            result.Add(part1, part2);
        }

        return result;
    }

    #endregion



    /// <summary>
    /// Are there any common values between a and b?
    /// </summary>
    public static bool SharesAnyValueWith<T>(this IEnumerable<T> a, IEnumerable<T> b)
    {
        return a.Intersect(b).Any();
    }

    /// <summary>
    /// If number has no decimal will return the whole value, or with at max one decimal if it has
    /// </summary>
    public static string FormatDecimal(float value)
    {
        return value % 1 == 0 ? value.ToString("0") : value.ToString("0.0");
    }

    #region GENERAL CHANCES

    [System.Serializable]
    public struct GeneralChances<T>
    {
        public T value;
        public int chanches;
    }

    public static T GetRandomValueFromGeneralChanches<T>(GeneralChances<T>[] array)
    {
        float randValue = Random.value;
        float tempSumChance = 0;

        T result = default;

        for (int i = 0; i < array.Length; i++)
        {
            tempSumChance += (float)array[i].chanches / 100f;
            if (randValue <= tempSumChance)
            {
                result = array[i].value;
                break;
            }
        }

        return result;
    }

    #endregion


    #region TUTORIAL

    public const int ID_INTRO_TUTORIAL = 0;


    private static string TUTORIAL_INTRO_1 = "This is the background Warrior.";
    private static string TUTORIAL_INTRO_2 = "He will keep fighting even when you are not looking.";
    private static string TUTORIAL_INTRO_3 = "Defeat monsters to advance the stages.";
    private static string TUTORIAL_INTRO_4 = "Once all the stages are cleared, a new map will be unlocked.";
            
    private static string TUTORIAL_INTRO_5 = "On the right side of the screen you will find various menus.";
    private static string TUTORIAL_INTRO_6 = "Click on the STATS icon to increase stats level.";
            
    private static string TUTORIAL_INTRO_7 = "If you want to select a different job, click on the JOB icon.";
    private static string TUTORIAL_INTRO_8 = "You can find more informations about jobs in the HELP section of the SETTINGS menu.";
            
            
    private static string TUTORIAL_INTRO_9 = "Check your items using the INVENTORY icon.";
            
    private static string TUTORIAL_INTRO_10 = "Click on the QUESTS icon to check your progress and claim your rewards.";
            
    private static string TUTORIAL_INTRO_11 = "Spend Bits in the shop to purchase cards and jobs.";

    /// <summary>
    /// Struct containing the dialogue and if the text panel need to move to next position
    /// </summary>
    public struct TutorialDialogueNeedPos
    {
        private readonly string dialogue;
        private readonly bool need;

        public TutorialDialogueNeedPos(string dialgoue, bool need)
        {
            this.dialogue = dialgoue;
            this.need = need;
        }

        public string Dialogue => dialogue;
        public bool Need => need;
    }

    // Tutorial intro
    public static IList<TutorialDialogueNeedPos> TutorialIntroDialogues = new ReadOnlyCollection<TutorialDialogueNeedPos>(
        new[]
        {
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_1, false),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_2, false),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_3, false),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_4, false),

            new TutorialDialogueNeedPos(TUTORIAL_INTRO_5, true),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_6, false),

            new TutorialDialogueNeedPos(TUTORIAL_INTRO_7, false),
            new TutorialDialogueNeedPos(TUTORIAL_INTRO_8, true),

            new TutorialDialogueNeedPos(TUTORIAL_INTRO_9, true),

            new TutorialDialogueNeedPos(TUTORIAL_INTRO_10, true),

            new TutorialDialogueNeedPos(TUTORIAL_INTRO_11, true),
        });
    

    // Use to get all the dialogue for a specific tutorial
    public static Dictionary<int, IList<TutorialDialogueNeedPos>> DictTutorials = new Dictionary<int, IList<TutorialDialogueNeedPos>>()
    {
        { ID_INTRO_TUTORIAL, TutorialIntroDialogues }
    };

    #endregion



    public class UIStatMultInfo
    {
        public string statName;
        public float multValue;

        public UIStatMultInfo(string statName, float multValue)
        {
            this.statName = statName;
            this.multValue = multValue;
        }
    }
}
