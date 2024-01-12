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

    public int Score { get; private set; }
    public float Timer { get; private set; }
    public int FinalScore { get; private set; }
    public int HighScore { get; private set; }
    
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
    }

    private void Update()
    {
        if(IsGameActive)
            Timer += Time.deltaTime;
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
        IsEscaping = false;
        OnGameStart?.Invoke();

        StartCoroutine(FindPickups());
            
        Timer = 0.0f;
        Score = 0;

    }

    private IEnumerator FindPickups()
    {
        // Coroutine needs to wait a frame for the scene to load before checking
        // This is because the sceneloaded event runs before the awake function, and the gamemanager start function will run only in the scene the game starts in
        // Ideally all this logic would be handled in the pickup scripts themselves anyways but i cant alter them...
        yield return new WaitForEndOfFrame();
        
        pickups = FindObjectsOfType<Pickup>().ToList();
        foreach (Pickup pickup in pickups)
        {
            pickup.OnPickUp += CheckPickups;
        }
    }

    /// <summary>
    /// Triggers the escape phase of the game
    /// </summary>
    private void StartEscape()
    {
        IsEscaping = true;
        
        OnEscapeStart?.Invoke();
    }

    /// <summary>
    /// Adds to the total score
    /// </summary>
    public void GiveScore(int addScore)
    {
        Score += addScore;
        
        OnUpdateScore?.Invoke(Score);
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
        
        CalculateFinalScore();
        
        SceneTransitioner.Instance.LoadSelectedScene("WinScreen", "You Win!");
    }

    private void CalculateFinalScore()
    {
        // Calculates final score by subtracting the time taken
        FinalScore = Score - (int)Timer*5;

        // Sets the highscore to the current score if its higher
        HighScore = FinalScore > HighScore ? FinalScore : HighScore;
    }

    public void RetryGame()
    {
        SceneTransitioner.Instance.LoadSelectedScene("Main", "Loading Game...");
    }
}
