using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAnimatedButton : Button
{
    [SerializeField] Image imageToAnimate;
    [SerializeField] float timeSingleFrame = 0.2f;
    [SerializeField] Sprite[] spriteList;


    private int animationDirection = 1;
    private int spriteIndex = 0;



    protected override void Awake()
    {
        base.Awake();

        if (imageToAnimate == null)
            imageToAnimate = image;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactable) return;

        StartCoroutine(CoAnimation(timeSingleFrame));
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!interactable) return;

        ResetButton();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        //TODO: check if this breaks something
        if (!interactable) return;

        ResetButton();

        AudioManager.Instance.PlayClickUI();

        base.OnPointerClick(eventData);
    }

    private void ResetButton()
    {
        StopAllCoroutines();
        animationDirection = 1;
        spriteIndex = 0;

        imageToAnimate.sprite = spriteList[0];
    }

    private IEnumerator CoAnimation(float timeBtwFrames)
    {
        while (true)
        {
            imageToAnimate.sprite = GetNextSprite();
            yield return new WaitForSecondsRealtime(timeBtwFrames);
        }
    }

    private Sprite GetNextSprite()
    {
        if (spriteList == null || spriteList.Length == 0)
            return null;

        // return current frame
        Sprite result = spriteList[spriteIndex];

        // move index
        spriteIndex += animationDirection;

        // handle bouncing
        if (spriteIndex == spriteList.Length - 1)
            animationDirection = -1;
        else if (spriteIndex == 0)
            animationDirection = 1;

        return result;
    }
}
