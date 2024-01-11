using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifetime;
    [SerializeField] private float bulletDamage;

    private float lifetime;
    private Action<Bullet> killEvent;

    private void Update()
    {
        // Bullet movement
        float speed = bulletSpeed * Time.deltaTime;
        transform.position += transform.forward * speed;

        // Bullet lifetime
        lifetime += Time.deltaTime;
        if (lifetime > bulletLifetime)
            killEvent?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") return;
        
        if (other.TryGetComponent(out Zombie zombieScript))
        {
            zombieScript.TakeDamage(bulletDamage);
        }
        
        killEvent?.Invoke(this);
    }
    
    /// <summary>
    /// Inititalises the bullet each time its shot
    /// </summary>
    public void Shoot(Transform origin, Action<Bullet> killEvent)
    {
        gameObject.SetActive(true);
        
        // Initialise transform
        transform.rotation = Quaternion.Euler(origin.rotation.eulerAngles);
        transform.position = origin.position;
        
        lifetime = 0.0f;
        
        // Used to return bullet to object pool once destroyed
        this.killEvent = killEvent;
    }
}
