using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 velocity;

    public static Bullet Fire(GameObject prefab, Vector3 position, Vector3 direction, float speed)
    {
        var go = Instantiate<GameObject>(prefab, position, Quaternion.identity);

        var bullet = go.AddComponent<Bullet>();

        bullet.velocity = direction * speed;

        TTL.Attach(go, 1.5f);

        return bullet;
    }

    bool dead;

    void FixedUpdate()
    {
        if (dead)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 position = transform.position;

        Vector3 step = velocity * Time.deltaTime;

        position += step;

        float distance = step.magnitude;

        // not the player
        int layerMask = ~(1 << Spaceship3D.PlayerLayer);

        var ray = new Ray(origin: position, direction: step);
        RaycastHit hit;
        if (Physics.Raycast( ray, hitInfo: out hit, maxDistance: distance, layerMask: layerMask))
		{
            position = hit.point;

            dead = true;

            // TODO: the rock we hit!
            var id = hit.collider.GetComponent<IDamageable>();
            if (id != null)
			{
                id.TakeDamage(1);
			}
		}

        transform.position = position;
    }
}
