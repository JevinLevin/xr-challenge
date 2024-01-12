using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public void OnStart()
    {
        GameManager.Instance.LoadGame();
    }
}
