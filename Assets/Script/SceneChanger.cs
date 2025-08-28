using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour // シーン切替用スクリプト
{
    public void ChangeScene(string sceneName)
    {
        // sceneNameに指定したシーンに切り替える
        SceneManager.LoadScene(sceneName);
    }
}
