using UnityEngine;

public class TrapReset : MonoBehaviour
{
    public float damage;
    private Inventory _inventory;

    void Awake()
    {
        _inventory = FindFirstObjectByType<Inventory>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _inventory.isSolid)
        {
            LevelManager.Instance.RestartScene();
        }
    }
}
