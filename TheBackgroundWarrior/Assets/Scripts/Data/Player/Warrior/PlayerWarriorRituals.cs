using UnityEngine;

public class PlayerWarriorRituals : MonoBehaviour
{
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float _cooldownSummon = 30f;

    public float FinalCooldownSummon => _cooldownSummon - (_cooldownSummon * PlayerManager.Instance.PlayerNecromancerData.CurrentSummon);


    private float _timerSummon;

    public int MaxHorde { get; private set; }
    public int CurrentHorde { get; private set; }

    [Header("Summons")]
    [SerializeField] GameObject _normalSummon;
    [SerializeField] GameObject _bigSummon;

    private void Start()
    {
        _timerSummon = FinalCooldownSummon;

        // set max horde, minimum 1, up to 5 from stats, and 6 with ade ritual
        PlayerNecromancerData necroData = PlayerManager.Instance.PlayerNecromancerData;
        MaxHorde = (int)necroData.CurrentHorde + 1;
        if (necroData.IsAdeRitualUnlocked) MaxHorde++;

        //Debug.Log("Max horde: " + MaxHorde);
    }

    private void Update()
    {
        // if necromancer isn't unlocked returns
        if (!PlayerManager.Instance.PlayerJobsData.AvailableJobs.Contains(UtilsPlayer.PlayerJob.Necromancer))
            return;

        HandleSummon();
    }

    private void HandleSummon()
    {
        if (_timerSummon <= 0)
        {
            Summon();

            _timerSummon = FinalCooldownSummon;
        }
        else
        {
            _timerSummon -= Time.deltaTime;
        }
    }

    private void Summon()
    {
        // if horde already at max returns
        if (CurrentHorde >= MaxHorde) return;

        //Debug.Log("Summoning...");

        PlayerNecromancerData necromancerData = PlayerManager.Instance.PlayerNecromancerData;

        // set prefab to spawn
        GameObject prefabToSpawn;
        if (UtilsGeneral.GetRandomSuccessFromValue(necromancerData.CurrentLuck))
        {
            prefabToSpawn = _bigSummon;
            //Debug.Log("Summon big");
        }
        else
        {
            prefabToSpawn = _normalSummon;
            //Debug.Log("Summon normal");
        }

        // set data
        float atkPerc = necromancerData.CurrentMight;

        // base 35s, up to double
        float maxHp = 35f * (1f + necromancerData.CurrentLifespan);

        // check arise ritual
        if (necromancerData.IsAriseRitualUnlocked)
        {
            if (UtilsGeneral.GetRandomSuccessFromValue(0.2f))
            {
                atkPerc *= 2f;
            }
        }

        SummonData data = new SummonData(atkPerc, maxHp);

        GameObject spawned = Instantiate(prefabToSpawn, _spawnPoint.position, Quaternion.identity);
        if(spawned.TryGetComponent(out Summon summoned))
        {
            summoned.Setup(data);
        }

        CurrentHorde++;

        PlayerManager.Instance.OnSummonEvent(1);
    }

    public void DecreaseHorde()
    {
        CurrentHorde--;
    }
}
