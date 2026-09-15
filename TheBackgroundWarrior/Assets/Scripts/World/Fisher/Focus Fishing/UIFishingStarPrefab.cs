using UnityEngine;

public class UIFishingStarPrefab : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void ResetStar()
    {
        _animator.SetTrigger("Idle");
    }

    public void CollectStar()
    {
        _animator.SetTrigger("Collected");
    }

    public void LostStar()
    {
        _animator.SetTrigger("Idle");
    }
}
