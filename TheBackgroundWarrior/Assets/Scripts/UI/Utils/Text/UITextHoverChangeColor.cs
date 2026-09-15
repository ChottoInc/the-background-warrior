using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class UITextHoverChangeColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] float _timerColor = 0.5f;
    [SerializeField] Color _hoverColor;

    private TMP_Text _text;
    private Color _startingColor;

    private Tween _tweenColor;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _startingColor = _text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Paint();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Unpaint();
    }

    private void Paint()
    {
        _tweenColor?.Kill();

        _tweenColor = _text.DOColor(_hoverColor, _timerColor).SetEase(Ease.InOutSine).SetLink(gameObject, LinkBehaviour.KillOnDestroy).SetUpdate(true);
    }

    private void Unpaint()
    {
        _tweenColor?.Kill();

        _tweenColor = _text.DOColor(_startingColor, _timerColor).SetEase(Ease.InOutSine).SetLink(gameObject, LinkBehaviour.KillOnDestroy).SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Unpaint();
    }
}
