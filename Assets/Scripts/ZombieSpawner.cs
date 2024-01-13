using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Assets")] 
    [SerializeField] private GameObject zombieObject;

    [Header("Config")] 
    [SerializeField] private Vector2 spawnDelay;
    [SerializeField] private Vector2 spawnCount;
    [SerializeField] private int maxZombieCount = 100;
    [SerializeField] private float minPlayerDistance = 15;
    [SerializeField] private float maxPlayerDistance = 40;

    private List<Zombie> zombies = new List<Zombie>();

    private bool pause;

    private Camera mainCamera;
    private NavMeshTriangulation triangulation;
    private Mesh navMesh;

    

    private void Start()
    {
        StartCoroutine(ZombieSpawning());
        
        mainCamera = Camera.main;
        triangulation = NavMesh.CalculateTriangulation();
        navMesh = new Mesh();
        navMesh.vertices = triangulation.vertices;

    }

    /// <summary>
    /// Loops throughout the whole game spawning zombies at random intervals
    /// </summary>
    private IEnumerator ZombieSpawning()
    {
        while (true)
        {
            float delay = Random.Range(spawnDelay.x, spawnDelay.y);

            yield return new WaitForSeconds(delay);

            if (pause) yield return null;

            SpawnZombies();

            // Prevent spawning more zombies than the max count
            while (zombies.Count >= maxZombieCount)
                yield return null;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    /// <summary>
    /// Spawns a horde of zombies at a random position
    /// </summary>
    private void SpawnZombies()
    {
        int randomCount = Mathf.RoundToInt(Random.Range(spawnCount.x, spawnCount.y));
        Vector3 randomPosition = CalculateSpawnPosition();
        for(int i = 0; i < randomCount; i++)
        {
            // Create zombie
            Zombie newZombie = Instantiate(zombieObject, transform).GetComponent<Zombie>();
            newZombie.Spawn(randomPosition, KillZombie);
            zombies.Add(newZombie);
        }
    }

    /// <summary>
    /// Create a random position to spawn a zobie
    /// </summary>
    /// <returns>Random position that is out of sight of the player.</returns>

    private Vector3 CalculateSpawnPosition()
    {
        Vector3 position;
        do
        {
            position = GetRandomPosition();

            // Repeatedly generate positions until its out of camera view and far enough from the player
        } while (Vector3.Distance(position, GameManager.Instance.Player.transform.position) < minPlayerDistance || IsVisible(position));
        
        return position;


    }

    /// <summary>
    /// Generate a random Vector3 on the maps navmesh
    /// </summary>
    /// <returns>Random position on navmesh.</returns>

    private Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * maxPlayerDistance;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, maxPlayerDistance, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }
    
    /// <summary>
    /// Calculates if position is out of camera view or obstructed by walls
    /// </summary>
    /// <returns>A bool stating if the position is visible or not.</returns>
    /// <param name="position">The point to check the visibility of.</param>
    private bool IsVisible(Vector3 position)
    {
        Vector3 cameraPosition = mainCamera.WorldToViewportPoint(position);
        // Uses -0.1 and 1.1 to add extra padding to prevent zombie spawning while half onscreen
        if (!(cameraPosition.x >= -0.1f) || !(cameraPosition.x <= 1.1f) || !(cameraPosition.y >= -0.1f) || !(cameraPosition.y <= 1.1f) || !(cameraPosition.z > -0.1f)) 
            return false;
        
        // Check if position is obstructed by walls
        // Linecast rather than raycast lets me determine the start and end position
        if (Physics.Linecast(position, mainCamera.transform.position, out RaycastHit hit) && hit.collider.gameObject.layer == 6)
                return false;

        return true;

    }

    /// <summary>
    /// Handles logic for when a zombie is killed in the map
    /// </summary>
    /// <param name="zombie">The script of the zombie thats being killed.</param>
    private void KillZombie(Zombie zombie)
    {
        zombies.Remove(zombie);
    }
}
