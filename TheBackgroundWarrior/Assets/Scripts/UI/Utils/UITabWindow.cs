using System;
using UnityEngine;

public class UITabWindow : MonoBehaviour
{
    protected bool isOpen;

    public event Action OnTabClose;

    public virtual void Open()
    {
        isOpen = true;

        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        isOpen = false;

        gameObject.SetActive(false);

        OnTabClose?.Invoke();
    }

    public virtual bool CanClose()
    {
        return true;
    }
}
