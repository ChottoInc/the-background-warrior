using UnityEngine;

public class MinerSmasher : MonoBehaviour
{
    [SerializeField] PlayerMiner player;

    public void PerformSmash()
    {
        player.PerformSmash();
    }
}
