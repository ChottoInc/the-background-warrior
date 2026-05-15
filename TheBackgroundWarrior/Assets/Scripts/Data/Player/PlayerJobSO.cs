using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Job/Job Data", fileName = "JobData_")]
public class PlayerJobSO : ScriptableObject
{
    [SerializeField] UtilsPlayer.PlayerJob job;
    [SerializeField] UtilsPlayer.PlayerJob[] requiredJobs;

    [Space(10)]
    [SerializeField] string jobName;
    [SerializeField] string unlockConditions;


    public UtilsPlayer.PlayerJob Job => job;
    public UtilsPlayer.PlayerJob[] RequiredJobs => requiredJobs;

    public string JobName => jobName;
    public string JobUnlockConditions => unlockConditions;
}
