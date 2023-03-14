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
            Vector2 randomPoint = RandomPointInAnnulus(innerRadius, outerRadius);
            
            Debug.Log("Attempt to spawn at: " + randomPoint);

            randomPoint += new Vector2(_playerTransform.position.x, _playerTransform.position.z);
                
            // Debug.Log("point 2: " + randomPoint);

            Ray ray = new Ray(new Vector3(randomPoint.x, 100f, randomPoint.y), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 150))
            {
                Vector3 spawnPosition = hit.point + Vector3.up * 4;
                Instantiate(enemy, spawnPosition, new Quaternion(0,0,0,0));
                Debug.Log("spawn successful");
                yield return new WaitForSeconds(spawnRandomEnemyTime);
            }
        }
    }
    
    public Vector2 RandomPointInAnnulus(float minRadius, float maxRadius)
    {
        // A variable that holds a random direction
        var randomDirection = Random.insideUnitCircle.normalized;
        // A variable that holds a random distance between minRadius and maxRadius
        var randomDistance = Random.Range(minRadius, maxRadius);
        // A variable that holds a random point in the annulus arc
        var randomPoint = randomDirection * randomDistance;
        // Check if the x value of the random point is negative
        if (Mathf.Sign(randomPoint.x) == -1f)
        {
            // If so, multiply it by -1 to make it positive
            randomPoint.x *= -1;
        }
        // Return the random point with a positive x value
        return randomPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
