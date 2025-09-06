using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMouseLook : MonoBehaviour
{
    [Range(50f, 500f)]
    public float mouseSensitivity = 100f; // Ajustable desde el Inspector
    public Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Opcional para juegos FPS
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Evita voltear 180°

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX); // Rotación horizontal suave
    }
}
