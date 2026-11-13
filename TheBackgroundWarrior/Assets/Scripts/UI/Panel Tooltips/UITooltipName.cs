using DG.Tweening;
using TMPro;
using UnityEngine;


public class UITooltipName : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [Space(10)]
    [SerializeField] float timeToFade = 1f;

    [Space(10)]
    [SerializeField] RectTransform root;
    [SerializeField] TMP_Text textName;

    [Header("Width")]
    [SerializeField] float minWidth = 80f;
    [SerializeField] float maxWidth = 2000f;

    [Header("Height")]
    [SerializeField] float minHeight = 80f;
    [SerializeField] float maxHeight = 2000f;

    private Tween tweenFade;

    private void OnDestroy()
    {
        tweenFade?.Kill();
    }

    private void Resize()
    {
        // Force TMP to update its geometry
        textName.ForceMeshUpdate();

        float textWidth = textName.preferredWidth;

        textWidth = Mathf.Clamp(textWidth, minWidth, maxWidth);

        root.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            textWidth
        );

        float textHeight = textName.preferredHeight;

        textHeight = Mathf.Clamp(textHeight, minHeight, maxHeight);

        root.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            textHeight
        );
    }

    public void Show(string text, Vector2 position, bool fade = false, float fontMaxSize = 50f)
    {
        if (!SettingsManager.Instance.AreTooltipsOn) return;

        // set pos
        transform.position = position;

        // set text
        textName.text = text;

        // set max font size
        textName.fontSizeMax = fontMaxSize;

        gameObject.SetActive(true);

        if (!fade)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            // handles fade
            if(tweenFade == null)
            {
                canvasGroup.alpha = 0f;
            }
            else
            {
                tweenFade.Kill();
            }

            // scale with unscaled delta time
            tweenFade = canvasGroup.DOFade(1f, timeToFade).SetEase(Ease.InOutSine).SetUpdate(true).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        Resize();
        FixTMPAnchors();
    }

    public void ResetAlignment()
    {
        textName.alignment = TextAlignmentOptions.Center;
    }

    public void SetAlignment(TextAlignmentOptions option)
    {
        textName.alignment = option;
    }

    private void FixTMPAnchors()
    {
        RectTransform rt = textName.rectTransform;

        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);

        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    public void Hide(bool fade = false)
    {
        // do not know why, but sometimes canvas group component is destroyed before that script
        // so set gameobject to false and then return
        if (canvasGroup == null)
        {
            //Debug.Log("canvas null");
            //
            //if (gameObject == null)
            //{
            //    Debug.Log("go null");
            //    return;
            //}
            //
            //Debug.Log("go not null");
            gameObject.SetActive(false);
            return;
        }

        if (!fade)
        {
            canvasGroup.alpha = 0f;

            gameObject.SetActive(false);
        }
        else
        {
            // handles fade
            if (tweenFade == null)
            {
                canvasGroup.alpha = 1f;
            }
            else
            {
                tweenFade.Kill();
            }

            // scale with unscaled delta time
            tweenFade = canvasGroup.DOFade(0f, timeToFade).SetEase(Ease.InOutSine).SetUpdate(true).SetLink(gameObject, LinkBehaviour.KillOnDestroy).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}
