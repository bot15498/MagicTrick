using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Prop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Canvas canvas;
    private Image imageComponent;
    [SerializeField] private bool instantiateVisual = true;
    private VisualCardsHandler visualHandler;
    private Vector3 offset;

    [Header("Movement")]
    [SerializeField] private float moveSpeedLimit = 50;
    [SerializeField] private Camera targetCamera;

    [Header("Visual")]
    [SerializeField] private GameObject propVisualPrefab;
    [HideInInspector] public PropVisual propVisual;

    [Header("States")]
    public bool isHovering;
    public bool isDragging;
    public bool wasDragged;

    [Header("Prop Data")]
    public PlayableProp PropData;

    [Header("Events")]
    public UnityEvent<Prop> PointerEnterEvent = new UnityEvent<Prop>();
    public UnityEvent<Prop> PointerExitEvent = new UnityEvent<Prop>();
    public UnityEvent<Prop, bool> PointerUpEvent = new UnityEvent<Prop, bool>();
    public UnityEvent<Prop> PointerDownEvent = new UnityEvent<Prop>();
    public UnityEvent<Prop> BeginDragEvent = new UnityEvent<Prop>();
    public UnityEvent<Prop> EndDragEvent = new UnityEvent<Prop>();
    public UnityEvent<Prop, bool> SelectEvent = new UnityEvent<Prop, bool>();

    public IPropZone previousSlotGroup;
    public IPropZone currentDropZone;

    private float pointerDownTime;
    private float pointerUpTime;
    private TooltipTrigger tooltipTrigger;

    void Start()
    {
        if (targetCamera == null)
        {
            GameObject camObj = GameObject.FindGameObjectWithTag("GameCamera");
            if (camObj != null)
                targetCamera = camObj.GetComponent<Camera>();

            if (targetCamera == null)
                Debug.LogWarning("Prop: No camera with tag 'GameCamera' found.");
        }

        canvas = GetComponentInParent<Canvas>();
        imageComponent = GetComponent<Image>();

        if (instantiateVisual)
        {
            visualHandler = FindObjectOfType<VisualCardsHandler>();
            propVisual = Instantiate(propVisualPrefab, visualHandler ? visualHandler.transform : canvas.transform).GetComponent<PropVisual>();
            propVisual.Initialize(this);
        }

        previousSlotGroup = GetComponentInParent<IPropZone>();
        currentDropZone = previousSlotGroup;
        tooltipTrigger = GetComponent<TooltipTrigger>();
    }

    void Update()
    {
        ClampPosition();

        if (isDragging)
        {
            Vector2 targetPosition = targetCamera.ScreenToWorldPoint(Input.mousePosition) - offset;
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
            transform.Translate(velocity * Time.deltaTime);
        }
    }

    void ClampPosition()
    {
        Vector2 screenBounds = targetCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, targetCamera.transform.position.z));
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -screenBounds.x, screenBounds.x),
            Mathf.Clamp(transform.position.y, -screenBounds.y, screenBounds.y),
            0
        );
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent.Invoke(this);
        isDragging = true;
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        imageComponent.raycastTarget = false;
        wasDragged = true;

        previousSlotGroup = GetComponentInParent<IPropZone>();
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent.Invoke(this);
        isDragging = false;
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        imageComponent.raycastTarget = true;

        StartCoroutine(FrameWait());
        IEnumerator FrameWait()
        {
            yield return new WaitForEndOfFrame();
            wasDragged = false;
        }

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        raycaster.Raycast(pointerData, results);

        bool droppedInZone = false;
        IPropZone targetZone = null;

        foreach (RaycastResult result in results)
        {
            targetZone = result.gameObject.GetComponent<IPropZone>();
            if (targetZone != null)
            {
                droppedInZone = true;
                break;
            }
        }

        if (droppedInZone && targetZone != null)
        {
            if (targetZone.CurrentProp != null && targetZone.CurrentProp != this)
            {
                Prop other = targetZone.CurrentProp;

                if (previousSlotGroup != null)
                {
                    previousSlotGroup.ClearSlot(this);
                    previousSlotGroup.AssignProp(other);
                }
                else if (currentDropZone != null)
                {
                    currentDropZone.ClearSlot(this);
                    currentDropZone.AssignProp(other);
                }
                else
                {
                    other.ReturnToHand();
                }
            }

            currentDropZone?.ClearSlot(this);
            targetZone.AssignProp(this);
            currentDropZone = targetZone;
        }
        else
        {
            if (previousSlotGroup != null)
            {
                previousSlotGroup.AssignProp(this);
                currentDropZone = previousSlotGroup;
            }
            else
            {
                ReturnToHand();
            }
        }
    }

    public void ClearCurrentSlot()
    {
        if (currentDropZone != null)
        {
            currentDropZone.ClearSlot(this);
            currentDropZone = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = true;
        tooltipTrigger.setTooltip(PropData.PropName,PropData.Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
        isHovering = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        PointerDownEvent.Invoke(this);
        pointerDownTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        pointerUpTime = Time.time;

        PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > .2f);
    }

    public void ReturnToHand()
    {
        if (previousSlotGroup != null)
        {
            previousSlotGroup.AssignProp(this);
            currentDropZone = previousSlotGroup;
        }
    }

    void OnDestroy()
    {
        if (propVisual != null)
            Destroy(propVisual.gameObject);
    }

    public void UpdateVisual()
    {
        if (PropData != null && propVisual != null)
        {
            propVisual.UpdateVisual(PropData);
        }
    }
}
