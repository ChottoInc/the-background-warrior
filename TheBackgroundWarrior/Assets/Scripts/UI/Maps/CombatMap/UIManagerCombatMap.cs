using UnityEngine;

public class UIManagerCombatMap : UIManager
{
    [Space(10)]
    [SerializeField] UIPlayerFightExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
