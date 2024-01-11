using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private float maxHealth;

    private float health;
    private Action<float> onHealthChange;
    
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
        player = GameManager.Instance.player.transform;
        healthbar.Setup(maxHealth, ref onHealthChange);
    }

    private void Update()
    {
        MoveToPlayer();
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
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
        
        onHealthChange?.Invoke(health);
    }

    /// <summary>
    /// Ran once the zombies health reaches 0
    /// </summary>
    private void Die()
    {
        Destroy(gameObject);
    }
}