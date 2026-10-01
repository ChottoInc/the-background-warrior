using TMPro;
using UnityEngine;

public class UITabHelpMiner : UITabWindow
{
    [SerializeField] TMP_Text _textHelp;
    [SerializeField] string _textId;

    private void OnDestroy()
    {
        SettingsManager.Instance.OnLanguageChange -= RefreshText;
    }

    private void Start()
    {
        RefreshText();
        SettingsManager.Instance.OnLanguageChange += RefreshText;
    }

    private void RefreshText()
    {
        // insert rock metal chances
        _textHelp.text = string.Format(UtilsText.AllText[_textId], 
            UtilsGeneral.FormatDecimal(UtilsMiner.ROCK_METAL_MAX_LEVEL_COPPER * 100f),
            UtilsGeneral.FormatDecimal(UtilsMiner.ROCK_METAL_MAX_LEVEL_IRON * 100f),
            UtilsGeneral.FormatDecimal(UtilsMiner.ROCK_METAL_MAX_LEVEL_BRONZE * 100f),
            UtilsGeneral.FormatDecimal(UtilsMiner.ROCK_METAL_MAX_LEVEL_SILVER * 100f),
            UtilsGeneral.FormatDecimal(UtilsMiner.ROCK_METAL_MAX_LEVEL_GOLD * 100f)
            );
    }
}
