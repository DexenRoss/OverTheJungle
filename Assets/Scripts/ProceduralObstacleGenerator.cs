using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralObstacleGenerator : MonoBehaviour
{
    [SerializeField] private int numberOfObstacles = 150;
    [SerializeField] private Vector3 areaMin = new Vector3(-60, 0, -60);
    [SerializeField] private Vector3 areaMax = new Vector3(60, 0, 60);

    void Start()
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            // Crea un cubo
            GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Posición aleatoria
            obstacle.transform.position = new Vector3(
                Random.Range(areaMin.x, areaMax.x),
                0.5f,
                Random.Range(areaMin.z, areaMax.z)
            );

            // Aleatoriza escala y color
            obstacle.transform.localScale = Vector3.one * Random.Range(0.5f, 3f);
            obstacle.GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
    }
}