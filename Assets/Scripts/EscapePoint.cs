using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePoint : MonoBehaviour
{
    [Header("Assets")] 
    [SerializeField] private Material offMaterial;
    [SerializeField] private Material onMaterial;

    [Header("References")] 
    private MeshRenderer escapeRenderer;

    private void Awake()
    {
        escapeRenderer = GetComponent<MeshRenderer>();
        
        SetMaterial(false);
    }

    private void Start()
    {
        GameManager.OnEscapeStart += SetMaterial;
    }

    /// <summary>
    /// Changes the material of the escape point depending on if the player can currently escape or not
    /// </summary>
    private void SetMaterial(bool state)
    {
        escapeRenderer.material = state ? onMaterial : offMaterial;
    }
}
