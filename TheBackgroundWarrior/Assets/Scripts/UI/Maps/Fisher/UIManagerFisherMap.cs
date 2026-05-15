using UnityEngine;

public class UIManagerFisherMap : UIManager
{
    [Space(10)]
    [SerializeField] UIPlayerFisherExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
