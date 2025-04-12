using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopProp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PlayableProp PropData;

    private Image propImage;
    private TooltipTrigger tooltipTrigger;
    [SerializeField] private float scaleOnHover = 1.15f;
    [SerializeField] private float scaleOnSelect = 1.25f;
    [SerializeField] private float scaleTransition = 0.15f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    void Start()
    {
        propImage = GetComponent<Image>();
        tooltipTrigger = GetComponent<TooltipTrigger>();
    }

    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipTrigger.setTooltip(PropData.PropName, PropData.Description);
        transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, scaleTransition).SetEase(scaleEase);
    }

    public void UpdateVisual()
    {
        if (propImage == null)
        {
            propImage = GetComponent<Image>();
        }
        propImage.sprite = PropData.Image;
    }
}
