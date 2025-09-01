using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Grate : MonoBehaviour
{
    public float gravity = 10f;
    private Inventory _inventory;
    private PlayerMovement _movement;
    private Rigidbody2D rb;
    private Collider2D _collider;
    public bool isFalling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        _movement = GameObject.FindAnyObjectByType<PlayerMovement>();
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
        if (isFalling)
        {
            Fall();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_inventory.isSolid && _movement.enabled)
        {
            Fall();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_inventory.isSolid && !isFalling)
        {
            Fall();
        }
    }

    void Fall()
    {
        isFalling = true;
        _inventory.ChangeState();
        _movement.isFalling = true;
        _movement.enabled = false;
        _collider.isTrigger = true;
        rb.gravityScale = gravity;
        rb.linearVelocity = new Vector2(0, 0);
    }
}
