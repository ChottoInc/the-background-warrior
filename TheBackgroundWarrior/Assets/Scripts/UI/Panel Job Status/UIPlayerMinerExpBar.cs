using UnityEngine;

public class UIPlayerMinerExpBar : GenericBar
{
    [Space(10)]
    [SerializeField] PlayerMiner player;

    private void OnDestroy()
    {
        if (player.PlayerData != null)
            player.PlayerData.OnAddedExp -= UpdateBar;
    }

    public void Setup()
    {
        player.PlayerData.OnAddedExp += UpdateBar;
        UpdateBar();
    }

    private void UpdateBar()
    {
        SetMaxValue(player.PlayerData.ExpToNextLevel);
        SetCurrentValue(player.PlayerData.CurrentExp);
    }
}
