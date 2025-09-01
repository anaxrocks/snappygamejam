using System;
using UnityEngine;
using UnityEngine.UI;

public class Fill : MonoBehaviour
{
    private Inventory _inventory;
    private GameObject _player;
    public Image liquid;
    private bool inRange = false;
    private bool isFilled = false;
    public bool startFilled = false;

    // Static reference to track which bottle is currently active
    public static Fill currentActiveBottle = null;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
        if (startFilled)
        {
            fillBottle();
        }
    }

    void Update()
    {
        if (InputManager.interactionPressed)
        {
            Debug.Log("pressed");

            // Only process input if this bottle is in range and no other bottle is active
            if (inRange && currentActiveBottle == null && !isFilled && !_inventory.isSolid)
            {
                // Fill this bottle
                fillBottle();
                SoundManager.Instance.PlaySound2D("Fill");
            }
            // Only allow exit if this specific bottle is the active one
            else if (currentActiveBottle == this && !_player.activeInHierarchy)
            {
                // Empty this bottle
                liquid.fillAmount = 0f;
                isFilled = false;
                _player.SetActive(true);
                currentActiveBottle = null; // Clear the active bottle
                SoundManager.Instance.PlaySound2D("Exit");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            inRange = true;
            Debug.Log($"Player entered range of {gameObject.name}");
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            inRange = false;
            Debug.Log($"Player left range of {gameObject.name}");

            // If player leaves range while this bottle is active, reset everything
            if (currentActiveBottle == this)
            {
                liquid.fillAmount = 0f;
                isFilled = false;
                _player.SetActive(true);
                currentActiveBottle = null;
            }
        }
    }

    void OnDisable()
    {
        if (currentActiveBottle == this)
        {
            currentActiveBottle = null;
            if (_player != null)
            {
                _player.SetActive(true);
            }
        }
    }

    void fillBottle()
    {
        // Fill this bottle
        liquid.fillAmount = 0.65f;
        isFilled = true;
        _player.SetActive(false);
        currentActiveBottle = this; // Mark this bottle as active
    }
}