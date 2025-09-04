using System;
using UnityEngine;
using UnityEngine.UI;

public class Fill : MonoBehaviour
{
    private Inventory _inventory;
    private GameObject _player;
    public GameObject particles;
    private bool inRange = false;
    private bool isFilled = false;
    public bool startFilled = false;
    public bool giveHint = false;

    // Static reference to track which bottle is currently active
    private static Fill currentActiveBottle = null;
    
    // Teleportation settings
    [Header("Teleportation Settings")]
    public float teleportDistance = 1f; // How far from the bottle to teleport
    public LayerMask obstacleLayer = 1; 
    public bool debugMode = true; 
    
    // Store player's last position before hiding
    private Vector3 lastPlayerPosition;

    void Start()
    {
        particles.SetActive(false);
        _player = GameObject.FindGameObjectWithTag("Player");
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
        
        if (startFilled)
        {
            lastPlayerPosition = _player.transform.position;
            fillBottle();
            startFilled = false;
        }
    }

    void FixedUpdate()
    {
        // Only check input when relevant to reduce overhead
        if (InputManager.interactionPressed)
        {
            Debug.Log("pressed");

            // Only process input if this bottle is in range and no other bottle is active
            if (inRange && currentActiveBottle == null && !isFilled && !_inventory.isSolid)
            {
                // Store player's position before hiding
                lastPlayerPosition = _player.transform.position;

                // Fill this bottle
                fillBottle();
                SoundManager.Instance.PlaySound2D("Fill");
            }
            // Only allow exit if this specific bottle is the active one
            else if (currentActiveBottle == this && !_player.activeInHierarchy)
            {
                // Check if player is holding movement input for teleportation
                if (InputManager.isHoldingMovement)
                {
                    Vector3 teleportPosition = GetValidTeleportPosition(InputManager.movement);
                    if (teleportPosition != Vector3.zero)
                    {
                        _player.transform.position = teleportPosition;
                    }
                    else
                    {
                        // If no valid teleport position, spawn at last position
                        _player.transform.position = lastPlayerPosition;
                    }
                }
                else
                {
                    // Not holding movement, spawn at last position
                    _player.transform.position = lastPlayerPosition;
                }

                // Empty this bottle
                particles.SetActive(false);
                isFilled = false;
                _player.SetActive(true);
                currentActiveBottle = null; // Clear the active bottle
                SoundManager.Instance.PlaySound2D("Exit");
            }
        }
    }
    
    private Vector3 GetValidTeleportPosition(Vector2 inputDirection)
    {
        // Normalize the input direction
        Vector2 direction = inputDirection.normalized;
        
        // Calculate the desired teleport position
        Vector3 bottlePosition = transform.position;
        Vector3 desiredPosition = bottlePosition + (Vector3)direction * teleportDistance;
        
        if (debugMode)
        {
            Debug.Log($"Trying to teleport from {bottlePosition} to {desiredPosition} in direction {direction}");
        }
        
        // Check if the position is reachable (no obstacles between bottle and teleport position)
        if (IsPositionReachable(bottlePosition, desiredPosition))
        {
            if (debugMode)
            {
                Debug.Log($"Direct teleport position is valid: {desiredPosition}");
            }
            return desiredPosition;
        }
        
        // If the exact position isn't reachable, try to find a valid position nearby
        Vector3 alternativePosition = FindNearestValidPosition(bottlePosition, direction);
        if (debugMode && alternativePosition != Vector3.zero)
        {
            Debug.Log($"Using alternative position: {alternativePosition}");
        }
        else if (debugMode)
        {
            Debug.Log("No valid teleport position found");
        }
        
        return alternativePosition;
    }
    
    private bool IsPositionReachable(Vector3 from, Vector3 to)
    {
        // Get player's collider size
        Collider2D playerCollider = _player.GetComponent<Collider2D>();
        float playerRadius = 0.5f; // Default fallback
        
        if (playerCollider != null)
        {
            if (playerCollider is CircleCollider2D circleCollider)
            {
                playerRadius = circleCollider.radius * _player.transform.localScale.x;
            }
            else if (playerCollider is CapsuleCollider2D capsuleCollider)
            {
                playerRadius = Mathf.Max(capsuleCollider.size.x, capsuleCollider.size.y) * 0.5f * _player.transform.localScale.x;
            }
            else if (playerCollider is BoxCollider2D boxCollider)
            {
                playerRadius = Mathf.Max(boxCollider.size.x, boxCollider.size.y) * 0.5f * _player.transform.localScale.x;
            }
        }
        
        // Add a small buffer to prevent clipping
        playerRadius += 0.1f;
        
        // Check if there's a clear path
        Vector3 direction = (to - from).normalized;
        float distance = Vector3.Distance(from, to);
        
        RaycastHit2D hit = Physics2D.CircleCast(from, playerRadius, direction, distance, obstacleLayer);
        
        if (hit.collider != null && hit.collider.gameObject != _player && hit.collider.gameObject != this.gameObject)
        {
            Debug.Log($"Path blocked by: {hit.collider.name}");
            return false;
        }
        
        // Check if the final position would overlap with any obstacles
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(to, playerRadius, obstacleLayer);
        foreach (var overlap in overlaps)
        {
            if (overlap.gameObject != _player && overlap.gameObject != this.gameObject)
            {
                Debug.Log($"Final position blocked by: {overlap.name}");
                return false;
            }
        }
        
        return true;
    }
    
    private Vector3 FindNearestValidPosition(Vector3 bottlePosition, Vector2 preferredDirection)
    {
        // Try different angles around the preferred direction
        float[] angleOffsets = { 0f, 15f, -15f, 30f, -30f, 45f, -45f, 60f, -60f };
        
        foreach (float angleOffset in angleOffsets)
        {
            // Calculate rotated direction
            float angle = Mathf.Atan2(preferredDirection.y, preferredDirection.x) + angleOffset * Mathf.Deg2Rad;
            Vector2 rotatedDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            
            // Try different distances
            float[] distances = { teleportDistance, teleportDistance * 0.75f, teleportDistance * 0.5f };
            
            foreach (float distance in distances)
            {
                Vector3 testPosition = bottlePosition + (Vector3)rotatedDirection * distance;
                
                if (IsPositionReachable(bottlePosition, testPosition))
                {
                    return testPosition;
                }
            }
        }
        
        // If no valid position found in the preferred direction, return zero vector
        return Vector3.zero;
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (giveHint)
            {
                Hints.Instance.TriggerEHint();
            }
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
                particles.SetActive(false);
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
        if (giveHint)
        {
            giveHint = false;
            Hints.Instance.TriggerEHint();
        }
        // Fill this bottle
        particles.SetActive(true);
        isFilled = true;
        _player.SetActive(false);
        currentActiveBottle = this; // Mark this bottle as active
    }
}