using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIBarrier : MonoBehaviour
{
    private CanvasGroup group;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    public void EnableBarrier(bool enable)
    {
        group.interactable = enable;
        group.blocksRaycasts = enable;
    }
}
