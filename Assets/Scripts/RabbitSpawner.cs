using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RabbitSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject rabbitPrefab;
    [SerializeField] private float initialSpawnDelay = 5f;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private int maxRabbits = 10;
    [SerializeField] private Vector2 spawnArea = new Vector2(20f, 20f);

    [Header("Progressive Difficulty")]
    [SerializeField] private float intervalReductionPerSpawn = 0.5f;
    [SerializeField] private float minSpawnInterval = 3f;

    private int currentRabbits = 0;

    void Start()
    {
        StartCoroutine(SpawnRabbits());
    }

    IEnumerator SpawnRabbits()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            if (currentRabbits < maxRabbits && rabbitPrefab != null) // Verifica prefab
            {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                    0,
                    Random.Range(-spawnArea.y / 2, spawnArea.y / 2)
                );

                // Verifica posición segura (opcional)
                if (PositionIsSafe(spawnPosition))
                {
                    Instantiate(rabbitPrefab, spawnPosition, Quaternion.identity);
                    currentRabbits++;
                    spawnInterval = Mathf.Max(spawnInterval - intervalReductionPerSpawn, minSpawnInterval);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    bool PositionIsSafe(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, 2f);
        return !colliders.Any(c => c != null && (c.CompareTag("Panther") || c.CompareTag("Snake")));
    }

    // Llamar desde RabbitAI cuando un conejo es destruido
    public void RabbitDestroyed()
    {
        currentRabbits--;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, 0.1f, spawnArea.y));
    }
}