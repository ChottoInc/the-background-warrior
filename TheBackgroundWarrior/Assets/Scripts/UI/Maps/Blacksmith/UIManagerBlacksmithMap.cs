using UnityEngine;

public class UIManagerBlacksmithMap : UIManager
{
    [Space(10)]
    [SerializeField] UIPlayerBlacksmithExpBar playerExpBar;

    public override void Setup()
    {
        playerExpBar.Setup();
    }
}
