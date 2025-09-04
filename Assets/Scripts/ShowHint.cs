using UnityEngine;

public class ShowHint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Hints.Instance.TriggerSpaceHint();
            Destroy(gameObject);
        }
    }
}
