using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem.Editor;

public class Enemy : MonoBehaviour
{
    public float baseHealth;
    private float currHealth;
    public Image slider;
    public Image sliderBG;
    private NavMeshAgent agent;
    private Animator animator;
    private Collider2D _collider;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float trapDamageInterval = 1f; // seconds between ticks
    private float trapDamageTimer = 0f;
    public GameObject key = null;
    public bool dropKey = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currHealth = baseHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currHealth <= 0)
        {
            _collider.enabled = false;
            sliderBG.enabled = false;
            agent.enabled = false;
            animator.SetTrigger("Die");
        }
        // Flip the sprite based on movement direction
        if (agent.velocity.x < 0)
        {
            // Moving left
            spriteRenderer.flipX = false;
        }
        else if (agent.velocity.x > 0)
        {
            // Moving right
            spriteRenderer.flipX = true;
        }
        if (!spriteRenderer.enabled)
        {
            Destroy(gameObject);
        }
        if (trapDamageTimer > 0f)
        {
            trapDamageTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            int damage = projectile._damage;
            TakeDamage(damage);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Trap") && trapDamageTimer <= 0f)
        {
            Trap _trap = other.GetComponent<Trap>();
            float damage = _trap.damage;
            TakeDamage(damage);

            trapDamageTimer = trapDamageInterval;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            animator.SetTrigger("Attack");
            //LevelManager.Instance.RestartScene();
        }
    }

    private void TakeDamage(float damage)
    {
        currHealth -= damage;
        currHealth = Mathf.Max(currHealth, 0);
        slider.fillAmount = currHealth / baseHealth;
        animator.SetTrigger("Damage");
    }

    void Die()
    {
        if (dropKey)
        {
            key.transform.position = transform.position;
            key.SetActive(true);
        }
        Destroy(gameObject, 0.1f); 
    }
}
