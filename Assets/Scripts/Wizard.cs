using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    private PlayerMovement _movement;
    private Enemy _enemy;
    private EnemyType _enemyType;
    public bool isDead = false;
    private bool hasTransferred = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame

    void Start()
    {
        _movement = GameObject.FindAnyObjectByType<PlayerMovement>();
        _enemy = GetComponent<Enemy>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && isDead && !hasTransferred)
        {
            hasTransferred = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _movement.TransferControlToWizard();
            Destroy(gameObject, 1f);
        }
    }

    void Dead()
    {
        _enemy.enabled = false;
        _enemyType.enabled = false;
        isDead = true;
    }

}
