using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField] Text text;
    private bool isTouched;

    // Start is called before the first frame update
    void Start()
    {
        //text.text = NetworkManager.Instance.UserID + " " +
        //    NetworkManager.Instance.UserName;
        text.text = NetworkManager.Instance.ApiToken + " " +
            NetworkManager.Instance.UserName;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTouched)
        {
            isTouched = true;
            // ���[�U�[�f�[�^���X�V���ĉ�ʂ��X�V
            StartCoroutine(NetworkManager.Instance.UpdateUser(
                "�R��q",       // ���O
                2,              // ���x��
                2,              // life
                2,              // exp
�@�@�@�@�@      result =>
           {     // �o�^�I����̏���
               if (result == true)
               {
                   // �\���e�L�X�g�̓��e���X�V
                   text.text = NetworkManager.Instance.UserName;
               }
               else
               {
                   Debug.Log("���[�U�[���X�V������ɏI�����܂���ł����B");
                   isTouched = false;
               }
           }));
        }

    }
}

