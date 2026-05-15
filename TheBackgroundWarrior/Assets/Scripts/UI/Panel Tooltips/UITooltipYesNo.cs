using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITooltipYesNo : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [Space(10)]
    [SerializeField] float timeToFade = 1f;

    [SerializeField] TMP_Text textQuestion;

    [Space(10)]
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;


    public bool IsShowing { get; private set; }


    private bool isFade;
    private Tween tweenFade;

    private TaskCompletionSource<bool> tcs;

    private void Awake()
    {
        yesButton.onClick.AddListener(() => Close(true));
        noButton.onClick.AddListener(() => Close(false));
    }

    private void OnDestroy()
    {
        tweenFade?.Kill();
    }
    
    public Task<bool> Show(string question, Vector2 position, bool fade = false)
    {
        IsShowing = true;

        // set pos
        transform.position = position;

        // set text
        textQuestion.text = question;
        
        isFade = fade;

        gameObject.SetActive(true);

        if (!fade)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            // handles fade
            if (tweenFade == null)
            {
                canvasGroup.alpha = 0f;
            }
            else
            {
                tweenFade.Kill();
            }

            // scale with unscaled delta time
            tweenFade = canvasGroup.DOFade(1f, timeToFade).SetEase(Ease.InOutSine).SetUpdate(true);
        }

        tcs = new TaskCompletionSource<bool>();
        return tcs.Task;
    }

    public void Hide(bool fade = false)
    {
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
            tweenFade = canvasGroup.DOFade(0f, timeToFade).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }

    private void Close(bool result)
    {
        IsShowing = false;

        tcs?.TrySetResult(result);

        Hide(isFade);
    }
}
