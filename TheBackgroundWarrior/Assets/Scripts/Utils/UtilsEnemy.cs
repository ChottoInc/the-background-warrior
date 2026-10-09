using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UtilsEnemy
{
    public enum EnemyType 
    { 
        ArmoredOrc, 
        ArmoredSkeleton, 
        EliteOrc, 
        GreatswordSkeleton, 
        Orc, 
        OrcRider, 
        Skeleton, 
        SkeletonArcher, 
        Slime, 
        Werebear, 
        Werewolf,
        AlphaWerewolf,
        ArmoredAxeman,
        CorruptedPriest,
        DarkWizard,
        OrcKing,
        RampagingSkeleton,
        ScorchingSlime,
        UndeadSoldier
    }


    public enum EnemyAffectedStatus { None, Chilled, Poisoned }
    public const float TIMER_CHILLED_STATUS = 20f;
    public const float TIMER_POISONED_STATUS = 20f;


    private static Dictionary<int, ListableGameDataSO> dictEnemies;
    private static Dictionary<int, ListableGameDataSO> dictEnemyTypeConverters;


    public static void Initialize()
    {
        LoadEnemies();
        LoadEnemyConverters();
    }

    private static void LoadEnemies()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Enemies/ContainerGameData_Enemies");
        dictEnemies = container.Entries.ToDictionary(e => e.Id);
    }

    public static EnemySO[] GetAllEnemies()
    {
        return dictEnemies.OfType<EnemySO>().ToArray();
    }

    public static EnemySO GetEnemyById(int id)
    {
        return UtilsGeneral.GetGameDataSO<EnemySO>(id, dictEnemies);
    }


    private static void LoadEnemyConverters()
    {
        var container = Resources.Load<ContainerGameDataSO>("Data/Enemies/ContainerGameData_EnemyTypeConverters");
        dictEnemyTypeConverters = container.Entries.ToDictionary(e => e.Id);
    }

    public static EnemyTypeConverterSO GetTypeConverterByType(int id)
    {
        return UtilsGeneral.GetGameDataSO<EnemyTypeConverterSO>(id, dictEnemyTypeConverters);
    }
}
