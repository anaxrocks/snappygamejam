using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Door[] doors;
    private Animator _animator;
    private bool inRange = false;
    private bool isActivated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.interactionPressed && inRange)
        {
            isActivated = !isActivated;
            _animator.SetBool("isActivated", isActivated);
            foreach (Door door in doors) {
                door.ActivateDoor();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
