using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFisherSessionFishPrefab : MonoBehaviour
{
    [SerializeField] TMP_Text _textGroupName;
    [SerializeField] Image _imageCaught;
    [SerializeField] Sprite _spriteCaught;
    [SerializeField] Sprite _spriteFled;

    public void Setup(string name, bool isCaught)
    {
        _textGroupName.text = name;
        _imageCaught.sprite = isCaught ? _spriteCaught : _spriteFled;
    }
}
