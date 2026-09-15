
public class PlayerBardData : BasePlayerData
{
    public float PlayedMusicTime { get; private set; }

    public bool IsRandom { get; private set; }
    public bool IsSingleLoop { get; private set; }

    public PlayerBardData()
    {
        GenerateBaseStats();
    }

    public PlayerBardData(PlayerBardSaveData saveData)
    {
        GenerateBaseStats();

        PlayedMusicTime = saveData.playedMusicTime;

        IsRandom = saveData.isRandom;
        IsSingleLoop = saveData.isSingleLoop;

        // if has some time add to buff
        Buff buff = new Buff(UtilsBuffs.BuffType.Inspiration, PlayedMusicTime);
        PlayerManager.Instance.PlayerBuffsData.AddBuff(buff);
    }

    private void GenerateBaseStats()
    {
        PlayedMusicTime = 0;

        IsRandom = false;
        IsSingleLoop = false;

        // set inspiration buff as default, need to check when expires because you actually don't remove it, just let it pending idle
        Buff buff = new Buff(UtilsBuffs.BuffType.Inspiration, 0);
        PlayerManager.Instance.PlayerBuffsData.AddBuff(buff);
    }

    public void AddPlayedTime(float val)
    {
        PlayedMusicTime += val;
        PlayerManager.Instance.SaveBardData();
    }

    public void SetIsSingleLoop(bool val)
    {
        IsSingleLoop = val;
        PlayerManager.Instance.SaveBardData();
    }

    public void SetIsRandom(bool val)
    {
        IsRandom = val;
        PlayerManager.Instance.SaveBardData();
    }
}
