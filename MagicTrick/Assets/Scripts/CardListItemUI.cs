using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardListItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string descriptionString;
    public Image cardIconImage;
    private TooltipTrigger tprigger;

    [HideInInspector] public UnityEvent<CardListItemUI> PointerEnterEvent;

    [Header("Scale Parameters")]
    [SerializeField] private bool scaleAnimations = true;
    [SerializeField] private float scaleOnHover = 1.15f;
    [SerializeField] private float scaleOnSelect = 1.25f;
    [SerializeField] private float scaleTransition = 0.15f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    private void Start()
    {
        tprigger = GetComponent<TooltipTrigger>();
    }

    public void Setup(PlayableCard card)
    {
        descriptionString = card.Description;
        cardIconImage.sprite = card.Image;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        

        PointerEnterEvent?.Invoke(this);

        if (tprigger != null)
            tprigger.setTooltip(descriptionString);
        else
            Debug.LogWarning("TooltipTrigger not found on " + gameObject.name);

        if (scaleAnimations)
            transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited: " + gameObject.name);

        if (scaleAnimations)
            transform.DOScale(1f, scaleTransition).SetEase(scaleEase);
    }
}
