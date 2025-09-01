using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    private Collider2D _collider;
    public bool solidDoor = false;
    private Inventory _inventory;
    private Animator _animator = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
        _animator = GetComponent<Animator>();
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player") && !_inventory.isSolid)
    //     {
    //         _collider.isTrigger = true;
    //     }
    // }

    void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") && !_inventory.isSolid && !solidDoor)
            || isOpen)
        {
            _collider.isTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _collider.isTrigger = false;
        }
    }

    public void ActivateDoor()
    {
        isOpen = !isOpen;
        if (_animator)
        {
            _animator.SetBool("isOpen", isOpen);
        }
    }
}
