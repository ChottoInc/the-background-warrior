using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsBuffs
{
    public enum BuffType
    {
        Greed = 0,                  // increase bit drop rates
        Veteran = 1,                // increase exp gain
        Storyteller = 2,            // increase card drop rates
        Stoned = 3,                 // increase ore drop rates
        Sailor = 4,                 // increase successfull hooks
        Dwarf = 5,                  // increase extra materials blacksmith
        Tamer = 6,                  // increase befriend chance
        Arcanist = 7,               // increase learn points gain, doubles

        MorningAngler = 30,         // find morning fishes
        AfternoonAngler = 31,       // find afternoon fishes
        NightAngler = 32,           // find night fishes

        IronSkin = 40,              // gain 20% max hp shield warrior

        Inspiration = 100,
    }

    private static Dictionary<int, ListableGameDataSO> _dictBuffToSprite;

    public static void Initialize()
    {
        LoadDictBuffToSprites();
    }

    private static void LoadDictBuffToSprites()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Player/ContainerGameData_BuffToSprites");
        _dictBuffToSprite = container.Entries.ToDictionary(e => e.Id);
    }

    public static BuffSO GetBuffSOByType(BuffType type)
    {
        return _dictBuffToSprite.Values.Cast<BuffSO>().Where(b => b.BuffType == type).First();
    }
}
