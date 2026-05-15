using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICardConverted : MonoBehaviour
{
    private int revealAmount = Shader.PropertyToID("_Reveal");


    [SerializeField] RectTransform cardRect;
    [SerializeField] RectTransform particleRect;

    private float height;
    private float bottom;

    [Space(10)]
    [SerializeField] Image imageBackground;
    [SerializeField] Image imageCard;

    [Header("Animations")]
    [SerializeField] float revealTime = 1f;
    [SerializeField] float revealDelay = 1f;
    [SerializeField] ParticleSystem revealVFX;


    private Material matImageBackground;
    private Material matImageCard;

    private float lerpedReveal;

    private float edgeY;

    private bool isRevealed;



    private CardSO cardSO;


    private bool isInitialized;



    private Tween tweenRot1;
    private Tween tweenRot2;


    public event Action OnRevealed;



    public bool IsRevealed => isRevealed;




    private void OnDestroy()
    {
        tweenRot1?.Kill();
        tweenRot2?.Kill();
    }

    public void Setup(CardSO cardSO)
    {
        InitializedIfNeeded();

        ResetUI();

        this.cardSO = cardSO;

        imageBackground.sprite = cardSO.BackgoundSprite;
        imageCard.sprite = cardSO.Sprite;

        StartCoroutine(CoReveal());
    }

    private void InitializedIfNeeded()
    {
        if (isInitialized) return;

        matImageBackground = new Material(imageBackground.material);
        imageBackground.material = matImageBackground;

        matImageCard = new Material(imageCard.material);
        imageCard.material = matImageCard;

        height = cardRect.rect.height;

        // UI pivot is usually center (0.5)
        // So bottom is -height/2
        bottom = -height * 0.5f;

        isInitialized = true;
    }

    public void ResetUI()
    {
        isRevealed = false;

        lerpedReveal = 0;

        revealVFX.Stop();

        edgeY = bottom + (height * lerpedReveal);
        particleRect.anchoredPosition = new Vector2(0, edgeY);
    }


    private IEnumerator CoReveal()
    {
        revealVFX.Play();

        float elapsedTime = 0;
        while (elapsedTime < revealTime)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // lerp amount
            lerpedReveal = Mathf.Clamp01(elapsedTime / revealTime);

            // set images reveal amount
            matImageBackground.SetFloat(revealAmount, lerpedReveal);
            matImageCard.SetFloat(revealAmount, lerpedReveal);

            // handle particle
            edgeY = bottom + (height * lerpedReveal);
            particleRect.anchoredPosition = new Vector2(0, edgeY);

            yield return null;
        }

        yield return new WaitForSecondsRealtime(revealDelay);

        revealVFX.Stop();

        isRevealed = true;
        OnRevealed?.Invoke();
    }
}
