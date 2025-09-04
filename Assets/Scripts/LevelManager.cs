using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
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
        playerMovement.enabled = false;
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