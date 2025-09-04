using Unity.VisualScripting;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private PlayerMovement _movement;
    private Rigidbody2D rb;
    private Inventory _inventory;
    private Collider2D _collider;

    void Awake() {
        _collider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        _movement = GameObject.FindAnyObjectByType<PlayerMovement>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _movement.isFalling)
        {
            _inventory.ChangeState();
            _collider.isTrigger = false;
            _movement.isFalling = false;
            _movement.enabled = true;
            rb.gravityScale = 0f;
            SoundManager.Instance.PlaySound2D("Land");
        }
    }
}