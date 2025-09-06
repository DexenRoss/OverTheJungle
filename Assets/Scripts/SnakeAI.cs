using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float pursueSpeed = 4f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Targeting")]
    [SerializeField] private float targetUpdateInterval = 2f;

    private GameObject targetRabbit;
    private float lastTargetUpdateTime;
    private RabbitSpawner spawner;
    private float lastAttackTime;
    private bool isAttacking;

    void Start()
    {
        spawner = FindObjectOfType<RabbitSpawner>();
        FindNearestRabbit();
    }

    void Update()
    {
        if (Time.time - lastTargetUpdateTime > targetUpdateInterval)
        {
            FindNearestRabbit();
            lastTargetUpdateTime = Time.time;
        }

        if (targetRabbit != null && !isAttacking)
        {
            PursueTarget();
        }
    }

    void PursueTarget()
    {
        if (targetRabbit == null)
        {
            FindNearestRabbit();
            return;
        }

        float distance = Vector3.Distance(transform.position, targetRabbit.transform.position);

        if (distance <= attackRange)
        {
            StartAttack();
        }
        else
        {
            Vector3 direction = (targetRabbit.transform.position - transform.position).normalized;
            transform.position += direction * pursueSpeed * Time.deltaTime;
            transform.LookAt(targetRabbit.transform.position);
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        DestroyTarget();
    }

    void DestroyTarget()
    {
        if (targetRabbit != null)
        {
            // Notifica al spawner
            if (spawner != null)
            {
                spawner.RabbitDestroyed();
            }

            Destroy(targetRabbit);
            targetRabbit = null;

            // Cooldown antes de buscar nuevo objetivo
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        FindNearestRabbit();
    }

    void FindNearestRabbit()
    {
        GameObject[] rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
        float closestDistance = Mathf.Infinity;
        GameObject nearestRabbit = null;

        foreach (GameObject rabbit in rabbits)
        {
            if (rabbit == null) continue;

            float distance = Vector3.Distance(transform.position, rabbit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestRabbit = rabbit;
            }
        }

        targetRabbit = nearestRabbit;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rabbit") && !isAttacking)
        {
            targetRabbit = collision.gameObject;
            StartAttack();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}