using System;
using UnityEngine;
using UnityEngine.UI;

public class UITab : MonoBehaviour
{
    [SerializeField] TabManager tabManager;
    [SerializeField] UITabWindow tabWindow;

    [Space(10)]
    [SerializeField] bool reclickToClose;

    private bool isSelected;

    [Space(10)]
    [SerializeField] bool stopTime;

    [Header("Highlight")]
    [SerializeField] bool canHighlight;
    [SerializeField] Image imageSelected;
    [SerializeField] Color selectedColor;


    public event Action OnSelected;
    public event Action OnDeselected;


    private void Awake()
    {
        tabWindow.OnTabClose += ManualClose;
    }

    private void OnDestroy()
    {
        tabWindow.OnTabClose -= ManualClose;
    }

    public void Select()
    {
        if(!reclickToClose)
        {
            tabManager.ChangeCurrentTab(this);
        }
        else
        {
            if (!isSelected)
            {
                tabManager.ChangeCurrentTab(this);
            }
            else
            {
                tabManager.CloseCurrentTab();
            }
        }
    }

    public void OnSelect()
    {
        if (stopTime) UtilsTime.Pause();

        isSelected = true;

        if (canHighlight)
        {
            imageSelected.color = selectedColor;
        }

        tabWindow.Open();

        OnSelected?.Invoke();
    }

    public void OnDeselect()
    {
        if (stopTime) UtilsTime.Resume();

        isSelected = false;

        if (canHighlight)
        {
            imageSelected.color = Color.white;
        }

        tabWindow.Close();

        OnDeselected?.Invoke();
    }

    private void ManualClose()
    {
        if (stopTime) UtilsTime.Resume();

        if (canHighlight)
        {
            imageSelected.color = Color.white;
        }

        isSelected = false;

        OnDeselected?.Invoke();
    }
}
