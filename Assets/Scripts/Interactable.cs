using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Inventory _inventory;
    bool inRange = false;
    public int consumableAmount = 0;
    public bool giveHint = false;
    public bool consumeHint = false;

    void Awake()
    {
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
    }
    void Update()
    {
        if (inRange && InputManager.interactionPressed)
        {
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
            Debug.Log("Added item");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            if (giveHint)
            {
                Hints.Instance.TriggerEHint();
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
