using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Door[] doors;
    public GameObject go = null;
    private bool gameObjectIsActivated = false;
    private Animator _animator;
    private bool inRange = false;
    private bool isActivated = false;
    public bool forDoor = true;
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
            if (forDoor)
            {
                foreach (Door door in doors)
                {
                    door.ActivateDoor();
                }
            }
            if (gameObject)
                {
                    activateObject();
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

    void activateObject()
    {
        if (!go) return;
        if (gameObjectIsActivated)
        {
            gameObjectIsActivated = false;
            go.SetActive(false);
        }
        else
        {
            gameObjectIsActivated = true;
            go.SetActive(true);
        }
    }
}
