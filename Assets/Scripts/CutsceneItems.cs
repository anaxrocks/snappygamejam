using UnityEngine;

public class CutsceneItems : MonoBehaviour
{
    public GameObject items;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // second time+, killed wizard
        if (!LevelManager.isIntro)
        {
            items.SetActive(false);
            LevelManager.Instance._camera.SetActive(false);
            LevelManager.Instance._cauldron.SetActive(false);
        }
        // first time
        else
        {
            LevelManager.isIntro = false;
        }
    }
}
