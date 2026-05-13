using UnityEngine;

public class UIBasePanelDamage : MonoBehaviour
{
    //[SerializeField] protected IDamageable damageableEntity;

    [Space(10)]
    [SerializeField] protected float moveTime = 0.5f;
    [SerializeField] protected Transform startContentPos;
    [SerializeField] protected Transform endContentPos;


    public float MoveTime => moveTime;
    public Transform StartContentPos => startContentPos;
    public Transform EndContentPos => endContentPos;


    [Space(10)]
    [SerializeField] protected UIAnimatedPanelDamage[] objectsToMove;


    protected virtual void OnDestroy()
    {
        //damageableEntity.OnTakeDamage -= ShowDamage;
    }

    protected virtual void Awake()
    {
        //damageableEntity.OnTakeDamage += ShowDamage;
    }


    protected virtual void ShowDamage(int damage)
    {
        if (!SettingsManager.Instance.IsDamageOn) return;

        // setup object
        var obj = GetFirstFreeObject();

        if(obj != null)
            obj.Animate(damage);
    }

    protected virtual UIAnimatedPanelDamage GetFirstFreeObject()
    {
        foreach (var obj in objectsToMove)
        {
            if (!obj.IsAnimating) return obj;
        }
        return null;
    }
}