using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArmtowards : MonoBehaviour
{
    [Header("Camera & Sensitivity")]
    public Camera targetCamera;
    [Range(0f, 2f)] public float sensitivity = 1.0f;

    [Header("Rotation Settings")]
    public float maxRotationDegrees = 180f; // Arc size
    public Vector3 aimAxis = Vector3.forward; // Axis that should aim forward
    public Vector3 rotationOffset;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        float mouseX01 = Mathf.Clamp01(Input.mousePosition.x / Screen.width); // 0 = left, 1 = right
        float targetAngle = (0.5f - mouseX01) * maxRotationDegrees * sensitivity; // Flipped direction

        // Create rotation only around Y axis
        Quaternion baseRotation = Quaternion.Euler(0, targetAngle, 0);

        // Apply axis correction and offset
        Quaternion axisAdjust = Quaternion.FromToRotation(aimAxis, Vector3.forward);
        transform.rotation = baseRotation * axisAdjust * Quaternion.Euler(rotationOffset);
    }
}
