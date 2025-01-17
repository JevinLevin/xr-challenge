using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    
    [Header("Config")] 
    [SerializeField] private float transitionDelay;
    [SerializeField] private float defaultInLength;
    [SerializeField] private float defaultOutLength;
    public static SceneTransitioner Instance { get; private set; }
    private TransitionScreen transitionScreen;

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

        transitionScreen = GetComponentInChildren<TransitionScreen>(true);
    }

    /// <summary>
    /// Reloads the currently playing scene, useful for resetting the game
    /// </summary>
    /// <param name="transitionMsg">Text to display on the screen during the transition.</param>
    public void ReloadCurrentScene(string transitionMsg)
    {

        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name, defaultInLength, defaultOutLength, transitionMsg));
    }

    /// <summary>
    /// Loads the passed scene
    /// </summary>
    /// <param name="sceneName">Name of the scene set to be loaded</param>
    /// <param name="transitionMsg">Text to display on the screen during the transition.</param>
    public void LoadSelectedScene(string sceneName, string transitionMsg)
    {
        StartCoroutine(LoadScene(sceneName, defaultInLength, defaultOutLength, transitionMsg));
    }


    /// <summary>
    /// Loads the scene provided with a transition animation
    /// </summary>
    /// <param name="sceneName">Name of the scene set to be loaded</param>
    /// <param name="inLength">Duration in seconds of first section of the transition</param>
    /// <param name="outLength">Duration in seconds of second section of the transition</param>
    /// <param name="transitionMsg">Text to display on the screen during the transition.</param>
    private IEnumerator LoadScene(string sceneName, float inLength, float outLength, string transitionMsg)
    {
        transitionScreen.FadeIn(inLength, transitionMsg);

        yield return new WaitForSeconds(inLength + transitionDelay);

        SceneManager.LoadScene(sceneName);

        transitionScreen.FadeOut(outLength);
    }
}
