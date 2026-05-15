using UnityEngine;

public class BlacksmithVFX : MonoBehaviour
{
    [SerializeField] PlayerBlacksmith player;

    public void PlayForgeVFX()
    {
        player.PlayForgeVFX();
    }
}
