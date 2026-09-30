using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UICardReveal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image imageRarity;

    [SerializeField] GameObject panelBack;

    [Space(10)]
    [SerializeField] GameObject panelFront;
    [SerializeField] Image imageBackground;
    [SerializeField] Image imageCard;

    [Space(10)]
    [SerializeField] float flip90Time = 0.5f;

    [Header("Vibration")]
    [SerializeField] float _vibrationRot = 8.5f;
    [SerializeField] float _vibrationTime = 0.15f;

    [Header("VFX")]
    [SerializeField] GameObject _importantCardVFX;

    public bool IsFlipped { get; private set; }



    private CardSO _cardSO;

    private Vector3 _startRotation;


    private Tween _tweenRot1;
    private Tween _tweenRot2;

    private Tween _tweenVibration;


    private void OnDestroy()
    {
        _tweenRot1?.Kill();
        _tweenRot2?.Kill();
    }

    private void Awake()
    {
        _startRotation = transform.localEulerAngles;
    }

    public void Setup(CardSO cardSO)
    {
        _cardSO = cardSO;

        imageRarity.gameObject.SetActive(false);
        imageRarity.color = UtilsColor.GetColorByRarity(cardSO.CardRarity);

        panelFront.SetActive(false);

        panelBack.SetActive(true);

        imageBackground.sprite = cardSO.BackgoundSprite;
        imageCard.sprite = cardSO.Sprite;
    }

    public void Flip()
    {
        if (IsFlipped) return;

        _tweenVibration?.Kill();
        transform.localEulerAngles = _startRotation;

        IsFlipped = true;

        _tweenRot1 = transform.DORotate(new Vector3(0, 90f, 0), flip90Time).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() =>
        {
            panelBack.SetActive(false);
            panelFront.SetActive(true);

            imageRarity.gameObject.SetActive(true);

            if (IsImportantCard()) StartVFX();

            _tweenRot2 = transform.DORotate(new Vector3(0, 0, 0), flip90Time).SetEase(Ease.InOutSine).SetUpdate(true);
        });
    }

    private bool IsImportantCard()
    {
        if (_cardSO == null) return false;

        switch(_cardSO.Id)
        {
            default: return false;
            case 80:
            case 83:
            case 84:
            case 85:
            case 86:
            case 87: return true;
        }
    }


    private void StartVFX()
    {
        GameObject prefab = Instantiate(_importantCardVFX, transform.position, _importantCardVFX.transform.rotation);
        prefab.SetActive(true);
        prefab.transform.parent = transform;

        prefab.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsFlipped) return;

        imageRarity.gameObject.SetActive(true);

        transform.localEulerAngles = new Vector3(0f, 0f, -_vibrationRot);

        _tweenVibration = transform.DORotate(new Vector3(0f, 0f, _vibrationRot), _vibrationTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsFlipped) return;

        imageRarity.gameObject.SetActive(false);

        _tweenVibration.Kill();
        transform.localEulerAngles = _startRotation;
    }
}
