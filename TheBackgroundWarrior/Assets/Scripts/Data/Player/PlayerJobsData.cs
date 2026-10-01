using System;
using System.Collections.Generic;

public class PlayerJobsData
{
    private List<UtilsPlayer.PlayerJob> availableJobs;


    // ------- CONDITIONS --------- //

    public List<UtilsPlayer.PlayerJob> AvailableJobs => availableJobs;

    public bool IsBlacksmithUnlocked { get; private set; }
    public bool IsFarmerUnlocked { get; private set; }
    public bool IsMageUnlocked { get; private set; }
    public bool IsAlchemistUnlocked { get; private set; }
    public bool IsNecromancerUnlocked { get; private set; }
    public bool IsBardUnlocked { get; private set; }


    public event Action<UtilsPlayer.PlayerJob> OnJobUnlocked;


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
            IsBlacksmithUnlocked = true;
        }

        if (availableJobs.Contains(UtilsPlayer.PlayerJob.Farmer))
        {
            IsFarmerUnlocked = true;
        }

        if (availableJobs.Contains(UtilsPlayer.PlayerJob.Mage))
        {
            IsMageUnlocked = true;
        }

        if (availableJobs.Contains(UtilsPlayer.PlayerJob.Alchemist))
        {
            IsAlchemistUnlocked = true;
        }

        if (availableJobs.Contains(UtilsPlayer.PlayerJob.Necromancer))
        {
            IsNecromancerUnlocked = true;
        }

        if (availableJobs.Contains(UtilsPlayer.PlayerJob.Bard))
        {
            IsBardUnlocked = true;
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
            //UtilsPlayer.PlayerJob.Mage
            //UtilsPlayer.PlayerJob.Alchemist,
            //UtilsPlayer.PlayerJob.Necromancer,
            //UtilsPlayer.PlayerJob.Bard,
        };

        IsBlacksmithUnlocked = false;
        IsFarmerUnlocked = false;
        IsMageUnlocked = false;
        IsAlchemistUnlocked = false;
        IsNecromancerUnlocked = false;
        IsBardUnlocked = false;
    }


    public void AddAvailableJob(UtilsPlayer.PlayerJob job)
    {
        if(!availableJobs.Contains(job))
            availableJobs.Add(job);

        switch(job)
        {
            case UtilsPlayer.PlayerJob.Blacksmith: IsBlacksmithUnlocked = true; break;
            case UtilsPlayer.PlayerJob.Farmer: IsFarmerUnlocked = true; break;
            case UtilsPlayer.PlayerJob.Mage: IsMageUnlocked = true; break;
            case UtilsPlayer.PlayerJob.Alchemist: IsAlchemistUnlocked = true; break;
            case UtilsPlayer.PlayerJob.Necromancer: IsNecromancerUnlocked = true; break;
            case UtilsPlayer.PlayerJob.Bard: IsBardUnlocked = true; break;
        }

        OnJobUnlocked?.Invoke(job);

        PlayerManager.Instance.SaveJobsData();
    }
}
