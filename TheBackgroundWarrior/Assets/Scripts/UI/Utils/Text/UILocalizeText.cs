using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UILocalizeText : MonoBehaviour
{
    [SerializeField] string textId;

    private TMP_Text _text;

    private bool _isInit;
    private bool _isListening;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        // if not first scene, load text at start
        InitText();
    }

    private void OnDestroy()
    {
        SettingsManager.Instance.OnLanguageChange -= RefreshText;
    }

    private void Update()
    {
        InitText();
        InitListening();
    }


    private void InitText()
    {
        if (InitializerManager.Instance.HasCheckFiles && !_isInit)
        {
            _isInit = true;
            RefreshText();
        }
    }

    private void InitListening()
    {
        if (_isListening) return;

        _isListening = true;
        SettingsManager.Instance.OnLanguageChange += RefreshText;
    }

    private void RefreshText()
    {
        _text.text = UtilsText.AllText[textId];
    }
}
