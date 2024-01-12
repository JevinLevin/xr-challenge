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
    private Material platformMaterial;

    private void Awake()
    {
        platformMaterial = GetComponent<Material>();
        
        SetMaterial(false);
    }

    private void Start()
    {
        GameManager.OnEscapeStart += SetMaterial;
    }

    private void SetMaterial(bool state)
    {
        platformMaterial = state ? onMaterial : offMaterial;
    }
}
