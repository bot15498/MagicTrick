using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private Canvas canvas;
    private Image imageComponent;
    [SerializeField] private bool instantiateVisual = true;
    private VisualCardsHandler visualHandler;
    private Vector3 offset;

    [Header("Movement")]
    [SerializeField] private float moveSpeedLimit = 50;

    [Header("Selection")]
    public bool selected;
    public float selectionOffset = 50;
    private float pointerDownTime;
    private float pointerUpTime;
    public bool canBeSelected = true;

    [Header("Visual")]
    [SerializeField] private GameObject cardVisualPrefab;
    [HideInInspector] public CardVisual cardVisual;

    [Header("States")]
    public bool isHovering;
    public bool isDragging;
    [HideInInspector] public bool wasDragged;

    [Header("Card Data")]
    public PlayableCard CardData;

    [Header("Events")]
    [HideInInspector] public UnityEvent<Card> PointerEnterEvent;
    [HideInInspector] public UnityEvent<Card> PointerExitEvent;
    [HideInInspector] public UnityEvent<Card, bool> PointerUpEvent;
    [HideInInspector] public UnityEvent<Card> PointerDownEvent;
    [HideInInspector] public UnityEvent<Card> BeginDragEvent;
    [HideInInspector] public UnityEvent<Card> EndDragEvent;
    [HideInInspector] public UnityEvent<Card, bool> SelectEvent;
    [HideInInspector] public CardDropZone previousSlotGroup;
    [HideInInspector] public CardDropZone currentDropZone;

    [SerializeField] private Camera targetCamera;

    [Header("Audio")]
    private AudioManager audioManager;
    [SerializeField] private AudioClip OnSelect;
    [SerializeField] private AudioClip OnUnselect;
    [SerializeField] private AudioClip OnCardDragStart;
    [SerializeField] private AudioClip OnCardPlace;

    private TooltipTrigger tooltipTrigger;

    public void Awake() { }

    void Start()
    {
        audioManager = AudioManager.Instance;
        canvas = GetComponentInParent<Canvas>();
        imageComponent = GetComponent<Image>();

        if (!instantiateVisual)
            return;

        visualHandler = FindObjectOfType<VisualCardsHandler>();
        cardVisual = Instantiate(cardVisualPrefab, visualHandler ? visualHandler.transform : canvas.transform).GetComponent<CardVisual>();
        cardVisual.Initialize(this);
        tooltipTrigger = GetComponent<TooltipTrigger>();

        if (targetCamera == null)
        {
            GameObject camObj = GameObject.FindWithTag("GameCamera");
            if (camObj != null)
                targetCamera = camObj.GetComponent<Camera>();

            if (targetCamera == null)
                Debug.LogWarning("Card: No camera with tag 'GameCamera' found.");
        }
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
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent.Invoke(this);
        isDragging = true;
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        imageComponent.raycastTarget = false;
        wasDragged = true;
        previousSlotGroup = transform.parent.GetComponentInParent<CardDropZone>();

        audioManager.PlayOneShot(OnCardDragStart, 1f);
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

        foreach (RaycastResult result in results)
        {
            var dropZone = result.gameObject.GetComponent<CardDropZone>();
            if (dropZone != null)
            {
                droppedInZone = true;

                if (currentDropZone != null && currentDropZone != dropZone)
                {
                    currentDropZone.ClearSlot(this);
                }

                if (dropZone.CurrentCard != null && dropZone.CurrentCard != this)
                {
                    dropZone.CurrentCard.ReturnToHand();
                }
                audioManager.PlayOneShot(OnCardPlace, 1f);

                dropZone.AssignCard(this);
                currentDropZone = dropZone;
                break;
            }
        }

        if (!droppedInZone)
        {
            if (currentDropZone != null)
            {
                currentDropZone.ClearSlot(this);
                currentDropZone = null;
            }

            ReturnToHand();
            TooltipTrigger tt = GetComponent<TooltipTrigger>();
            if (tt != null) tt.rightTooltip = false;
        }
        else
        {
            TooltipTrigger tt = GetComponent<TooltipTrigger>();
            if (tt != null)
                tt.rightTooltip = currentDropZone != null && currentDropZone.CompareTag("TrickSlot");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = true;
        tooltipTrigger.setTooltip(CardData.CardName ,CardData.Description);
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

        if (pointerUpTime - pointerDownTime > .2f || wasDragged || !canBeSelected)
            return;

        selected = !selected;
        if(selected)
        {
            audioManager.PlayOneShot(OnSelect, 1f);
        }
        else
        {
            audioManager.PlayOneShot(OnUnselect, 1f);
        }
        SetSelected(selected);
    }

    public void SetSelected(bool isSelected)
    {
        selected = isSelected;
        SelectEvent.Invoke(this, isSelected);

        if (isSelected)
            transform.localPosition += (cardVisual.transform.up * selectionOffset);
        else
            transform.localPosition = Vector3.zero;
    }

    public int SiblingAmount()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
    }

    public int ParentIndex()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    }

    public float NormalizedPosition()
    {
        return transform.parent.CompareTag("Slot") ? ExtensionMethods.Remap((float)ParentIndex(), 0, (float)(transform.parent.parent.childCount - 1), 0, 1) : 0;
    }

    private void OnDestroy()
    {
        if (cardVisual != null)
            Destroy(cardVisual.gameObject);
    }

    public void ReturnToHand()
    {
        HorizontalCardHolder hand = GameObject.FindObjectOfType<HorizontalCardHolder>();
        if (hand == null) return;

        Transform slot = transform.parent;
        slot.SetParent(hand.transform, false);
        slot.localPosition = Vector3.zero;

        if (!hand.cards.Contains(this))
            hand.cards.Add(this);

        tooltipTrigger.rightTooltip = false;
    }

    public void UpdateVisual()
    {
        if (CardData != null && cardVisual != null)
            cardVisual.UpdateVisual(CardData);
    }
}
