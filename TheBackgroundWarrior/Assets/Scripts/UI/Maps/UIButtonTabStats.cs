using UnityEngine;

public class UIButtonTabStats : MonoBehaviour
{
    [SerializeField] UITab tabButton;
    [SerializeField] GameObject notificationObj;

    private Player _player;


    private void OnDestroy()
    {
        if(_player != null)
            _player.OnLevelUp += EnableNotification;

        tabButton.OnDeselected -= DisableNotification;
    }

    private void Awake()
    {
        _player = FindFirstObjectByType<Player>();

        _player.OnLevelUp += EnableNotification;
        tabButton.OnDeselected += DisableNotification;
    }

    public void EnableNotification()
    {
        notificationObj.SetActive(true);
    }

    public void DisableNotification()
    {
        notificationObj.SetActive(false);
    }
}
