using JetBrains.Annotations;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // if player is able to pick up an item or not
    public bool isSolid = false;
    public GameObject itemHeld;
    private SpriteRenderer spriteRenderer;
    private PlayerCombat playerCombat;
    private GameObject _player;
    private GameObject _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        playerCombat = GameObject.FindAnyObjectByType<PlayerCombat>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.throwPressed)
        {
            if (isSolid)
            {
                ThrowItem();
            }
        }
        if (InputManager.consumePressed)
        {
            if (isSolid)
            {
                ConsumeItem();
            }
        }
    }

    public void AddItem(GameObject item)
    {
        if (!isSolid)
        {
            isSolid = true;
            itemHeld = item;
            item.transform.SetParent(gameObject.transform);
            item.transform.localPosition = new Vector3(0, 0, 0);
            Color currentAlpha = spriteRenderer.color;
            spriteRenderer.color = new Color(currentAlpha.r, currentAlpha.g, currentAlpha.b, 0.7f);
            _camera.transform.SetParent(_player.transform);
            SoundManager.Instance.PlaySound2D("Pickup");
        }
    }

    public void ConsumeItem()
    {
        if (isSolid)
        {
            isSolid = false;
            Color currentAlpha = spriteRenderer.color;
            spriteRenderer.color = new Color(currentAlpha.r, currentAlpha.g, currentAlpha.b, 1f);
            int amount = itemHeld.GetComponent<Interactable>().consumableAmount;
            playerCombat._currentAmmo += amount;
            Debug.Log("amount: " + amount + "ammo: " + playerCombat._currentAmmo);
            Destroy(itemHeld);
            _camera.transform.parent = null;
            SoundManager.Instance.PlaySound2D("Consume");
        }
    }

    public void ThrowItem()
    {
        if (isSolid)
        {
            isSolid = false;
            Color currentAlpha = spriteRenderer.color;
            spriteRenderer.color = new Color(currentAlpha.r, currentAlpha.g, currentAlpha.b, 1f);
            itemHeld.transform.parent = null;
            _camera.transform.parent = null;
            itemHeld = null;
            SoundManager.Instance.PlaySound2D("Drop");
        }
    }
}
