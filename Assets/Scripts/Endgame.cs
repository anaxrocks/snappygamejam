using UnityEngine;

public class Endgame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayerMovement _movement;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _movement =  player.GetComponent<PlayerMovement>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _movement.wizard)
        {
            _movement.enabled = false;
            player.SetActive(false);
            LevelManager.Instance.LoadCutScene();
        }
    }
}
