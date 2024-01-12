using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    
    [Header("Config")] 
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerGravity;
    [SerializeField] private float playerJumpPower;
    [SerializeField] private float playerJumpBuffer = 0.2f;
    [SerializeField] private float playerCoyoteTime = 0.2f;

    [Header("Debug")] 
    [SerializeField] private bool immune;

    private CharacterController controller;
    private Camera mainCamera;
    
    private float ySpeed;
    private bool onGround;
    
    private float jumpBuffer;
    private float coyoteTime;
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        GameManager.Instance.Player = this;

    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Dont run update if game is inactive
        if (!GameManager.Instance.IsGameActive) return;

        Vector3 moveDirection = GetMovementDirection();

        // Get mouse direction in 3d worldspace
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit);
        Vector3 mouseDirection = hit.point - transform.position;
        mouseDirection.y = 0;
        // Rotate towards mouse position smoothly
        Quaternion rotationQuaternion = Quaternion.LookRotation(mouseDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationQuaternion, Time.deltaTime * 16);
        
        Vector3 velocity = moveDirection * playerSpeed;
        
        // Jump logic
        // Buffer
        jumpBuffer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBuffer = playerJumpBuffer;
        
        // Coyote Time
        if (onGround)
            coyoteTime = playerCoyoteTime;
        else
            coyoteTime -= Time.deltaTime;

        if (jumpBuffer > 0.0f && coyoteTime>0)
            Jump();

        // Apply gravity
        if(!onGround)
            ySpeed -= playerGravity * Time.deltaTime;
        
        velocity.y = ySpeed;
        
        controller.Move(velocity * Time.deltaTime);

        // Ground interaction
        if (onGround && !controller.isGrounded)
            Fall();
        if(!onGround && controller.isGrounded)
            Land();
        
        
        // Falling logic
        // No longer really needed since there is no place to fall, but good for if the player glitches out of bounds i guess
        if (transform.position.y < -1 && GameManager.Instance.IsGameActive)
            Die();
        
    }

    /// <summary>
    /// Creates vector based on movement input
    /// </summary>
    /// <returns> A vector based on input and camera direction for the player to move </returns>
    private Vector3 GetMovementDirection()
    {
        // Get players input direction
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;
        
        // Rotate based on camera direction
        Vector3 moveDirection = mainCamera.transform.TransformDirection(inputDirection);
        moveDirection.y = 0;

        return moveDirection.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Pickup pickupScript))
        {
            CollectPickup(pickupScript);
        }
        
        if (GameManager.Instance.IsGameActive && GameManager.Instance.IsEscaping && other.CompareTag("Escape"))
            Escape();
    }

    /// <summary>
    /// Collects pickup when player walks into it
    /// </summary>
    private void CollectPickup(Pickup pickupScript)
    {
        // Function returns score to add to player
        int newScore = pickupScript.GetPickedUp();

        // Ignore if already collected
        if (newScore == -1)
            return;
        
        GameManager.Instance.GiveScore(newScore);
    }
    

    /// <summary>
    /// Ran if the player falls off a platform without jumping
    /// </summary>
    private void Fall()
    {
        onGround = false;
    }

    /// <summary>
    /// Ran when the player jumps
    /// </summary>
    private void Jump()
    {

        onGround = false;
        coyoteTime = 0.0f;
        
        ySpeed = playerJumpPower;
    }

    /// <summary>
    /// Ran whenever the player lands on a platform
    /// </summary>
    private void Land()
    {
        onGround = true;

        // Setting to a small negative values prevent issues with floating midair and other character controller quirks
        ySpeed = -0.5f;
        
        //print("land");
    }

    /// <summary>
    /// Ran if the player died in any way
    /// </summary>
    public void Die()
    {
        // Stops player from dying if debugging with immune bool
        if (immune) return;
        
        // Stop this from being ran multiple times
        if (!GameManager.Instance.IsGameActive) return;
        
        GameManager.Instance.EndGame();
        
        SceneTransitioner.Instance.ReloadCurrentScene("You Died");
    }

    /// <summary>
    /// Ran once the player reaches the escape point
    /// </summary>
    private void Escape()
    {
        GameManager.Instance.WinGame();
    }
    
}
