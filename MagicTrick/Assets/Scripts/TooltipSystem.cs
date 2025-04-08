using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem Instance;

    public RectTransform tooltipPanel;
    public TextMeshProUGUI tooltipText;
    public Canvas canvas;
    public Vector2 screenPadding = new Vector2(10f, 10f);

    [SerializeField] private Camera targetCamera;

    private bool isShowing = false;

    void Awake()
    {
        Instance = this;
        HideTooltip();

        if (targetCamera == null)
        {
            GameObject camObj = GameObject.FindGameObjectWithTag("GameCamera");
            if (camObj != null)
                targetCamera = camObj.GetComponent<Camera>();

            if (targetCamera == null)
                Debug.LogWarning("TooltipSystem: No camera with tag 'GameCamera' found.");
        }
    }

    public void setdescriptionText(string descriptiontext)
    {
        tooltipText.text = descriptiontext;
    }

    public void ShowTooltip(string text, Vector3 worldPosition, Vector3 offset)
    {
        isShowing = true;
        setdescriptionText(text);
        tooltipPanel.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipPanel);
        UpdateTooltipPosition(worldPosition, offset);
    }

    public void HideTooltip()
    {
        isShowing = false;
        tooltipPanel.gameObject.SetActive(false);
        tooltipPanel.position = Vector3.zero;
    }

    void Update()
    {
        if (isShowing)
        {
            // Optional: dynamically update if tooltip follows something
        }
    }

    public void UpdateTooltipPosition(Vector3 worldPosition, Vector3 offset)
    {
        if (targetCamera == null)
        {
            GameObject camObj = GameObject.FindGameObjectWithTag("GameCamera");
            if (camObj != null)
                targetCamera = camObj.GetComponent<Camera>();

            if (targetCamera == null)
                Debug.LogWarning("TooltipSystem: No camera with tag 'GameCamera' found.");
        }

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(targetCamera, worldPosition);
        Vector2 targetPosition = screenPoint + (Vector2)offset;

        Vector2 tooltipSize = tooltipPanel.sizeDelta;
        float pivotX = tooltipPanel.pivot.x;
        float pivotY = tooltipPanel.pivot.y;

        float left = targetPosition.x - tooltipSize.x * pivotX;
        float right = targetPosition.x + tooltipSize.x * (1 - pivotX);
        float bottom = targetPosition.y - tooltipSize.y * pivotY;
        float top = targetPosition.y + tooltipSize.y * (1 - pivotY);

        if (left < screenPadding.x)
            targetPosition.x += screenPadding.x - left;
        else if (right > Screen.width - screenPadding.x)
            targetPosition.x -= right - (Screen.width - screenPadding.x);

        if (bottom < screenPadding.y)
            targetPosition.y += screenPadding.y - bottom;
        else if (top > Screen.height - screenPadding.y)
            targetPosition.y -= top - (Screen.height - screenPadding.y);

        tooltipPanel.position = targetPosition;
    }
}
