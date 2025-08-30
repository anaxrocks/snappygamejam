using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Grate : MonoBehaviour
{
    public float gravity = 10f;
    private Inventory _inventory;
    private PlayerMovement _movement;
    private Rigidbody2D rb;
    public bool isFalling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        _movement = GameObject.FindAnyObjectByType<PlayerMovement>();
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_inventory.isSolid)
        {
            _inventory.ChangeState();
            _movement.isFalling = true;
            _movement.enabled = false;
            rb.gravityScale = gravity;
            rb.linearVelocity = new Vector2(0, 0);
        }
    }
}
