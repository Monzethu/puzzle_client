using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン切り替え用
/// </summary>
public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        Debug.Log("Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
