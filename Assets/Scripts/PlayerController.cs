using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float mouseSensitivity = 0.00001f;
    private CharacterController controller;
    private Camera mainCamera;

    void Start()
    {

        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Importante para movimiento con CharacterController
        }
    }

    void Update()
    {
        // Movimiento libre (WASD)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Rotación hacia el mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(0, mouseX, 0);
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        
        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 lookPoint = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(lookPoint.x, transform.position.y, lookPoint.z));
        
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Snake") || collision.gameObject.CompareTag("Panther"))
        {
            TakeDamage(10f);
        }
    }
}
