using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPanelTutorial : MonoBehaviour
{
    [SerializeField] bool skipTutorial;
    [SerializeField] GameObject content;

    [Space(10)]
    [SerializeField] Transform introTutorialPart1Pos;  // intro
    [SerializeField] Transform introTutorialPart2Pos;  // level button
    [SerializeField] Transform introTutorialPart3Pos;  // job button
    [SerializeField] Transform introTutorialPart4Pos;  // inventory
    [SerializeField] Transform introTutorialPart5Pos;  // quests
    [SerializeField] Transform introTutorialPart6Pos;  // shop

    private List<Transform> tutorialPositions;
    private int tutorialPosIndex;

    /*
     * intro dialogue player highlighted
     * button highlighted - manualy add canvasggroup
     * */
    [Space(10)]
    [SerializeField] SpriteRenderer player;

    private bool hasChangedPlayer;

    [SerializeField] GameObject buttonLevel;
    [SerializeField] GameObject buttonJob;
    [SerializeField] GameObject buttonInventory;
    [SerializeField] GameObject buttonQuests;
    [SerializeField] GameObject buttonShop;

    [Space(10)]
    [SerializeField] GameObject buttonSetting;
    [SerializeField] GameObject panelStage;
    [SerializeField] GameObject panelAutoBattle;
    [SerializeField] GameObject panelShrink;

    private List<GameObject> uiElementsToHighlight;
    private int elementHighlightedIndex;

    private GameObject lastUIElementHighlighted;
    private Canvas lastCanvasComponentAdded;


    [Space(10)]
    [SerializeField] float timeBtwChars = 0.05f;
    [SerializeField] GameObject panelText;
    [SerializeField] TMP_Text textTutorial;

    private IList<UtilsGeneral.TutorialDialogueNeedPos> currentDialogues;
    private int currentDialogueIndex;
    private bool isCurrentDialogueEnded;


    private int lastShownTutorial;


    public event Action<int> OnTutorialEnded;


    private void Start()
    {
        // check if need intro
        if (SettingsManager.Instance.HasSeenIntroTutorial || skipTutorial)
        {
            content.SetActive(false);
            return;
        }
        else
        {
            content.SetActive(true);
            HandleTutorial();
        }
    }

    private void HandleTutorial()
    {
        tutorialPositions = new List<Transform>()
        {
            introTutorialPart1Pos,
            introTutorialPart2Pos,
            introTutorialPart3Pos,
            introTutorialPart4Pos,
            introTutorialPart5Pos,
            introTutorialPart6Pos
        };

        uiElementsToHighlight = new List<GameObject>()
        {
            buttonLevel,
            buttonJob,
            buttonInventory,
            buttonQuests,
            buttonShop
        };

        

        DisableAllUI();

        StartDialoge();
    }

    private void StartDialoge()
    {
        // get dialogues
        lastShownTutorial = UtilsGeneral.ID_INTRO_TUTORIAL;
        currentDialogues = UtilsGeneral.DictTutorials[lastShownTutorial];
        currentDialogueIndex = 0;

        // true at start so you can start the dialogues
        isCurrentDialogueEnded = true;


        // tutorial positions
        tutorialPosIndex = 0;

        panelText.transform.position = tutorialPositions[tutorialPosIndex].position;

        // button indexes highlighted, start from -1 so the first next is set to start of array
        elementHighlightedIndex = -1;

        // set the player to be visible
        player.sortingLayerName = "UI";
        player.sortingOrder = 150;

        NextDialogue();
    }

    public void NextDialogue()
    {
        // barrier on the click to skip if dialogue not ended
        if (!isCurrentDialogueEnded) return;

        AudioManager.Instance.PlayClickUI();

        if(currentDialogueIndex >= currentDialogues.Count)
        {
            // ended dialogues

            // dehighlight last ui element
            ResetLastUIElement();

            // active remaining ui elements

            buttonSetting.SetActive(true);
            panelStage.SetActive(true);
            panelAutoBattle.SetActive(true);
            panelShrink.SetActive(true);

            // hide panel
            content.SetActive(false);

            SettingsManager.Instance.SetSeenTutorial(lastShownTutorial, true);

            // trigger last tutorial has been finished
            OnTutorialEnded?.Invoke(lastShownTutorial);
        }
        else
        {
            // next dialogue
            isCurrentDialogueEnded = false;

            // get dialogue and if need a new position
            UtilsGeneral.TutorialDialogueNeedPos dialogueNeedPos = currentDialogues[currentDialogueIndex++];

            // if need new pos change panel text position
            if (dialogueNeedPos.Need)
            {
                if (!hasChangedPlayer)
                {
                    hasChangedPlayer = true;

                    // hide back the player
                    player.sortingLayerName = "Player";
                    player.sortingOrder = 10;
                }

                // set new position of text
                if (tutorialPosIndex < tutorialPositions.Count - 1)
                {
                    panelText.transform.position = tutorialPositions[++tutorialPosIndex].position;
                }

                ResetLastUIElement();

                // if there are still ui element to highlight, according to tutorial array, add canvas component to do so
                if (elementHighlightedIndex < uiElementsToHighlight.Count - 1)
                {
                    lastUIElementHighlighted = uiElementsToHighlight[++elementHighlightedIndex];

                    // active every button in order
                    lastUIElementHighlighted.SetActive(true);

                    lastCanvasComponentAdded = lastUIElementHighlighted.AddComponent<Canvas>();
                    lastCanvasComponentAdded.overrideSorting = true;
                    lastCanvasComponentAdded.sortingLayerName = "UI";
                    lastCanvasComponentAdded.sortingOrder = 100;
                }
            }

            // start showing the text
            StartCoroutine(CoShowDialogue(dialogueNeedPos.Dialogue));
        }
            
    }

    // dehighlight last ui element
    private void ResetLastUIElement()
    {
        if (lastCanvasComponentAdded != null && lastUIElementHighlighted != null)
        {
            Destroy(lastCanvasComponentAdded);
        }
    }

    private IEnumerator CoShowDialogue(string dialogue)
    {
        string showingDialogue = "";

        // one char at a time
        for (int i = 0; i < dialogue.Length; i++)
        {
            char nextChar = dialogue[i];
            showingDialogue += nextChar;

            textTutorial.text = showingDialogue;

            if(nextChar != ' ')
                yield return new WaitForSeconds(timeBtwChars);
        }

        // set current dialogue ended so you can move on
        isCurrentDialogueEnded = true;
    }


    private void DisableAllUI()
    {
        buttonLevel.SetActive(false);
        buttonJob.SetActive(false);
        buttonInventory.SetActive(false);

        buttonShop.SetActive(false);
        buttonQuests.SetActive(false);

        buttonSetting.SetActive(false);
        panelStage.SetActive(false);
        panelAutoBattle.SetActive(false);

        panelShrink.SetActive(false);
    }


    public void OnClickSkipTutorial()
    {
        // dehighlight ui elements
        foreach (var uiElement in uiElementsToHighlight)
        {
            if(uiElement.TryGetComponent(out Canvas canvasComp))
            {
                Destroy(canvasComp);
            }
        }

        // active all ui elements
        buttonLevel.SetActive(true);
        buttonJob.SetActive(true);
        buttonInventory.SetActive(true);

        buttonShop.SetActive(true);
        buttonQuests.SetActive(true);

        buttonSetting.SetActive(true);
        panelStage.SetActive(true);
        panelAutoBattle.SetActive(true);
        panelShrink.SetActive(true);

        // hide panel
        content.SetActive(false);

        SettingsManager.Instance.SetSeenTutorial(lastShownTutorial, true);

        // trigger last tutorial has been finished
        OnTutorialEnded?.Invoke(lastShownTutorial);
    }
}
