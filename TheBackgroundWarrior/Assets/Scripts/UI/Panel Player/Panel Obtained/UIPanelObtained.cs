using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIPanelObtained : MonoBehaviour
{
    private const int MAX_QUEUE = 5;

    [SerializeField] Sprite spriteOre;
    [SerializeField] Sprite spriteCard;
    [SerializeField] Sprite spriteFish;

    [Space(10)]
    [SerializeField] Player player;

    [Space(10)]
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] Transform startContentPos;
    [SerializeField] Transform endContentPos;

    [Space(10)]
    [SerializeField] GameObject objectToMove;
    [SerializeField] Image imageObtained;




    //private Vector2 startPos;
    //private Vector2 endPos;

    private Queue<ItemSO> queueItems;

    private Tween tweenMovement;
    private bool isAnimating;

    private void Awake()
    {
        player.OnItemAdd += AddItemToQueue;
    }

    private void OnDestroy()
    {
        player.OnItemAdd -= AddItemToQueue;

        tweenMovement?.Kill();
    }

    private void Start()
    {
        queueItems = new Queue<ItemSO>();
    }


    private void Update()
    {
        if(queueItems.Count > 0 && !isAnimating)
        {
            // if queue not empty and no animation is playing, play new animation
            isAnimating = true;
            //Debug.Log("start ani");
            // dequeue item and set sprite
            ItemSO itemSO = queueItems.Dequeue();
            imageObtained.sprite = GetSpriteByType(itemSO);

            // reset obejct pos and show
            objectToMove.transform.position = startContentPos.position;
            objectToMove.SetActive(true);

            // move and hide at the end
            tweenMovement = objectToMove.transform.DOMove(endContentPos.position, moveTime).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                objectToMove.SetActive(false);
                isAnimating = false;
                //Debug.Log("end ani");
            });
        }
    }




    private void AddItemToQueue(ItemSO itemSO)
    {
        if (queueItems.Count >= MAX_QUEUE) return;

        // check if Floating is enabled
        if (SettingsManager.Instance.IsItemCollectionOn)
        {
            // enqueue new item animation to do
            queueItems.Enqueue(itemSO);
        }
    }

    private Sprite GetSpriteByType(ItemSO itemSO)
    {
        switch(itemSO.ItemType)
        {
            default:
            case UtilsItem.ItemType.Card: return spriteCard;

            case UtilsItem.ItemType.Ore:
            case UtilsItem.ItemType.Fish:
            case UtilsItem.ItemType.Metal:
                return itemSO.Sprite;
        }
    }
}
