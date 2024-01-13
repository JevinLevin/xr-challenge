using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private float maxHealth;
    [SerializeField] private int scoreValue;

    private float health;
    private Action<float> onHealthChange;
    private Action<Zombie> onKillZombie;
    
    private NavMeshAgent agent;
    private Transform player;
    private Healthbar healthbar;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        healthbar = GetComponentInChildren<Healthbar>();

        health = maxHealth;
    }

    private void Start()
    {
        player = GameManager.Instance.Player.transform;
        healthbar.Setup(maxHealth, ref onHealthChange);
    }

    private void Update()
    {
        MoveToPlayer();
    }

    /// <summary>
    /// Initialise zombie
    /// </summary>
    /// <param name="position">World point to spawn the zombie.</param>
    /// <param name="onKillZombie">Action to be ran when the zombie dies.</param>
    public void Spawn(Vector3 position, Action<Zombie> onKillZombie)
    {
        this.onKillZombie = onKillZombie;
        
        agent.Warp(position);
    }

    /// <summary>
    /// Move the zombie to the players position
    /// </summary>
    private void MoveToPlayer()
    {
        agent.destination = player.position;
    }

    /// <summary>
    /// Ran whenever the zombie takes damage
    /// </summary>
    /// <param name="damage">Value to damage the zombie by</param>
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
        
        onHealthChange?.Invoke(health);
    }

    /// <summary>
    /// Kills zombie once the health reaches 0
    /// </summary>
    private void Die()
    {
        onKillZombie?.Invoke(this);
        
        GameManager.Instance.GiveScore(scoreValue);
        
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Kills player on collision
        if (other.gameObject.CompareTag("Player"))
            GameManager.Instance.Player.Die();
    }
}
