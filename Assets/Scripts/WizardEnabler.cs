using UnityEngine;

public class WizardEnabler : MonoBehaviour
{
    private GameObject wizard;
    private Enemy _enemy;
    private EnemyType _enemyType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wizard = GameObject.FindGameObjectWithTag("Enemy");
        _enemy = wizard.GetComponent<Enemy>();
        _enemyType = wizard.GetComponent<EnemyType>();
        _enemy.enabled = false;
        _enemyType.enabled = false;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MusicManager.Instance.PlayMusic("Boss", 0.5f);
            _enemy.enabled = true;
            _enemyType.enabled = true;
            Destroy(gameObject);
        }
    }
}
