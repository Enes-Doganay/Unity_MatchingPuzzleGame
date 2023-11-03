using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    [SerializeField] private Sprite cardBack;
    public Sprite cardFront;
    EventTrigger myEventTrigger;
    private bool isFlipped = false;
    private bool isMatched = false;
    private void Awake()
    {
        GetComponent<Image>().sprite = cardBack;
    }
    void Start()
    {
        myEventTrigger = this.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
        myEventTrigger.triggers.Add(entry);
    }

    private void OnPointerClick(PointerEventData data)
    {
        CardManager.Instance.OnCardClicked(this);
    }

    public void FlipCard()
    {
        if (isFlipped)
        {
            CloseCard();
        }
        else
        {
            OpenCard();
        }
    }

    private void OpenCard()
    {
        RectTransform cardTransform = GetComponent<RectTransform>();
        cardTransform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            GetComponent<Image>().sprite = cardFront;
            cardTransform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });

        isFlipped = true;
    }

    private void CloseCard()
    {
        RectTransform cardTransform = GetComponent<RectTransform>();
        cardTransform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            GetComponent<Image>().sprite = cardBack;
            cardTransform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });

        isFlipped = false;
    }

    public bool IsMatched
    {
        get { return isMatched; }
        set { isMatched = value; }
    }

    public void SetCardFront(Sprite frontSprite, float flipDuration)
    {
        cardFront = frontSprite;
        GetComponent<Image>().sprite = cardFront;
        Invoke("CloseCard", flipDuration *2);
    }
}
