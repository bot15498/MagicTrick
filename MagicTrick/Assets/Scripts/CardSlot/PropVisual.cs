using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;
using Unity.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PropVisual : MonoBehaviour
{
    private bool initalize = false;

    [Header("Prop")]
    public Prop parentProp;
    public Transform propTransform;
    private Vector3 rotationDelta;
    private int savedIndex;
    Vector3 movementDelta;
    private Canvas canvas;

    [Header("References")]
    [SerializeField] private Transform shakeParent;
    [SerializeField] private Transform tiltParent;
    [SerializeField] private Image propImage;

    [Header("Follow Parameters")]
    [SerializeField] private float followSpeed = 30;

    [Header("Rotation Parameters")]
    [SerializeField] private float rotationAmount = 20;
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private float autoTiltAmount = 30;
    [SerializeField] private float manualTiltAmount = 20;
    [SerializeField] private float tiltSpeed = 20;

    [Header("Scale Parameters")]
    [SerializeField] private bool scaleAnimations = true;
    [SerializeField] private float scaleOnHover = 1.15f;
    [SerializeField] private float scaleOnSelect = 1.25f;
    [SerializeField] private float scaleTransition = .15f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    [Header("Select Parameters")]
    [SerializeField] private float selectPunchAmount = 20;

    [Header("Hover Parameters")]
    [SerializeField] private float hoverPunchAngle = 5;
    [SerializeField] private float hoverTransition = .15f;

    [Header("Swap Parameters")]
    [SerializeField] private bool swapAnimations = true;
    [SerializeField] private float swapRotationAngle = 30;
    [SerializeField] private float swapTransition = .15f;
    [SerializeField] private int swapVibrato = 5;

    [Header("Curve")]
    [SerializeField] private CurveParameters curve;

    private float curveYOffset;
    private float curveRotationOffset;
    private Coroutine pressCoroutine;

    private void Start()
    {
      
    }

    public void Initialize(Prop target, int index = 0)
    {
        parentProp = target;
        propTransform = target.transform;
        canvas = GetComponent<Canvas>();


        parentProp.PointerEnterEvent.AddListener(PointerEnter);
        parentProp.PointerExitEvent.AddListener(PointerExit);
        parentProp.BeginDragEvent.AddListener(BeginDrag);
        parentProp.EndDragEvent.AddListener(EndDrag);
        parentProp.PointerDownEvent.AddListener(PointerDown);
        parentProp.PointerUpEvent.AddListener(PointerUp);

        initalize = true;
    }

    public void UpdateIndex(int length)
    {
        transform.SetSiblingIndex(parentProp.transform.parent.GetSiblingIndex());
    }

    void Update()
    {
        if (!initalize || parentProp == null) return;

        
        SmoothFollow();
        FollowRotation();
        PropTilt();

    }



    private void SmoothFollow()
    {
        Vector3 verticalOffset = (Vector3.up * (parentProp.isDragging ? 0 : curveYOffset));
        transform.position = Vector3.Lerp(transform.position, propTransform.position + verticalOffset, followSpeed * Time.deltaTime);
    }

    private void FollowRotation()
    {
        Vector3 movement = (transform.position - propTransform.position);
        movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
        Vector3 movementRotation = (parentProp.isDragging ? movementDelta : movement) * rotationAmount;
        rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -60, 60));
    }

    private void PropTilt()
    {
        
    }

 

    public void Swap(float dir = 1)
    {
        if (!swapAnimations)
            return;

        DOTween.Kill(2, true);
        shakeParent.DOPunchRotation((Vector3.forward * swapRotationAngle) * dir, swapTransition, swapVibrato, 1).SetId(3);
    }

    private void BeginDrag(Prop prop)
    {
        if (scaleAnimations)
            transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);

        canvas.overrideSorting = true;
    }

    private void EndDrag(Prop prop)
    {
        canvas.overrideSorting = false;
        transform.DOScale(1, scaleTransition).SetEase(scaleEase);
    }

    private void PointerEnter(Prop prop)
    {
        if (scaleAnimations)
            transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);

        DOTween.Kill(2, true);
        shakeParent.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
    }

    private void PointerExit(Prop prop)
    {
        if (!parentProp.wasDragged)
            transform.DOScale(1, scaleTransition).SetEase(scaleEase);
    }

    private void PointerUp(Prop prop, bool longPress)
    {
        if (scaleAnimations)
            transform.DOScale(longPress ? scaleOnHover : scaleOnSelect, scaleTransition).SetEase(scaleEase);
        canvas.overrideSorting = false;

    }

    private void PointerDown(Prop prop)
    {
        if (scaleAnimations)
            transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);

    }

    public void UpdateVisual(PlayableProp PropData)
    {
        propImage.sprite = PropData.Image;
    }
}
