using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bulletSpeed;
    private float bulletLifeTime;
    private float bulletDamage;

    private float lifetime;
    private Action<Bullet> killEvent;

    private void Update()
    {
        // Bullet movement
        float speed = bulletSpeed * Time.deltaTime;
        transform.position += transform.forward * speed;

        // Bullet lifetime
        lifetime += Time.deltaTime;
        if (lifetime > bulletLifeTime)
            killEvent?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") return;

        // Stop from colliding with ground
        if (other.gameObject.layer == 7) return;
        
        if (other.TryGetComponent(out Zombie zombieScript))
        {
            zombieScript.TakeDamage(bulletDamage);
        }
        
        killEvent?.Invoke(this);
    }
    
    /// <summary>
    /// Inititalises the bullet each time its shot
    /// </summary>
    /// <param name="origin">Transform of the object spawning the bullet.</param>
    /// <param name="killEvent">Action to be ran once the bullet is killed</param>
    /// <param name="speed">Speed at which the bullet will travel in</param>
    /// <param name="lifeTime">How long the bullet will last before being automatically killed.</param>
    /// <param name="damage">How much health the bullet will take off an enemy it hits.</param>
    public void Shoot(Transform origin, Action<Bullet> killEvent, float speed, float lifeTime, float damage)
    {
        gameObject.SetActive(true);
        
        // Initialise transform
        transform.rotation = Quaternion.Euler(origin.rotation.eulerAngles);
        transform.position = origin.position;
        
        lifetime = 0.0f;
        
        // Used to return bullet to object pool once destroyed
        this.killEvent = killEvent;
        
        // Set attributes
        bulletSpeed = speed;
        bulletLifeTime = lifeTime;
        bulletDamage = damage;
    }
}
