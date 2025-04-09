
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class ExplanationToolTip : MonoBehaviour, IPointerEnterHandler
{
    public string titleString;
    public string descriptionString;
    TooltipTrigger tprigger;
    // Start is called before the first frame update
    [HideInInspector] public UnityEvent<ExplanationToolTip> PointerEnterEvent;
    void Start()
    {
        tprigger = GetComponent<TooltipTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent?.Invoke(this);

        if (tprigger != null)
            tprigger.setTooltip(titleString, descriptionString);
        else
            Debug.LogWarning("TooltipTrigger not found on " + gameObject.name);

    }
}
