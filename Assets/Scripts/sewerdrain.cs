using UnityEngine;
using System.Collections;

public class sewerdrain : MonoBehaviour
{
    private Inventory _inventory;
    private PlayerMovement _movement;
    private SpriteRenderer _sprite;
    private Rigidbody2D rb;
    private Collider2D _collider;
    public Transform destination;
    private Transform _source;
    public float speed = 5f;
    private GameObject _camera;
    
    // Add this flag to prevent multiple coroutines
    private bool isMovingPlayer = false;

    void Start()
    {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        _source = _player.GetComponent<Transform>();
        _collider = _player.GetComponent<Collider2D>();
        rb = _player.GetComponent<Rigidbody2D>();
        _sprite = _player.GetComponent<SpriteRenderer>();
        _movement = GameObject.FindAnyObjectByType<PlayerMovement>();
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_movement.isFalling && !isMovingPlayer)
        {
            if (_inventory.isSolid)
            {
                _inventory.ThrowItem();
            }
            StartCoroutine(MovePlayer(rb));
        }
    }

    // Remove OnTriggerStay2D entirely - it's not needed and causes the problem
    // If you need it for some reason, add the same !isMovingPlayer check

    IEnumerator MovePlayer(Rigidbody2D playerRb)
    {
        // Set flag to prevent multiple coroutines
        isMovingPlayer = true;
        
        // disable player controls
        PlayerMovement movement = playerRb.GetComponent<PlayerMovement>();
        _camera.transform.SetParent(_source);
        movement.enabled = false;
        _collider.isTrigger = true;
        _sprite.enabled = false;

        // calc direction to move in
        Vector2 dir = (destination.position - playerRb.transform.position).normalized;

        // move with a more precise stopping condition
        while (Vector2.Distance(playerRb.position, destination.position) > 0.05f)
        {
            rb.linearVelocity = dir * speed;
            yield return null;
        }

        // stop
        playerRb.linearVelocity = Vector2.zero;
        playerRb.position = destination.position;

        // re-enable controls
        movement.enabled = true;
        _collider.isTrigger = false;
        _camera.transform.parent = null;
        _sprite.enabled = true;
        
        // Reset flag when movement is complete
        isMovingPlayer = false;
    }
}