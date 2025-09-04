using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PressurePlate : MonoBehaviour
{
    public List<Door> doors;
    public bool isActivated = false;
    private Animator _animator;
    public List<GameObject> items;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item") || collision.CompareTag("Player"))
        {
            if (!items.Contains(collision.gameObject))
            {
                items.Add(collision.gameObject);
            }
            if (!isActivated)
            {
                isActivated = true;
                _animator.SetBool("isActivated", isActivated);
                foreach (Door door in doors)
                {
                    door.ActivateDoor();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item") || collision.CompareTag("Player"))
        {
            items.Remove(collision.gameObject);
            
            if (items.Count == 0)
            {
                isActivated = false;
                _animator.SetBool("isActivated", isActivated);
                
                foreach (Door door in doors)
                {
                    door.ActivateDoor();
                }
            }   
        }
    }
}
