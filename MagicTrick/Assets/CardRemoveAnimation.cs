using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardRemoveAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public UnityEvent<CardRemoveAnimation> PointerEnterEvent = new UnityEvent<CardRemoveAnimation>();
    public UnityEvent<CardRemoveAnimation> PointerExitEvent = new UnityEvent<CardRemoveAnimation>();
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        Debug.Log("AAAAAAAAAAAAAA");
        anim.SetBool("ishover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
        anim.SetBool("ishover", false);

    }
}
