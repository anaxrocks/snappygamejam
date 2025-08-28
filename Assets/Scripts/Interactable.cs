using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Inventory _inventory;
    bool inRange = false;
    public int consumableAmount = 0;

    void Awake()
    {
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
    }
    void Update()
    {
        if (inRange && InputManager.interactionPressed)
        {
            // Perform interaction here
            _inventory.AddItem(gameObject);
            Debug.Log("Added item");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
