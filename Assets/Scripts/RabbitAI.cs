using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : MonoBehaviour
{
    [SerializeField] private float wanderSpeed = 3f;
    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float wanderInterval = 3f;
    private Vector3 targetPosition;
    private float timer;
    private RabbitSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<RabbitSpawner>();
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
        GenerateNewTarget();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= wanderInterval)
        {
            GenerateNewTarget();
            timer = 0;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);
    }

    void OnDestroy()
    {
        // Verifica si el spawner existe (puede ser null si se destruye primero)
        RabbitSpawner spawner = FindObjectOfType<RabbitSpawner>();
        if (spawner != null)
        {
            spawner.RabbitDestroyed();
        }
    }
    void GenerateNewTarget()
    {
        targetPosition = transform.position + Random.insideUnitSphere * wanderRadius;
        targetPosition.y = 0; // Mantener en el plano
    }
}
