using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonLongPress : Button
{
    private float cooldownLongPress = 0.4f;

    private float timerLongPress;

    public override void OnPointerDown(PointerEventData eventData)
    {
        // instantly do as a click
        base.OnPointerClick(eventData);

        StartCoroutine(CoLongPress(eventData));
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        // stop long press
        StopAllCoroutines();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        // click only does nothing since it's in on pointer down
    }

    private IEnumerator CoLongPress(PointerEventData eventData)
    {
        // starting timer is by default 0.5 seconds
        timerLongPress = cooldownLongPress;

        while (true)
        {
            // after timer call on click again
            yield return new WaitForSecondsRealtime(timerLongPress);

            // reduce timer every tick of long press, until 0.1f which is minimum
            timerLongPress = Mathf.Max(0.1f, timerLongPress - 0.1f);

            base.OnPointerClick(eventData);
        }
    }
}
