using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint; // Empty GameObject positioned where projectiles spawn
    
    [Header("Combat Stats")]
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private int _maxCapacity = 10;

    [Header("Current Stats")]
    [SerializeField] public int _currentAmmo = 0;
    [SerializeField] private bool _canAttack = true;
    
    private Vector2 _lastMovementDirection = Vector2.down; // Default facing direction
    private PlayerMovement _playerMovement;
    private Inventory _Inventory;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _Inventory = GameObject.FindAnyObjectByType<Inventory>();
        //_currentAmmo = _maxCapacity;
    }

    void Update()
    {
        HandleAttackInput();
        UpdateFacingDirection();
    }

    private void HandleAttackInput()
    {
        if (InputManager.attackPressed && _canAttack && _currentAmmo > 0 && !_Inventory.isSolid)
        {
            Attack();
        }
    }

    private void UpdateFacingDirection()
    {
        // Update facing direction based on movement
        if (InputManager.movement != Vector2.zero)
        {
            _lastMovementDirection = InputManager.movement.normalized;
        }
    }

    private void Attack()
    {
        // Spawn projectile
        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        
        if (projectileScript != null)
        {
            projectileScript.SetDirection(_lastMovementDirection);
        }

        // Consume ammo
        _currentAmmo--;

        SoundManager.Instance.PlaySound2D("Shoot");
        
        // Start cooldown
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    // Public getters for UI
    public int CurrentAmmo => _currentAmmo;
    public int MaxCapacity => _maxCapacity;
    public bool CanAttack => _canAttack;
}