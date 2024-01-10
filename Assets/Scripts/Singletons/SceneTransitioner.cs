using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
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

    public void ReloadCurrentScene(string transitionMsg)
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name, 1.0f, 1.0f, transitionMsg));
    }


    private IEnumerator LoadScene(string sceneName, float fadeInLength, float fadeOutLength, string transitionMsg)
    {
        transitionScreen.FadeIn(fadeInLength, transitionMsg);

        yield return new WaitForSeconds(fadeInLength);

        SceneManager.LoadScene(sceneName);
        yield return new WaitForEndOfFrame();
        
        transitionScreen.FadeOut(fadeOutLength);
    }
}
