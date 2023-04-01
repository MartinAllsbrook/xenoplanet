using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private int spawnChance;

    public void SpawnEnemy()
    {
        if (Random.Range(1,101) < spawnChance )
            return;

        var x = Random.Range(1, 65);
        var z = Random.Range(1, 65);

        Vector2 randomPoint = new Vector2(transform.position.x + x, transform.position.z + z);
        
        Ray ray = new Ray(new Vector3(randomPoint.x, 100f, randomPoint.y), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 150))
        {
            Vector3 spawnPosition = hit.point + Vector3.up * 4;
            int randomIndex = Random.Range(0, enemys.Length);
            Instantiate(enemys[randomIndex], spawnPosition, new Quaternion(0,0,0,0));
        }
    }
}
