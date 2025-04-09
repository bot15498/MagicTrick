using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltipText = "IF YOU SEE THIS YOU MESSED UP";
    public Vector3 tooltipOffset = new Vector3(0, 50, 0);
    public Vector3 RighttooltipOffset;
    public bool rightTooltip = false; 

    public void OnPointerEnter(PointerEventData eventData)
    {
       
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Instance.HideTooltip();
    }

    public void setTooltip(string Title,string tooltiptoadd)
    {
        Vector3 offset = rightTooltip ? RighttooltipOffset : tooltipOffset;
        TooltipSystem.Instance.ShowTooltip(Title,tooltiptoadd,transform.position, offset);

    }
}
