using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardListItemRemoveUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string descriptionString;
    public string titleString;
    public Image cardIconImage;
    private TooltipTrigger tprigger;

    private DeckManager deckManager;
    private ScoreManager scoreManager;
    private ShopManager shopManager;

    [HideInInspector] public UnityEvent<CardListItemRemoveUI> PointerEnterEvent;
    public UnityEvent OnDeckChanged; // <- Add this event for click actions

    [Header("Scale Parameters")]
    [SerializeField] private bool scaleAnimations = true;
    [SerializeField] private float scaleOnHover = 1.15f;
    [SerializeField] private float scaleOnSelect = 1.25f;
    [SerializeField] private float scaleTransition = 0.15f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;


    private PlayableCard savedCard;

    private void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        scoreManager = deckManager.GetComponent<ScoreManager>();
        shopManager = deckManager.GetComponent<ShopManager>();
        tprigger = GetComponent<TooltipTrigger>();
    }

    public void Setup(PlayableCard card)
    {
        descriptionString = card.Description;
        cardIconImage.sprite = card.Image;
        titleString = card.CardName;
        savedCard = card;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent?.Invoke(this);

        if (tprigger != null)
            tprigger.setTooltip(titleString, descriptionString);
        else
            Debug.LogWarning("TooltipTrigger not found on " + gameObject.name);

        if (scaleAnimations)
            transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleAnimations)
            transform.DOScale(1f, scaleTransition).SetEase(scaleEase);

    }
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //MINUS MONEY DISABLE CARD REMOVAL 
        //ADD REMOVAL EFFECT HERE
        
        deckManager.RemoveCard(savedCard);
        TooltipSystem.Instance.HideTooltip();
        scoreManager.money -= shopManager.deleteCardCost;
    }
}
