using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    private Collider2D _collider;
    private Inventory _inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player") && !_inventory.isSolid)
    //     {
    //         _collider.isTrigger = true;
    //     }
    // }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_inventory.isSolid)
        {
            _collider.isTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _collider.isTrigger = false;
        }
    }
}
