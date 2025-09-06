using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PantherAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float pursueSpeed = 6f;
    [SerializeField] private List<Transform> waypoints;

    [Header("Chase Settings")]
    [SerializeField] private float chaseStartTime = 10f;
    [SerializeField] private float attackRange = 1.5f;

    private int currentWaypoint = 0;
    private float timer;
    private bool isChasing = false;
    private GameObject currentTarget;
    private bool playerWasTarget = false;

    void Update()
    {
        timer += Time.deltaTime;

        if (!isChasing && timer >= chaseStartTime)
        {
            isChasing = true;
            FindNearestTarget();
        }

        if (isChasing)
        {
            if (currentTarget != null)
            {
                ChaseTarget();
            }
            else
            {
                ReturnToPatrol();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Transform wp = waypoints[currentWaypoint];
        transform.position = Vector3.MoveTowards(transform.position, wp.position, patrolSpeed * Time.deltaTime);
        transform.LookAt(wp.position);

        if (Vector3.Distance(transform.position, wp.position) < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
        }
    }

    void ChaseTarget()
    {
        // Primero verifica si el objetivo aún existe
        if (currentTarget == null)
        {
            ReturnToPatrol();
            return;
        }

        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distance <= attackRange)
        {
            DestroyTarget();
        }
        else
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.position += direction * pursueSpeed * Time.deltaTime;
            transform.LookAt(currentTarget.transform.position);
        }
    }

    void DestroyTarget()
    {
        if (currentTarget != null) // Verificación crucial
        {
            playerWasTarget = currentTarget.CompareTag("Player");
            Destroy(currentTarget);
            currentTarget = null; // Limpia la referencia inmediatamente
        }

        if (!playerWasTarget)
        {
            ReturnToPatrol();
        }
    }

    void ReturnToPatrol()
    {
        isChasing = false;
        timer = 0f;
        currentWaypoint = GetClosestWaypoint();
    }

    int GetClosestWaypoint()
    {
        int closestIndex = 0;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < waypoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        targets = targets.Concat(GameObject.FindGameObjectsWithTag("Rabbit")).ToArray();

        float closestDistance = Mathf.Infinity;
        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = target;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
