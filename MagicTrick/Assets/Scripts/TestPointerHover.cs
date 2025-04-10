using UnityEngine;
using UnityEngine.EventSystems;

public class TestPointerHover : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered: " + gameObject.name);
    }
}
