using UnityEngine;

public class SignalProxy : MonoBehaviour
{
    public void EndScene()
    {
        LevelManager.Instance.EndCutScene();
    }
}
