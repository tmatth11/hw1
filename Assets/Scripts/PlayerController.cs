/*
    Author: Tony Matthews
    Date: 9/7/2024
    Course: CMP SCI 3410-001
    Description:
        This script allows for the PlayerController GameObject to move around, get hit by the PickUp GameObject, and collect a PowerUp Gameobject to slow down the PickUps for 10 seconds.
        It also sets the UI elements such as WinText, TimeText, SlowText, and the restart Button.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed; // Speed of Player
    public float timeLeft; // Time remaining until game is won
    public float powerUpDuration; // Duration of PowerUp activation
    public Text timeText; // Display time remaining until game is won
    public Text winText; // Win or lose message
    public Text slowText; // Display time remaining of slower PickUp speed
    public Button restartButton; // Button to restart game
    public bool gameOver; // Game over flag
    
    private Rigidbody2D rb2d; // Rigidbody2D component of Player
    private float powerUpTimeLeft; // Time left until PowerUp is no longer active
    private bool isPowerUpActive = false; // Determine if the PowerUp effect is active

    // Start is called before the first frame update
    void Start()
    {
        // Get the Player's Rigidbody2D component
        rb2d = GetComponent<Rigidbody2D>();
        // Set the game over flag to false
        gameOver = false;
        
        // Set initial states of UI elements
        timeText.text = "Time: " + timeLeft.ToString("F0");
        winText.text = "";
        slowText.text = "";
        restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If the game timer is still running and the game isn't over...
        if (timeLeft > 0 && !gameOver)
        {
            // Subtract the time by 1 second and update time display
            timeLeft -= Time.deltaTime;
            timeText.text = "Time: " + Mathf.Ceil(timeLeft).ToString();
        }
        // Else if the reaches zero...
        else if (!gameOver)
        {
            // There is no time left
            timeLeft = 0;
            // The game is over
            gameOver = true;
            // The Player is prevented from moving 
            rb2d.velocity = Vector2.zero;

            // Update UI to tell the Player that they won and allow them to restart
            winText.text = "You Win!";
            slowText.text = "";
            winText.color = Color.green;
            restartButton.gameObject.SetActive(true);
        }

        // Update PowerUp timer
        if (isPowerUpActive && !gameOver)
        {
            // Count down the time it takes until the PowerUp's slowness effect is no longer active
            powerUpTimeLeft -= Time.deltaTime;
            slowText.text = "Slow: " + Mathf.Ceil(powerUpTimeLeft).ToString();

            // Deactivate the slowness effect of the PowerUp if the time is up
            if (powerUpTimeLeft <= 0)
            {
                DeactivatePowerUp();
            }
        }
    }

    // Get the movement of the Player while the game is not over
    void FixedUpdate()
    {
        if (!gameOver)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            rb2d.velocity = movement * speed;
        }
    }

    // End the game if the Player collides with a PickUp
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PickUp"))
        {
            // End game
            gameOver = true;
            rb2d.velocity = Vector2.zero;

            // Update UI elements
            winText.text = "Game Over!";
            slowText.text = "";
            winText.color = Color.red;
            restartButton.gameObject.SetActive(true);
        }
    }

    // Activate the PowerUp effect and destroy the object upon collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            ActivatePowerUp();
            Destroy(other.gameObject);
        }
    }

    // Activate the PowerUp effect
    private void ActivatePowerUp()
    {
        isPowerUpActive = true;
        powerUpTimeLeft = powerUpDuration;
        slowText.gameObject.SetActive(true);
    }

    // Deactivate the PowerUp effect
    private void DeactivatePowerUp()
    {
        isPowerUpActive = false;
        slowText.text = "";
        slowText.gameObject.SetActive(false);
    }

    // Reset the scene
    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Determine if the PowerUp effect is still active
    public bool IsPowerUpActive()
    {
        return isPowerUpActive;
    }
}