

public class PlayerBardSaveData
{
    public float playedMusicTime;

    public bool isRandom;
    public bool isSingleLoop;

    public PlayerBardSaveData() { }

    public PlayerBardSaveData(PlayerBardData data)
    {
        playedMusicTime = data.PlayedMusicTime;

        isRandom = data.IsRandom;
        isSingleLoop = data.IsSingleLoop;
    }
}
