using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 movement;
    public static bool attackPressed;
    public static bool interactionPressed;
    public static bool consumePressed;
    public static bool throwPressed;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _interactAction;
    private InputAction _consumeAction;
    private InputAction _throwAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
        _interactAction = _playerInput.actions["Interact"];
        _consumeAction = _playerInput.actions["Consume"];
        _throwAction = _playerInput.actions["Throw"];
    }

    // Update is called once per frame
    void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();
        attackPressed = _attackAction.WasPressedThisFrame();
        interactionPressed = _interactAction.WasPressedThisFrame();
        consumePressed = _consumeAction.WasPressedThisFrame();
        throwPressed = _throwAction.WasPressedThisFrame();
    }
}
