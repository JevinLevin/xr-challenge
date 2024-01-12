using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void CreateSingleton()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return;
        } 
        else 
        { 
            Instance = this; 
        } 

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
    
    
    public Player Player { get; set; }
    public static Action OnGameStart;
    public static Action OnEscapeStart;
    public static Action<int> OnUpdateScore;

    private List<Pickup> pickups;

    private int score;
    private float timer;
    
    public bool IsGameActive { get; private set; }
    public bool IsEscaping { get; private set; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += StartGame;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= StartGame;
    }
    
    private void Awake()
    {
        CreateSingleton();

        IsEscaping = false;
    }

    private void Start()
    {
        // I would do this in the pickup script instead but I can't alter it...
        pickups = FindObjectsOfType<Pickup>().ToList();
        foreach (Pickup pickup in pickups)
        {
            pickup.OnPickUp += CheckPickups;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    /// <summary>
    /// Checks if all the pickups have been collected
    /// </summary>
    private void CheckPickups(Pickup pickup)
    {
        pickups.Remove(pickup);
        
        if(pickups.Count <= 0)
            StartEscape();
    }

    /// <summary>
    /// Ran when the game scene loads
    /// </summary>
    private void StartGame(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name != "Main") return;

        IsGameActive = true;
        OnGameStart?.Invoke();

        timer = 0.0f;
    }

    /// <summary>
    /// Triggers the escape phase of the game
    /// </summary>
    private void StartEscape()
    {
        print("ESCAPE");

        IsEscaping = true;
        
        OnEscapeStart?.Invoke();
    }

    /// <summary>
    /// Adds to the total score
    /// </summary>
    public void GiveScore(int addScore)
    {
        score += addScore;
        
        OnUpdateScore?.Invoke(score);
    }

    /// <summary>
    /// Ran whenever the game ends
    /// </summary>
    public void EndGame()
    {
        IsGameActive = false;
    }

    /// <summary>
    /// Ran once the player escapes
    /// </summary>
    public void WinGame()
    {
        EndGame();
        IsEscaping = false;
    }

    public float GetCurrentTime()
    {
        return timer;
    }
}
