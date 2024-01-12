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
    
    
    private ObjectPool<Bullet> bullets;

    private void Awake()
    {
        bullets = new ObjectPool<Bullet>(
            () => Instantiate(bulletObject, bulletsOrigin.position, Quaternion.identity, bulletsParent).GetComponent<Bullet>(),
            bullet => bullet.Shoot(bulletsOrigin, KillBullet), // Run shoot function whenever a new bullet is needed
            bullet => bullet.gameObject.SetActive(false),
            bullet => Destroy(bullet.gameObject)
        );
    }

    private void Update()
    {
        // Shooting logic
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }
    
    /// <summary>
    /// Ran when the player shoots their gun
    /// </summary>
    private void Shoot()
    {
        // Use object pool to spawn bullet
        bullets.Get();
    }

    /// <summary>
    /// Returns a bullet to the object pool once done
    /// </summary>
    private void KillBullet(Bullet bullet)
    {
        bullets.Release(bullet);
    }
}
