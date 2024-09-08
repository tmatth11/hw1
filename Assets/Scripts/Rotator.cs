/*
    Author: Tony Matthews
    Date: 9/7/2024
    Course: CMP SCI 3410-001
    Description:
        This script allows for the PickUp Prefab to move in random directions, bounce off walls, and reduce its speed while a PowerUp timer is active.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float movementSpeed; // Control speed of PickUp
    
    private Vector2 movementDirection; // Control movement direction of PickUp
    private Rigidbody2D rb; // Rigidbody2D component of PickUp
    private PlayerController playerController; // PlayerController object
    private float reducedSpeed; // Speed reducer

    // Start is called before the first frame update
    void Start()
    {
        // Get the PickUp's rigidbody component
        rb = GetComponent<Rigidbody2D>();

        // Set random direction of PickUp
        movementDirection = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)).normalized;
        
        // Locate the Player object
        playerController = GameObject.FindObjectOfType<PlayerController>();

        // Reduce the speed when the PowerUp timer is active
        reducedSpeed = movementSpeed * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the PickUp object in place
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
    }

    // Check if PowerUp timer is running and set appropriate speed
    void FixedUpdate()
    {   
        // Determine current speed based on if the PowerUp timer is running
        if (playerController != null && !playerController.gameOver)
        {
            float currentSpeed = playerController.IsPowerUpActive() ? reducedSpeed : movementSpeed;
            rb.velocity = movementDirection * currentSpeed;
        }
        else
        {
            // No speed is found
            rb.velocity = Vector2.zero;
        }
    }

    // Detect wall collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Allow for PickUp object to bounce off walls upon collision
        if (collision.collider.CompareTag("Wall"))
        {
            Vector2 normal = collision.contacts[0].normal;
            movementDirection = Vector2.Reflect(movementDirection, normal).normalized;
        }
    }
}