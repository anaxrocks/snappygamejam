using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Door[] doors;
    public GameObject go = null;
    private bool gameObjectIsActivated = false;
    private Animator _animator;
    private bool inRange = false;
    private bool isActivated = false; // Once this is true, lever can never be used again
    
    public bool forDoor = true;
    public bool giveHint = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isActivated) // Only enter if not already activated
        {
            if (giveHint)
            {
                giveHint = false;
                Hints.Instance.TriggerEHint();
            }
            inRange = true;
            
            // Start checking for input only when player is in range and lever hasn't been used
            StartCoroutine(CheckForInteraction());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            inRange = false;
            StopAllCoroutines(); // Stop checking for input when player leaves
        }
    }

    // Coroutine that only runs when player is in range and lever hasn't been activated
    private IEnumerator CheckForInteraction()
    {
        while (inRange && !isActivated)
        {
            if (InputManager.interactionPressed)
            {
                ActivateLever();
                yield break; // Exit coroutine after activation (lever is now permanently activated)
            }
            yield return null; // Wait one frame
        }
    }

    private void ActivateLever()
    {
        // This can only run once due to the !isActivated checks
        isActivated = true;
        _animator.SetBool("isActivated", isActivated);
        SoundManager.Instance.PlaySound2D("Lever");
        
        // Activate doors
        if (forDoor)
        {
            foreach (Door door in doors)
            {
                door.ActivateDoor();
            }
        }
        
        // Activate/deactivate GameObject
        if (go != null)
        {
            ActivateObject();
        }
        
        // Stop all coroutines since lever is now permanently activated
        StopAllCoroutines();
    }

    private void ActivateObject()
    {
        if (!go) return;
        
        if (gameObjectIsActivated)
        {
            SoundManager.Instance.PlaySound2D("LightTorches");
            gameObjectIsActivated = false;
            go.SetActive(false);
        }
        else
        {
            gameObjectIsActivated = true;
            go.SetActive(true);
        }
    }
}