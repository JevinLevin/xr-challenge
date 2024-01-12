using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    

    private void Awake()
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

        IsEscaping = false;
    }
    #endregion
    
    
    public Player Player { get; set; }
    public static Action<int> OnUpdateScore;
    public static Action<bool> OnEscapeStart;

    private List<Pickup> pickups;

    private int score;
    public bool IsEscaping { get; private set; }

    private void Start()
    {
        // I would do this in the pickup script instead but I can't alter it...
        pickups = FindObjectsOfType<Pickup>().ToList();
        foreach (Pickup pickup in pickups)
        {
            pickup.OnPickUp += CheckPickups;
        }
    }

    private void CheckPickups(Pickup pickup)
    {
        pickups.Remove(pickup);
        
        if(pickups.Count <= 0)
            StartEscape();
    }

    private void StartEscape()
    {
        print("ESCAPE");

        IsEscaping = true;
        
        OnEscapeStart?.Invoke(true);
    }

    public void GiveScore(int addScore)
    {
        score += addScore;
        
        OnUpdateScore?.Invoke(score);
    }

    public void WinGame()
    {
        IsEscaping = false;
    }
}
