using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    [Header("Assets")] 
    [SerializeField] private GameObject bulletObject;
    
    [Header("References")]
    [SerializeField] private Transform bulletsParent;
    [SerializeField] private Transform bulletsOrigin;

    [Header("Config")] 
    [SerializeField] private float shootDelay;
    
    [Header("Bullets")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float bulletDamage;

    
    
    private float currentShootDelay;
    
    
    private ObjectPool<Bullet> bullets;

    private void Awake()
    {
        bullets = new ObjectPool<Bullet>(
            () => Instantiate(bulletObject, bulletsOrigin.position, Quaternion.identity, bulletsParent).GetComponent<Bullet>(),
            bullet => bullet.Shoot(bulletsOrigin, KillBullet, bulletSpeed, bulletLifeTime, bulletDamage), // Run shoot function whenever a new bullet is needed
            bullet => bullet.gameObject.SetActive(false),
            bullet => Destroy(bullet.gameObject)
        );
    }

    private void Update()
    {
        if (currentShootDelay > 0)
            currentShootDelay -= Time.deltaTime;
        
        // Shooting logic
        if ( currentShootDelay <= 0 && Input.GetMouseButton(0))
            Shoot();
    }
    
    /// <summary>
    /// Ran when the player shoots their gun
    /// </summary>
    private void Shoot()
    {
        // Use object pool to spawn bullet
        bullets.Get();
        
        // Add delay to each shot
        currentShootDelay = shootDelay;
    }

    /// <summary>
    /// Returns a bullet to the object pool once done
    /// </summary>
    private void KillBullet(Bullet bullet)
    {
        bullets.Release(bullet);
    }
}
