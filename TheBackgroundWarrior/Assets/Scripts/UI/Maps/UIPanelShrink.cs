using UnityEngine;
using UnityEngine.UI;

public class UIPanelShrink : MonoBehaviour
{
    [SerializeField] Sprite spriteShrink;
    [SerializeField] Sprite spriteExpand;
    [SerializeField] Image imageButton;
    [SerializeField] GameObject[] objectsToHide;

    private bool isExpanded;

    [Header("Notification")]
    [SerializeField] UITab tabButton;
    [SerializeField] GameObject notificationObj;

    private void OnDestroy()
    {
        QuestManager.Instance.OnNeedNotification -= EnableNotification;
        tabButton.OnDeselected -= DisableNotification;
    }

    private void Awake()
    {
        QuestManager.Instance.OnNeedNotification += EnableNotification;
        tabButton.OnDeselected += DisableNotification;
    }

    private void Start()
    {
        isExpanded = true;

        UpdateButtonUI();
    }

    public void OnButtonShrink()
    {
        AudioManager.Instance.PlayClickUI();

        isExpanded = !isExpanded;

        if (isExpanded)
        {
            DisableNotification();
        }

        foreach (var item in objectsToHide)
        {
            item.SetActive(isExpanded);
        }

        UpdateButtonUI();
    }

    private void UpdateButtonUI()
    {
        imageButton.sprite = isExpanded ? spriteShrink : spriteExpand;
    }




    public void EnableNotification()
    {
        if (isExpanded) return;
        notificationObj.SetActive(true);
    }

    public void DisableNotification()
    {
        notificationObj.SetActive(false);
    }
}
