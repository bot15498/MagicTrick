using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public PlayableCard CardData;
    public bool selected = false;
    [SerializeField]
    private Image cardImage;
    private TooltipTrigger tooltipTrigger;
    [SerializeField] private float scaleOnHover = 1.15f;
    [SerializeField] private float scaleOnSelect = 1.25f;
    [SerializeField] private float scaleTransition = 0.15f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;
    [Header("Select Parameters")]
    [SerializeField] private float selectPunchAmount = 20;
    public float selectionOffset = 50;
    [Header("Hober Parameters")]
    [SerializeField] private float hoverPunchAngle = 5;
    [SerializeField] private float hoverTransition = .15f;

    void Start()
    {
        cardImage = GetComponent<Image>();
        tooltipTrigger = GetComponent<TooltipTrigger>();
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selected = !selected;
        DOTween.Kill(2, true);
        float dir = selected ? 1 : 0;
        transform.DOPunchPosition(transform.up * selectPunchAmount * dir, scaleTransition, 10, 1);
        transform.DOPunchRotation(Vector3.forward * (hoverPunchAngle / 2), hoverTransition, 20, 1).SetId(2);
        if (selected)
            transform.localPosition += (transform.up * selectionOffset);
        else
            transform.localPosition = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipTrigger.setTooltip(CardData.CardName, CardData.Description);
        transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, scaleTransition).SetEase(scaleEase);
    }

    public void UpdateVisual()
    {
        if(cardImage == null)
        {
            cardImage = GetComponent<Image>();
        }
        cardImage.sprite = CardData.Image;
    }
}
