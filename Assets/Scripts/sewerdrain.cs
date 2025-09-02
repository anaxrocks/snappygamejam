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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        if (collision.CompareTag("Player"))
        {
            if (_inventory.isSolid)
            {
                _inventory.ThrowItem();
            }
            StartCoroutine(MovePlayer(rb));
        }
    }
        void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_inventory.isSolid)
            {
                _inventory.ThrowItem();
            }
            StartCoroutine(MovePlayer(rb));
        }
    }

    IEnumerator MovePlayer(Rigidbody2D playerRb)
    {
        // disable player controls
        PlayerMovement movement = playerRb.GetComponent<PlayerMovement>();
        _camera.transform.SetParent(_source);
        movement.enabled = false;
        _collider.isTrigger = true;
        _sprite.enabled = false;

        // calc direction to move in
        Vector2 dir = (destination.position - playerRb.transform.position).normalized;

        // move 
        while (Vector2.Distance(playerRb.position, destination.position) > 0.1f)
        {
            rb.linearVelocity = dir * speed; yield return null;
        }

        // stop
        playerRb.linearVelocity = Vector2.zero;
        playerRb.position = destination.position;

        // re-enable controls
        movement.enabled = true;
        _collider.isTrigger = false;
        _camera.transform.parent = null;
        _sprite.enabled = true;
    }
}
