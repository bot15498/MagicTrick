using UnityEngine;

public class WorldToUIScreenFollow : MonoBehaviour
{
    [Tooltip("The world object to follow (e.g., player or NPC).")]
    public Transform targetWorldObject;

    [Tooltip("The UI element (e.g., Image, Text, etc.)")]
    public RectTransform uiElement;

    [Tooltip("Optional offset from the target world object's position.")]
    public Vector3 worldOffset = Vector3.up;

    [Tooltip("Canvas with Screen Space - Camera or Overlay mode.")]
    public Canvas canvas;

    private Camera mainCamera;

    void Start()
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            mainCamera = canvas.worldCamera;
        }
        else
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (targetWorldObject == null || uiElement == null || mainCamera == null)
            return;

        // World position with offset
        Vector3 worldPosition = targetWorldObject.position + worldOffset;

        // Convert world position to screen position
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // If the UI canvas is Screen Space - Overlay, we can set it directly
        uiElement.position = screenPosition;
    }
}
