using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _LastHorizontal = "LastHorizontal";
    private const string _LastVertical = "LastVertical";
    public bool isFalling = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement.Set(InputManager.movement.x, InputManager.movement.y);
        _rb.linearVelocity = _movement * _moveSpeed;
        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(_LastHorizontal, _movement.x);
            _animator.SetFloat(_LastVertical, _movement.y);
        }
    }
}
