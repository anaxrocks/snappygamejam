using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    private Collider2D _collider;
    public bool solidDoor = false;
    public GameObject key;
    private Inventory _inventory;
    private Animator _animator = null;
    private bool inRange = false;
    private bool hasUnlocked = false; // Prevent multiple unlocks

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
        _animator = GetComponent<Animator>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") && !_inventory.isSolid && !solidDoor)
            || isOpen)
        {
            _collider.isTrigger = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
            
            // Only start checking for key interaction if door needs a key and hasn't been unlocked
            if (key != null && !hasUnlocked && !isOpen)
            {
                StartCoroutine(CheckForKeyInteraction());
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _collider.isTrigger = false;
            inRange = false;
            
            // Stop checking for key interaction when player leaves
            StopAllCoroutines();
        }
    }

    // Coroutine that only runs when player is in range and door needs key
    private IEnumerator CheckForKeyInteraction()
    {
        while (inRange && !hasUnlocked && !isOpen)
        {
            // Check if player has the correct key and is in solid state
            if (InputManager.interactionPressed && 
                _inventory.isSolid && 
                _inventory.itemHeld == key)
            {
                UnlockDoor();
                yield break; // Exit after unlocking
            }
            yield return null; // Wait one frame
        }
    }

    public void ActivateDoor()
    {
        isOpen = !isOpen;
        if (_animator)
        {
            _animator.SetBool("isOpen", isOpen);
        }
        
        // If door opens, stop checking for key interactions
        if (isOpen)
        {
            StopAllCoroutines();
        }
    }

    public void UnlockDoor()
    {
        if (hasUnlocked) return; // Prevent multiple unlocks
        
        hasUnlocked = true;
        _inventory.ChangeState();
        Destroy(_inventory.itemHeld);
        
        ActivateDoor();
        SoundManager.Instance.PlaySound2D("WoodenDoor", 0.5f);
        
        // Stop all coroutines since door is now unlocked
        StopAllCoroutines();
    }
}