using TMPro;
using UnityEngine;

public class UITimerBait : MonoBehaviour
{
    [SerializeField] TMP_Text _textTimer;

    public void SetTimer(string time)
    {
        _textTimer.text = time;
    }
}
