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
    public RuntimeAnimatorController wizardController;
    public bool wizard = false;
    private Magic magicScript;
    private SpriteRenderer _spriteRenderer;
    private Camera _camera;
    [SerializeField]
    private float _screenBorder = 50f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        magicScript = GameObject.FindAnyObjectByType<Magic>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (magicScript != null && magicScript._hit == true)
        {
            _movement.Set(-InputManager.movement.x, -InputManager.movement.y);
        }
        else
        {
            _movement.Set(InputManager.movement.x, InputManager.movement.y);
        }

        _rb.linearVelocity = _movement * _moveSpeed;
        PreventPlayerGoingOffScreen();
        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(_LastHorizontal, _movement.x);
            _animator.SetFloat(_LastVertical, _movement.y);
        }
        HandleWizardAnimation();
    }

     private void PreventPlayerGoingOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < _screenBorder && _rb.linearVelocity.x < 0) ||
            (screenPosition.x > _camera.pixelWidth - _screenBorder && _rb.linearVelocity.x > 0))
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }

        if ((screenPosition.y < _screenBorder && _rb.linearVelocity.y < 0) ||
            (screenPosition.y > _camera.pixelHeight - _screenBorder && _rb.linearVelocity.y > 0))
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);
        }
    }

    public void TransferControlToWizard()
    {
        wizard = true;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = wizardController;
        gameObject.transform.localScale = Vector2.one;
        gameObject.GetComponent<Inventory>().enabled = false;
        GameObject _camera = GameObject.FindGameObjectWithTag("MainCamera");
        _camera.transform.SetParent(gameObject.transform);
    }
    
    private void HandleWizardAnimation()
    {
        if (wizard)
        {
            // Set wizard-specific velocity parameters for animation
            _animator.SetFloat("velX", _rb.linearVelocity.x);
            _animator.SetFloat("velY", _rb.linearVelocity.y);
            
            // Handle wizard sprite flipping
            if (_rb.linearVelocity.x < 0)
            {
                // Moving left - flip sprite for wizard
                _spriteRenderer.flipX = wizard;
            }
            else if (_rb.linearVelocity.x > 0)
            {
                // Moving right - don't flip sprite for wizard
                _spriteRenderer.flipX = !wizard;
            }
        }
}
}
