using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltipText = "Tooltip here!";
    public Vector3 tooltipOffset = new Vector3(0, 50, 0);
    public Vector3 RighttooltipOffset;
    public bool rightTooltip = false; // This will be set by the Card script

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 offset = rightTooltip ? RighttooltipOffset : tooltipOffset;
        TooltipSystem.Instance.ShowTooltip(tooltipText, transform.position, offset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Instance.HideTooltip();
    }

    public void setTooltip(string tooltiptoadd)
    {
        TooltipSystem.Instance.setdescriptionText(tooltiptoadd);
        Debug.Log(tooltiptoadd);
    }
}
