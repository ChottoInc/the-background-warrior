using UnityEngine;

public class WarriorAttacker : MonoBehaviour
{
    [SerializeField] PlayerFight player;

    public void PerformAttack()
    {
        player.PerformAttack();
    }
}
