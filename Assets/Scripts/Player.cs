using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private float playerSpeed;

    [SerializeField] private float playerGravity;

    private CharacterController controller;
    private Camera mainCamera;

    private float ySpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {

        // Get players input direction
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;
        
        // Rotate based on camera direction
        Vector3 moveDirection = mainCamera.transform.TransformDirection(inputDirection);
        moveDirection.y = 0;
        
        Vector3 velocity = moveDirection * playerSpeed;

        // Apply any vertical motion, like gravity
        ySpeed -= playerGravity * Time.deltaTime;
        velocity.y = ySpeed;

        controller.Move(velocity * Time.deltaTime);

        // Reset vertical motion if falling and on ground
        if (ySpeed < 0 && controller.isGrounded)
            ySpeed = 0;
    }
}
