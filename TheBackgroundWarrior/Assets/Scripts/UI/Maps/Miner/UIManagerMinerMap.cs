using UnityEngine;

public class UIManagerMinerMap : UIManager
{
    [Space(10)]
    [SerializeField] UIPlayerMinerExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
