using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject _camera;
    public GameObject _cauldron;
    public static bool isIntro = true;
    private bool playerIsDying = false; // Prevent multiple death calls

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if (InputManager.restartPressed)
        {
            RestartScene();
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }

    public void LoadCutScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Level 5")
        {
            _cauldron = GameObject.Find("Cauldron");
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
            SceneManager.LoadSceneAsync("Cutscene", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadSceneAsync("Cutscene");
        }
    }

    public void EndCutScene()
    {
        SceneManager.LoadSceneAsync("Level 1");
    }

    public void KillPlayer()
    {
        // Prevent multiple calls to KillPlayer
        if (playerIsDying) return;
        playerIsDying = true;
        
        Animator _animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        _animator.SetTrigger("Die");
        PlayerMovement playerMovement = GameObject.FindAnyObjectByType<PlayerMovement>();
        Rigidbody2D rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        playerMovement.enabled = false;
        SpriteRenderer spriteRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1.0f, 0.4f, 0.4f, 1f);
        StartCoroutine(CheckPlayer());
    }

    IEnumerator CheckPlayer()
    {
        // Wait for death animation to finish
        yield return new WaitForSeconds(2f);
        
        // Restart scene once
        RestartScene();
        
        // Reset flag when scene restarts
        playerIsDying = false;
    }
}