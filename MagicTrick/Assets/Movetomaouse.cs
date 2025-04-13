using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movetomaouse : MonoBehaviour
{
    public Camera targetCamera;
    public float planeHeight = 1.0f; // Y level the arm operates on

    void Update()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, planeHeight, 0));

        if (plane.Raycast(ray, out float enter))
        {
            transform.position = ray.GetPoint(enter);
        }
    }
}
