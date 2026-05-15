using UnityEngine;

public class UIInfiniteInputQuantity : MonoBehaviour
{
    [SerializeField] GameObject panelInfinite;
    [SerializeField] GameObject panelText;

    private bool isInfinite;

    public void UpdatePanelUI()
    {
        panelInfinite.SetActive(isInfinite);
        panelText.SetActive(!isInfinite);
    }

    public void SetInfinite(bool infinite)
    {
        isInfinite = infinite;
        UpdatePanelUI();
    }
}
