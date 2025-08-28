using UnityEngine;
using UnityEngine.UI;

public class Fill : MonoBehaviour
{
    private Inventory _inventory;
    private GameObject _player;
    public Image liquid;
    private bool inRange = false;
    private bool isFilled = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _inventory = GameObject.FindAnyObjectByType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.interactionPressed)
        {
            Debug.Log("pressed");
            if (inRange && !isFilled && !_inventory.isSolid)
            {
                liquid.fillAmount = 0.65f;
                isFilled = true;
                _player.SetActive(false);
            }
            else
            {
                liquid.fillAmount = 0f;
                isFilled = false;
                _player.SetActive(true);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            inRange = true;
            Debug.Log("player enter");
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
