using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : BreakableObject
{
    protected override void Die()
    {
        StartCoroutine(FallOver());
    }

    IEnumerator FallOver()
    {
        var rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.AddForce(RandomDirection() * 100f);
        
        if (gameObject.GetComponent<SphereCollider>())
            Destroy(gameObject.GetComponent<SphereCollider>());

        if (gameObject.GetComponent<CapsuleCollider>())
            Destroy(gameObject.GetComponent<CapsuleCollider>());
        
        yield return new WaitForSeconds(2f);
        // Instantiate the objects death particle system
        Instantiate(deathParticles, transform.position + transform.up * 2, new Quaternion(0,0,0,0));
        yield return new WaitForSeconds(1f);
        Disappear();
    }
    
    private Vector3 RandomDirection()
    {
        // get a random point inside a unit circle
        Vector2 direction = Random.insideUnitCircle;
        // handle the rare case of a zero vector
        if (direction == Vector2.zero)
        {
            // default to a unit vector pointing up
            direction = Vector2.up;
        }
        // return the normalized vector
        var normalized = direction.normalized;
        return new Vector3(normalized.x, 0, normalized.y);
    }
    
    private void Disappear()
    {
        for (int i = 0; i < itemDrops.Length; i++)
        {
            for (int j = 0; j < itemDrops[i].dropTries; j++)
            {
                if (Random.Range(1,101) < itemDrops[i].dropChance)
                    Instantiate(itemDrops[i].drop, transform.position, transform.rotation);
            }
        }
        
        Destroy(gameObject);
    }

    
}