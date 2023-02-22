using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float spawnRandomEnemyTime;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;

    private Transform _playerTransform;
    private void Start()
    {
        _playerTransform = Player.Instance.transform;
        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            Debug.Log("SpawnEnemy");
            Vector2 randomPoint = RandomPointInAnnulus(innerRadius, outerRadius);
            
            Ray ray = new Ray(new Vector3(randomPoint.x + _playerTransform.position.x, 100f, randomPoint.y + _playerTransform.position.z), Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit, 100);
            Vector3 spawnPosition = hit.point + Vector3.up * 4;
            Instantiate(enemy, spawnPosition, new Quaternion(0,0,0,0));
            yield return new WaitForSeconds(spawnRandomEnemyTime);
        }
    }
    
    public Vector2 RandomPointInAnnulus(float minRadius, float maxRadius)
    {
        // A variable that holds a random direction
        var randomDirection = Random.insideUnitCircle.normalized;
        // A variable that holds a random distance between minRadius and maxRadius
        var randomDistance = Random.Range(minRadius, maxRadius);
        // Return a vector 2 that is the product of randomDirection and randomDistance
        return randomDirection * randomDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
