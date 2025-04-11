using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UIElements;
using System.Security.Cryptography;

public class BuyCardPack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Start is called before the first frame update


    [Header("Scale Parameters")]
    [SerializeField] private bool scaleAnimations = true;
    [SerializeField] private float scaleOnHover = 1.15f;
    [SerializeField] private float scaleOnSelect = 1.25f;
    [SerializeField] private float scaleTransition = 0.15f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;
    [HideInInspector] public UnityEvent<BuyCardPack> PointerEnterEvent;

    [Header("Tilt Settings")]
    public float autoTiltAmount = 5f;      // Max degrees of tilt
    public float tiltSpeed = 2f;           // Lerp speed
    public float tiltFrequency = 1f;       // How fast it sways

    private float tiltX, tiltY, tiltZ;     // Base rotation
    private Transform tiltTarget;
    void Start()
    {
        tiltTarget = transform;
        Vector3 startAngles = tiltTarget.eulerAngles;
        tiltX = startAngles.x;
        tiltY = startAngles.y;
        tiltZ = startAngles.z;
    }

    void Update()
    {
        float time = Time.time * tiltFrequency;
        float sine = Mathf.Sin(time);
        float cosine = Mathf.Cos(time);

        float lerpX = Mathf.LerpAngle(tiltTarget.eulerAngles.x, tiltX + (sine * autoTiltAmount), tiltSpeed * Time.deltaTime);
        float lerpY = Mathf.LerpAngle(tiltTarget.eulerAngles.y, tiltY + (cosine * autoTiltAmount), tiltSpeed * Time.deltaTime);
        float lerpZ = Mathf.LerpAngle(tiltTarget.eulerAngles.z, tiltZ, tiltSpeed / 2f * Time.deltaTime); // Less movement on Z

        tiltTarget.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent?.Invoke(this);


        if (scaleAnimations)
            transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleAnimations)
            transform.DOScale(1f, scaleTransition).SetEase(scaleEase);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //MINUS MONEY DISABLE CARD REMOVAL 
        //ADD REMOVAL EFFECT HERE

     
        TooltipSystem.Instance.HideTooltip();

    }




}
