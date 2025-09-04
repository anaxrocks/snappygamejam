using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Inventory _inventory;
    bool inRange = false;
    private bool hasInteracted = false;
    public int consumableAmount = 0;
    public bool giveHint = false;
    public bool consumeHint = false;
    public bool consumable = true;

    void Start()
    {
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
    }
    void HandleInteraction()
    {
        if (inRange && InputManager.interactionPressed && _inventory.enabled && _inventory != null)
        {
            hasInteracted = true;
            // Perform interaction here
            if (giveHint && _inventory.itemHeld == null && !consumeHint)
            {
                giveHint = false;
                Hints.Instance.TriggerQHint();
            }
            else if (giveHint && _inventory.itemHeld == null && consumeHint)
            {
                giveHint = false;
                Hints.Instance.TriggerShiftHint();
            }
            _inventory.AddItem(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            hasInteracted = false;
            if (giveHint)
            {
                Hints.Instance.TriggerEHint();
            }
            StartCoroutine(CheckForInteraction());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            StopAllCoroutines();
        }
    }
    private IEnumerator CheckForInteraction()
    {
        while (inRange && !hasInteracted)
        {
            if (InputManager.interactionPressed)
            {
                HandleInteraction();
                yield break;
            }
            yield return null;
        }   
    }
}
