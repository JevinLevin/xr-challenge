using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private float playerSpeed;

    [SerializeField] private float playerGravity;
    [SerializeField] private float playerJumpPower;

    private CharacterController controller;
    private Camera mainCamera;

    private float ySpeed;
    private bool onGround = false;

    private int score;

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
        
        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Apply any vertical motion, like gravity
        ySpeed -= playerGravity * Time.deltaTime;
        velocity.y = ySpeed;
        
        
        controller.Move(velocity * Time.deltaTime);
        
        if(!onGround && controller.isGrounded)
            Land();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Pickup pickupScript))
        {
            CollectPickup(pickupScript);
        }
    }

    private void CollectPickup(Pickup pickupScript)
    {
        // Function returns score to add to player
        int newScore = pickupScript.GetPickedUp();

        // Ignore if already collected
        if (newScore == -1)
            return;
        
        score += newScore;
        UIManager.Instance.ScoreScript.SetScore(score);
            
        //print(score);
    }

    private void Jump()
    {
        // Prevent jumping midair
        if (!onGround) return;

        onGround = false;
        
        ySpeed = playerJumpPower;
        
        //print("jump");
    }

    private void Land()
    {
        onGround = true;

        ySpeed = 0.0f;
        
        //print("land");
    }
}
