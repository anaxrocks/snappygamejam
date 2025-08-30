using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    public float baseHealth;
    private float currHealth;
    private GameObject target;
    public float speed = 1f;
    public Image slider;
    public Image sliderBG;
    private Animator animator;
    private Collider2D _collider;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float trapDamageInterval = 1f; // seconds between ticks
    private float trapDamageTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = GameObject.FindGameObjectWithTag("Player");
        currHealth = baseHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);
        if (currHealth <= 0)
        {
            _collider.enabled = false;
            sliderBG.enabled = false;
            animator.SetTrigger("Die");
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

    void FixedUpdate()
    {
        // if (target.activeInHierarchy)
        // {
        //     Vector2 direction = ((Vector2)target.transform.position - rb.position).normalized;
        //     Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
        //     rb.MovePosition(newPos);
        // }
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

    private void TakeDamage(float damage)
    {
        currHealth -= damage;
        currHealth = Mathf.Max(currHealth, 0); 
        slider.fillAmount = currHealth / baseHealth;
    }
}
