using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelpers : MonoBehaviour
{
    public void Load(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}