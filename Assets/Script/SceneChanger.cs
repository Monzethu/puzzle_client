using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour// �J�ڐ�p�X�N���v�g
{
    public void ChangeScene(string sceneName)
    {
        // sceneName�Ɉړ���̃V�[�����������đJ��
        SceneManager.LoadScene(sceneName);
    }
}
