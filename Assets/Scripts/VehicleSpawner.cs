using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    private ObjectPool carPool;

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float maxSpawnDelay = 2f;
    [SerializeField] float spawnXOffset = 1f;

    private void Awake()
    {
        carPool = FindObjectOfType<ObjectPool>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnVehicle), 0, Random.Range(1, maxSpawnDelay));
    }

    private void SpawnVehicle()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];
        Vector3 spawnPosition = spawnPoint.position + Vector3.forward * Random.Range(-spawnXOffset, spawnXOffset);

        carPool.SpawnFromPool("Car", spawnPosition, spawnPoint.rotation);
    }
}
