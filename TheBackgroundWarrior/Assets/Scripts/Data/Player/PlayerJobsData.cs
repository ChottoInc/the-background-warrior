using System.Collections.Generic;

public class PlayerJobsData
{
    private List<UtilsPlayer.PlayerJob> availableJobs;


    // ------- CONDITIONS --------- //

    private bool isBlacksmithUnlocked;
    private bool isFarmerUnlocked;


    public List<UtilsPlayer.PlayerJob> AvailableJobs => availableJobs;


    public bool IsBlacksmithUnlocked => isBlacksmithUnlocked;
    public bool IsFarmerUnlocked => isFarmerUnlocked;


    public PlayerJobsData()
    {
        GenerateBaseStats();
    }

    public PlayerJobsData(PlayerJobsSaveData saveData)
    {
        GenerateBaseStats();

        foreach (var job in saveData.availableJobs)
        {
            if (!availableJobs.Contains((UtilsPlayer.PlayerJob)job))
            {
                availableJobs.Add((UtilsPlayer.PlayerJob)job);
            }
        }

        // Add here check on available job to set the unlock, so there is no need to save extra space in memory

        if(availableJobs.Contains(UtilsPlayer.PlayerJob.Blacksmith))
        {
            isBlacksmithUnlocked = true;
        }

        if (availableJobs.Contains(UtilsPlayer.PlayerJob.Farmer))
        {
            isFarmerUnlocked = true;
        }
    }

    private void GenerateBaseStats()
    {
        availableJobs = new List<UtilsPlayer.PlayerJob>
        {
            UtilsPlayer.PlayerJob.None,
            UtilsPlayer.PlayerJob.Warrior,
            UtilsPlayer.PlayerJob.Miner,
            UtilsPlayer.PlayerJob.Fisher,
            //UtilsPlayer.PlayerJob.Farmer,
            //UtilsPlayer.PlayerJob.Blacksmith
        };

        isBlacksmithUnlocked = false;
    }


    public void AddAvailableJob(UtilsPlayer.PlayerJob job)
    {
        availableJobs.Add(job);

        switch(job)
        {
            case UtilsPlayer.PlayerJob.Blacksmith: isBlacksmithUnlocked = true; break;
            case UtilsPlayer.PlayerJob.Farmer: isFarmerUnlocked = true; break;
        }

        PlayerManager.Instance.SaveJobsData();
    }
}
