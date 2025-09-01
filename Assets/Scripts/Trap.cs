using UnityEngine;

public class Trap : MonoBehaviour
{
    private Animator animator;
    public float damage;
    private Inventory _inventory;

    void Awake()
    {
        _inventory = FindFirstObjectByType<Inventory>();
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            animator.SetBool("inRange", true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            animator.SetBool("inRange", false);
        }
    }
}
