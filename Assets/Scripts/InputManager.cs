using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 movement;
    public static bool attackPressed;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _attackAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
    }

    // Update is called once per frame
    void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();
        attackPressed = _attackAction.WasPressedThisFrame();
    }
}
