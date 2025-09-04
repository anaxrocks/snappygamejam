using UnityEngine;

public class BugTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.interactionPressed)
        {
            LevelManager.isIntro = false;
            LevelManager.Instance.LoadCutScene();
        }
    }
}
