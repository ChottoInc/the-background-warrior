using UnityEngine;

public class UIPlayerBlacksmithExpBar : GenericBar
{
    [Space(10)]
    [SerializeField] PlayerBlacksmith player;

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
