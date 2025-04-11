using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MoveWithMouse : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.2f;
       [SerializeField] private Camera targetCamera;


    private void Start()
    {
        if (targetCamera == null)
        {
            GameObject camObj = GameObject.FindGameObjectWithTag("GameCamera");
            if (camObj != null)
                targetCamera = camObj.GetComponent<Camera>();

            if (targetCamera == null)
                Debug.LogWarning("Prop: No camera with tag 'GameCamera' found.");
        }
    }

    private void Update()
    {
       

        Vector3 mouseWorldPos = targetCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z; // keep original z

        transform.DOMove(mouseWorldPos, moveDuration).SetEase(Ease.OutQuad);
    }
}
