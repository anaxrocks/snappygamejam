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
    private Magic _magic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame

    void Start()
    {
        _movement = GameObject.FindAnyObjectByType<PlayerMovement>();
        _enemy = GetComponent<Enemy>();
        _enemyType = GetComponent<EnemyType>();
        _magic = GameObject.FindAnyObjectByType<Magic>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && isDead && !hasTransferred)
        {
            hasTransferred = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            MusicManager.Instance.PlayMusic("Theme", 0.3f);
            _movement.TransferControlToWizard();
            Destroy(gameObject, 1f);
        }
    }

    public void Dead()
    {
        _enemy.enabled = false;
        _enemyType.enabled = false;
        _magic.enabled = false;
        isDead = true;
    }

}
