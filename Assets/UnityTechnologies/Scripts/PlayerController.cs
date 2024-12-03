using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float maxStamina = 100f;
    public float staminaRechargeRate = 5f;
    public float staminaDepletionRate = 10f;
    public Slider staminaSlider;
    public TextMeshProUGUI scoreText; // Reference to the ScoreText UI element
    public AudioSource scoreSound; // Reference to the AudioSource component

    private float currentStamina;
    private bool isSprinting;
    private int score; // Player's score

    void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
        staminaSlider.gameObject.SetActive(false); // Hide the slider initially
        score = 0; // Initialize score
        UpdateScoreText(); // Update the score UI
    }

    void Update()
    {
        HandleMovement();
        HandleStamina();
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    void HandleStamina()
    {
        if (Input.GetKey(KeyCode.Space) && currentStamina > 0)
        {
            isSprinting = true;
            currentStamina -= staminaDepletionRate * Time.deltaTime;
            staminaSlider.gameObject.SetActive(true); // Show the slider when sprinting
        }
        else
        {
            isSprinting = false;
            currentStamina += staminaRechargeRate * Time.deltaTime;
            if (currentStamina >= maxStamina)
            {
                staminaSlider.gameObject.SetActive(false); // Hide the slider when fully recharged
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaSlider.value = currentStamina;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.name); // Add this line

        if (other.CompareTag("Enemy"))
        {
            // Increment score when passing an enemy
            score += 10; // Adjust points as needed
            UpdateScoreText();
            Debug.Log("Score updated: " + score); // Add this line

            // Play the score sound effect
            if (scoreSound != null)
            {
                scoreSound.Play();
            }
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
