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
    [SerializeField] private float _reloadTime = 2f;
    
    [Header("Current Stats")]
    [SerializeField] private int _currentAmmo;
    [SerializeField] private bool _canAttack = true;
    [SerializeField] private bool _isReloading = false;
    
    private Vector2 _lastMovementDirection = Vector2.down; // Default facing direction
    private PlayerMovement _playerMovement;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _currentAmmo = _maxCapacity;
    }

    void Update()
    {
        HandleAttackInput();
        UpdateFacingDirection();
    }

    private void HandleAttackInput()
    {
        if (InputManager.attackPressed && _canAttack && !_isReloading && _currentAmmo > 0)
        {
            Attack();
        }
        
        // Auto-reload when out of ammo
        if (_currentAmmo <= 0 && !_isReloading)
        {
            StartReload();
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
        
        // Start cooldown
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    public void StartReload()
    {
        if (!_isReloading && _currentAmmo < _maxCapacity)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        _isReloading = true;
        yield return new WaitForSeconds(_reloadTime);
        _currentAmmo = _maxCapacity;
        _isReloading = false;
    }

    // Public getters for UI
    public int CurrentAmmo => _currentAmmo;
    public int MaxCapacity => _maxCapacity;
    public bool IsReloading => _isReloading;
    public bool CanAttack => _canAttack;
}