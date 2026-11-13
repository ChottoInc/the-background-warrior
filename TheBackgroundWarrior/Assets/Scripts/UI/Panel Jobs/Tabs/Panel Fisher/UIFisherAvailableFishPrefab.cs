using TMPro;
using UnityEngine;

public class UIFisherAvailableFishPrefab : MonoBehaviour
{
    [SerializeField] TMP_Text _textGroupName;
    [SerializeField] GameObject _imageCaught;


    public void Setup(string name, bool isCaught)
    {
        _textGroupName.text = name;
        _imageCaught.SetActive(isCaught);
    }
}
