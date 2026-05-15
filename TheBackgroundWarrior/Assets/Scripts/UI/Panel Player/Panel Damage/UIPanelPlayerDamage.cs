using UnityEngine;

public class UIPanelPlayerDamage : UIBasePanelDamage
{
    [SerializeField] PlayerFight player;

    protected override void Awake()
    {
        player.OnTakeDamage += ShowDamage;
    }

    protected override void OnDestroy()
    {
        player.OnTakeDamage -= ShowDamage;
    }
}
