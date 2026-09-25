using UnityEngine;

public static class UtilsCombatMap
{
    private static CombatMapSO[] maps;
    private static MapToEnemiesSO[] mapEnemies;
    private static MapToCardsSO[] mapCards;


    public static void Initialize()
    {
        maps = LoadMaps();
        mapEnemies = LoadMapEnemies();
        mapCards = LoadMapCards();
    }

    private static CombatMapSO[] LoadMaps()
    {
        return Resources.LoadAll<CombatMapSO>("Data/CombatMaps");
    }


    public static CombatMapSO[] GetAllMaps()
    {
        return maps;
    }

    public static CombatMapSO GetMapById(int id)
    {
        foreach (var map in maps)
        {
            if(map.IdMap == id)
                return map;
        }
        return null;
    }



    private static MapToEnemiesSO[] LoadMapEnemies()
    {
        return Resources.LoadAll<MapToEnemiesSO>("Data/MapToEnemies");
    }


    public static MapToEnemiesSO[] GetAllMapEnemies()
    {
        return mapEnemies;
    }

    public static MapToEnemiesSO GetEnemiesByMap(int id)
    {
        foreach (var mapEnemy in mapEnemies)
        {
            if (mapEnemy.MapSO.IdMap == id)
                return mapEnemy;
        }
        return null;
    }

    public static bool IsEnemyInMap(int idEnemy, MapToEnemiesSO mapEnemySO)
    {
        foreach(var possibleEnemy in mapEnemySO.PossibleEnemies)
        {
            if (possibleEnemy.value.Id == idEnemy)
                return true;
        }
        return false;
    }



    private static MapToCardsSO[] LoadMapCards()
    {
        return Resources.LoadAll<MapToCardsSO>("Data/MapToCards");
    }


    public static MapToCardsSO[] GetAllMapCards()
    {
        return mapCards;
    }

    public static MapToCardsSO GetCardsByMap(int id)
    {
        foreach (var mapCard in mapCards)
        {
            if (mapCard.MapSO.IdMap == id)
                return mapCard;
        }
        return null;
    }

    public static bool IsCardsInMap(int idCard, MapToCardsSO mapCardsSO)
    {
        foreach (var possibleCard in mapCardsSO.PossibleCards)
        {
            if (possibleCard.value.Id == idCard)
                return true;
        }
        return false;
    }




    public enum MapDifficulty
    {
        VeryEasy,   // 0
        Easy,       // 1
        Normal,     // 2
        Hard,       // 3
        VeryHard    // 4
    }

    // Used to calculate from base exp given
    public static float[] DifficultyExpMultiplier = 
    {
        0.55f,   // VeryEasy
        0.65f,   // Easy
        0.75f,   // Normal
        0.85f,   // Hard
        1.0f    // VeryHard
    };

    // Used to calculate actual stats
    public static float[] DifficultyStatMultiplier = 
    {
        0.7f,
        0.85f,
        1.0f,
        1.2f,
        1.4f
    };


    /// <summary>
    /// Exp given by the dead monsters
    /// </summary>
    public static int GetEnemyExp(int enemyLevel, MapDifficulty difficulty)
    {
        float baseExp = 7f;
        float exp = baseExp
                    * Mathf.Pow(enemyLevel, 1.035f)
                    * DifficultyExpMultiplier[(int)difficulty];

        return Mathf.FloorToInt(exp);
        //return Mathf.FloorToInt(exp * 100);
    }
}
