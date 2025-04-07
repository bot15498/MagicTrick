using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem Instance;

    public RectTransform tooltipPanel;
    public TextMeshProUGUI tooltipText;
    public Canvas canvas;

    private bool isShowing = false;

    void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    public void setdescriptionText(string descriptiontext)
    {
        tooltipText.text = descriptiontext;
  
    }

    public void ShowTooltip(string text, Vector3 worldPosition, Vector3 offset)
    {
        isShowing = true;
        
        UpdateTooltipPosition(worldPosition, offset);
        tooltipPanel.gameObject.SetActive(true);
    }


    public void HideTooltip()
    {
        isShowing = false;
        tooltipPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isShowing)
        {
            // Optional: make it follow the object if needed
            // Tooltip can continuously update its position if the target is moving
        }
    }

    public void UpdateTooltipPosition(Vector3 worldPosition, Vector3 offset)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
        tooltipPanel.position = screenPoint + (Vector2)offset;
    }
}
