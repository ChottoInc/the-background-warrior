using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public static class UtilsGeneral
{
    public const float TIMER_5MIN_IN_SECONDS = 300f;
    public const float TIMER_20SECONDS = 20f;

    public enum DayMoment { Morning, Afternoon, Evening, Night }

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
            case DayMoment.Morning: return "Morning";
            case DayMoment.Afternoon: return "Afternoon";
            case DayMoment.Evening: return "Evening";
            case DayMoment.Night: return "Night";
        }
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


    public static List<string> GetFileStrings(string filename)
    {
        var dataset = Resources.Load<TextAsset>(filename);
        var dataLines = dataset.text.Split("\r\n");

        List<string> strings = new List<string>();

        // for every line of file
        // start from 1, excluding headers
        for (int i = 1; i < dataLines.Length; i++)
        {
            strings.Add(dataLines[i]);
        }

        return strings;
    }






    public struct MyColors
    {
        public static Color CommonRarity
        {
            get
            {
                return new Color(255f / 255f, 195f / 255f, 95f / 255f, 1f);
            }
        }

        public static Color UncommonRarity
        {
            get
            {
                return new Color(96f / 255f, 180f / 255f, 255f / 255f, 1f);
            }
        }

        public static Color RareRarity
        {
            get
            {
                return new Color(255f / 255f, 125f / 255f, 95f / 255f, 1f);
            }
        }
    }


    public static Color GetColorByRarity(UtilsItem.CardRarity rarity)
    {
        switch(rarity)
        {
            default:
            case UtilsItem.CardRarity.Common: return MyColors.CommonRarity;
            case UtilsItem.CardRarity.Uncommon: return MyColors.UncommonRarity;
            case UtilsItem.CardRarity.Rare: return MyColors.RareRarity;
        }
    }

    /// <summary>
    /// Are there any common values between a and b?
    /// </summary>
    public static bool SharesAnyValueWith<T>(this IEnumerable<T> a, IEnumerable<T> b)
    {
        return a.Intersect(b).Any();
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


    private const string TUTORIAL_INTRO_1 = "This is the background Warrior.";
    private const string TUTORIAL_INTRO_2 = "He will keep fighting even when you are not looking.";
    private const string TUTORIAL_INTRO_3 = "Defeat monsters to advance the stages.";
    private const string TUTORIAL_INTRO_4 = "Once all the stages are cleared, a new map will be unlocked.";

    private const string TUTORIAL_INTRO_5 = "On the right side of the screen you will find various menus.";
    private const string TUTORIAL_INTRO_6 = "Click on the STATS icon to increase stats level.";

    private const string TUTORIAL_INTRO_7 = "If you want to select a different job, click on the JOB icon.";
    private const string TUTORIAL_INTRO_8 = "You can find more informations about jobs in the HELP section of the SETTINGS menu.";


    private const string TUTORIAL_INTRO_9 = "Check your items using the INVENTORY icon.";

    private const string TUTORIAL_INTRO_10 = "Click on the QUESTS icon to check your progress and claim your rewards.";

    private const string TUTORIAL_INTRO_11 = "Spend Bits in the shop to purchase cards and jobs.";

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
    public static readonly IList<TutorialDialogueNeedPos> TutorialIntroDialogues = new ReadOnlyCollection<TutorialDialogueNeedPos>(
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
    public static readonly Dictionary<int, IList<TutorialDialogueNeedPos>> DictTutorials = new Dictionary<int, IList<TutorialDialogueNeedPos>>()
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
