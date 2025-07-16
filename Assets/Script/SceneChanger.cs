using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour// 遷移専用スクリプト
{
    public void ChangeScene(string sceneName)
    {
        // sceneNameに移動先のシーン名を書いて遷移
        SceneManager.LoadScene(sceneName);
    }
}
