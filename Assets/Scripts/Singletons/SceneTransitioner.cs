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

    public void ReloadCurrentScene(string transitionMsg)
    {

        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name, defaultInLength, defaultOutLength, transitionMsg));
    }


    private IEnumerator LoadScene(string sceneName, float inLength, float outLength, string transitionMsg)
    {
        transitionScreen.FadeIn(inLength, transitionMsg);

        yield return new WaitForSeconds(inLength + transitionDelay);

        SceneManager.LoadScene(sceneName);

        transitionScreen.FadeOut(outLength);
    }
}
