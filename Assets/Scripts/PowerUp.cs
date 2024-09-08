/*
    Author: Tony Matthews
    Date: 9/7/2024
    Course: CMP SCI 3410-001
    Description:
        This script allows for the PowerUp object to flash between a light and dark green color every half-second.
        The flashing effect allows for the Player to understand that they can pick it up.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Color lightGreen = new Color(0.6f, 1.0f, 0.6f); // Light green color
    public Color darkGreen = new Color(0.0f, 0.5f, 0.0f);  // Dark green color

    private Renderer powerUpRenderer; // Renderer component of PowerUp object
    private float flashInterval = 0.5f; // Interval of PowerUp color change
    private float timeSinceLastFlash; // Time spent since last flash
    private bool isLightGreen = true; // Determine if the PowerUp color is light green

    // Start is called before the first frame update
    void Start()
    {
        // Get the Renderer Component of the PowerUp object
        powerUpRenderer = GetComponent<Renderer>();
        // Set its color to light green
        powerUpRenderer.material.color = lightGreen;
        // PowerUp has not flashed colors yet
        timeSinceLastFlash = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Detect if it is time for the object to alternate colors
        timeSinceLastFlash += Time.deltaTime;

        // Object is able to alernate colors
        if (timeSinceLastFlash >= flashInterval)
        {
            // Color change is ready to happen
            timeSinceLastFlash = 0f;

            // Change color to dark green
            if (isLightGreen)
            {
                powerUpRenderer.material.color = darkGreen;
            }
            // Change color to light green
            else
            {
                powerUpRenderer.material.color = lightGreen;
            }

            // Object is no longer light green
            isLightGreen = !isLightGreen;
        }
    }
}
