using UnityEngine.SceneManagement;
using UnityEngine;

public class DisconnectedManager : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
