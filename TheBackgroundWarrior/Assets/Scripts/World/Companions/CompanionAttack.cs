using UnityEngine;

public class CompanionAttack : MonoBehaviour
{
    private Companion companion;

    private void Awake()
    {
        companion = GetComponentInParent<Companion>();
    }

    public void CallExternalAttack()
    {
        if(companion != null)
        {
            companion.ExternalAttack();
        }
    }
}
