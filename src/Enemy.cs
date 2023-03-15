using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Ground Object")]
    GameObject ground;

    [Header("Particle System")]
    [SerializeField] ParticleSystem asteroidGroundExplosion;
    ParticleSystem asteroidGroundExplosionInstance;

    void Update()
    {
        // Rotate asteroid as it falls
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime * .5f);
    }

    // Asteroid collides with base
    void OnCollisionEnter(Collision other)
    {
        // Assign a Base class instance to the object colliding with the asteroid
        Base theBase = other.transform.GetComponent<Base>();

        if(theBase != null)
        {
            // Apply damage to the base
            theBase.Damage();

            // Create explosion when asteroid hits the base
            asteroidGroundExplosionInstance = Instantiate(asteroidGroundExplosion, transform.position, Quaternion.identity);

            // Destroy asteroid Game Object
            Destroy(this.gameObject);
        }

    }   
}
