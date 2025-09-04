using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    private Collider2D _collider;
    public bool solidDoor = false;
    public GameObject key;
    private Inventory _inventory;
    private Animator _animator = null;
    private bool inRange = false;
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
    void Update()
    {
        if (InputManager.interactionPressed && inRange && _inventory.isSolid && _inventory.itemHeld == key)
        {
            unlockDoor();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") && !_inventory.isSolid && !solidDoor)
            || isOpen)
        {
            _collider.isTrigger = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _collider.isTrigger = false;
            inRange = false;
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

    public void unlockDoor()
    {
        _inventory.ChangeState();
        Destroy(_inventory.itemHeld);
        //insert some unlock sound
        //SoundManager.Instance.PlaySound2D()
        ActivateDoor();
        SoundManager.Instance.PlaySound2D("WoodenDoor", 0.5f);
    }
}
