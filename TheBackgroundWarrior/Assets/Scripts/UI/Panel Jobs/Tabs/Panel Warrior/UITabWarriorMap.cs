using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITabWarriorMap : MonoBehaviour
{
    [SerializeField] TMP_Text textMapName;
    [SerializeField] TMP_Text textMapStage;

    [Space(10)]
    [SerializeField] Button buttonMap;
    [SerializeField] GameObject availableBarrier;

    private UISelfIncreaseObject incrObj;

    [Space(10)]
    [SerializeField] Image imageToHighlight;
    [SerializeField] Color colorSelected;

    [Header("Tooltip")]
    [SerializeField] Transform tooltipPosition;
    [SerializeField] float timerTooltip = 0.35f;

    private bool isShowingTooltip;

    private UITabJobWarrior uiTabWarrior;
    private CombatMapSO mapSO;
    private ScrollRect scroll;

    private void Awake()
    {
        incrObj = buttonMap.GetComponent<UISelfIncreaseObject>();
    }

    public void Setup(UITabJobWarrior uiTabWarrior, CombatMapSO mapSO, ScrollRect scroll)
    {
        this.uiTabWarrior = uiTabWarrior;
        this.mapSO = mapSO;
        this.scroll = scroll;

        textMapName.text = mapSO.MapName;

        CombatMapSaveData mapData = SettingsManager.Instance.GetCombatMapSaveData(mapSO);
        textMapStage.text = string.Format("Stage: {0}/{1}", mapData.currentStage, mapSO.Stages);

        if (PlayerManager.Instance.PlayerFightData.AvailableMaps.Contains(mapSO.IdMap))
        {
            availableBarrier.SetActive(false);

            incrObj.Resize();
        }
        else
        {
            availableBarrier.SetActive(true);
            textMapStage.gameObject.SetActive(false);

            buttonMap.interactable = false;
        }
    }

    public void OnButtonClick()
    {
        uiTabWarrior.OnMapSelected(mapSO.MapSceneName, mapSO.IdMap);

        imageToHighlight.color = colorSelected;
    }

    public void OnPointerEnter()
    {
        if (isShowingTooltip) return;

        StartCoroutine(CoShowMapMonsters());
    }

    public void OnPointerExit()
    {
        StopAllCoroutines();

        if (!isShowingTooltip) return;

        isShowingTooltip = false;

        UITooltipManager.Instance.Hide(UITooltipManager.ID_SHOW_TEXT, true);
    }

    private IEnumerator CoShowMapMonsters()
    {
        yield return new WaitForSecondsRealtime(timerTooltip);

        isShowingTooltip = true;

        string possibleMonsters = string.Empty;

        MapToEnemiesSO mapEnemiesSO = UtilsCombatMap.GetEnemiesByMap(mapSO.IdMap);

        for (int i = 0; i < mapEnemiesSO.PossibleEnemies.Length; i++)
        {
            possibleMonsters += mapEnemiesSO.PossibleEnemies[i].value.EnemyName;

            // add new line only when not last possible
            if (i < mapEnemiesSO.PossibleEnemies.Length - 1)
            {
                possibleMonsters += "\n";
            }
        }

        string text = string.Format(
            "Possible monsters:\n" +
            "{0}",
            possibleMonsters);


        TooltipManagerData tooltipData = new TooltipManagerData();
        tooltipData.idTooltip = UITooltipManager.ID_SHOW_TEXT;
        tooltipData.text = text;
        UITooltipManager.Instance.Show(tooltipData, tooltipPosition.position, true, 35f);
    }

    public void OnBeginDrag(BaseEventData data)
    {
        if (scroll != null)
        {
            PointerEventData pointerData = (PointerEventData)data;
            scroll.OnBeginDrag(pointerData);
        }
    }

    public void OnDrag(BaseEventData data)
    {
        if (scroll != null)
        {
            PointerEventData pointerData = (PointerEventData)data;
            scroll.OnDrag(pointerData);
        }
    }

    public void OnEndDrag(BaseEventData data)
    {
        if (scroll != null)
        {
            PointerEventData pointerData = (PointerEventData)data;
            scroll.OnEndDrag(pointerData);
        }
    }
}
