using UnityEngine;

public class UIManagerFarmerMap : UIManager
{
    [Space(10)]
    [SerializeField] UIPlayerFarmerExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
