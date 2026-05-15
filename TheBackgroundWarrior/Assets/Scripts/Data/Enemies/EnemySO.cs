using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy/Enemy Data", fileName = "EnemyData_")]
public class EnemySO : ScriptableObject
{
    [SerializeField] string enemyPoolName;
    [SerializeField] string enemyName;
    [SerializeField] int id;

    [Space(10)]
    [SerializeField] float baseMaxHp = 30f;
    [SerializeField] float baseAtk = 3f;
    [SerializeField] float baseDef = 1f;
    [SerializeField] float baseCritDmg = 1.5f;


    public string EnemyPoolName => enemyPoolName.ToLower();
    public string EnemyName => enemyName;
    public int Id => id;

    public float BaseMaxHp => baseMaxHp;
    public float BaseAtk => baseAtk;
    public float BaseDef => baseDef;
    public float BaseCritDmg => baseCritDmg;



    public void SetPoolName(string poolName)
    {
        enemyPoolName = poolName;
    }

    public void SetEnemyName(string enemyName)
    {
        this.enemyName = enemyName;
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void SetBaseMaxhHp(float value)
    {
        baseMaxHp = value;
    }

    public void SetBaseAtk(float value)
    {
        baseAtk = value;
    }

    public void SetBaseDef(float value)
    {
        baseDef = value;
    }

    public void SetBaseCritDmg(float value)
    {
        baseCritDmg = value;
    }




    public override bool Equals(object other)
    {
        EnemySO otherEnemy = other as EnemySO;
        return id == otherEnemy.id;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
