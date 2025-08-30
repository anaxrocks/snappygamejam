using Unity.VisualScripting;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private PlayerMovement _movement;
    private Rigidbody2D rb;
    private Inventory _inventory;

    void Awake() {
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        _movement = GameObject.FindAnyObjectByType<PlayerMovement>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _movement.isFalling)
        {
            _inventory.ChangeState();
            _movement.isFalling = false;
            _movement.enabled = true;
            rb.gravityScale = 0f;
        }
    }
}