using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIAnimatedPanelDamage : MonoBehaviour
{
    [SerializeField] UIBasePanelDamage panelDamage;
    [SerializeField] TMP_Text textDamage;

    private Tween tweenMovement;
    private Tween tweenScale;

    private Vector3 startScaleObject;

    public bool IsAnimating { get; set; }

    private void OnDestroy()
    {
        tweenMovement?.Kill();

        tweenScale?.Kill();
    }

    private void Awake()
    {
        startScaleObject = transform.localScale;
    }

    public void Animate(int damage)
    {
        IsAnimating = true;

        textDamage.text = damage.ToString();

        gameObject.SetActive(true);

        transform.position = panelDamage.StartContentPos.position;
        transform.localScale = startScaleObject;

        // move and hide at the end
        tweenMovement = transform.DOMove(panelDamage.EndContentPos.position, panelDamage.MoveTime).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            gameObject.SetActive(false);
            
            // reset starting scale
            transform.localScale = startScaleObject;
            //Debug.Log("end ani");

            IsAnimating = false;
        });

        tweenScale = transform.DOScale(0, panelDamage.MoveTime).SetEase(Ease.InOutSine);
    }
}
