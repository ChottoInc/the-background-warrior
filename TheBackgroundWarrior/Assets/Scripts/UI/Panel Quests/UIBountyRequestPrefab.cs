using UnityEngine;

public class UIBountyRequestPrefab : MonoBehaviour
{
    [SerializeField] int slot;
    [SerializeField] UITabQuestsBounties tabBounties;

    public void OnButtonChoose()
    {
        AudioManager.Instance.PlayClickUI();

        tabBounties.OpenBountiesList(slot);
    }
}
