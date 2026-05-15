using UnityEngine;

public class UIPanelDamage : UIBasePanelDamage
{
    [SerializeField] Enemy enemy;

    protected override void Awake()
    {
        enemy.OnTakeDamage += ShowDamage;
    }

    protected override void OnDestroy()
    {
        enemy.OnTakeDamage -= ShowDamage;
    }
}
