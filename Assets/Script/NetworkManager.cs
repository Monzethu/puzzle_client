using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    // WebAPI�̐ڑ����ݒ�
#if DEBUG
    // �J�����Ŏg�p����l���Z�b�g
    const string API_BASE_URL = "http://localhost:8000/api/";
#else
  // �{�Ԋ��Ŏg�p����l���Z�b�g
  const string API_BASE_URL = "https://�cazure.com/api/";
#endif

    //private int userID; // �����̃��[�U�[ID
    private string userName; // ���͂����z��̎����̃��[�U�[��
    public string apiToken;// API�g�[�N��

    // �v���p�e�B
    //public int UserID
    //{
    //    get
    //    {
    //        return this.userID;
    //    }
    //}


    public string UserName
    {
        get
        {
            return this.userName;
        }
    }

    public string ApiToken
    {
        get
        {
            return this.apiToken;
        }
    }
    private static NetworkManager instance;
    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObj = new GameObject("NetworkManager");
                instance = gameObj.AddComponent<NetworkManager>();
                DontDestroyOnLoad(gameObj);
            }
            return instance;
        }
    }

    // �ʐM�p�̊֐�

    //���[�U�[�o�^����
    public IEnumerator RegistUser(string name, int level, int life, int exp, Action<bool> result)
    {
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        RegistUserRequest requestData = new RegistUserRequest();
        requestData.Name = name;
        requestData.Level = level;
        requestData.Exp = exp;
        requestData.Life = life;
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(requestData);
        //���M
        UnityWebRequest request = UnityWebRequest.Post(
                    API_BASE_URL + "users/store", json, "application/json");


        yield return request.SendWebRequest();
        bool isSuccess = false;
        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //�ʐM�����������ꍇ�A�Ԃ��Ă���JSON���I�u�W�F�N�g�ɕϊ�
            string resultJson = request.downloadHandler.text;
            RegistUserResponse response =
                         JsonConvert.DeserializeObject<RegistUserResponse>(resultJson);
            //�t�@�C����API�g�[�N����ۑ�
            this.userName = name;
            this.apiToken = response.APIToken;
            SaveUserData();
            isSuccess = true;
        }
        result?.Invoke(isSuccess); //�����ŌĂяo������result�������Ăяo��
    }

    //���[�U�[���X�V
    public IEnumerator UpdateUser(string name, int level, int exp, int life, Action<bool> result)
    {
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g���쐬
        UpdateUserRequest requestData = new UpdateUserRequest();
        requestData.Name = name;
        requestData.Level = 1;
        requestData.Exp = 1;
        requestData.Life = 1;
        //�T�[�o�[�ɑ��M����I�u�W�F�N�g��JSON�ɕϊ�
        string json = JsonConvert.SerializeObject(requestData);
        //���M
        UnityWebRequest request = UnityWebRequest.Post(
                    API_BASE_URL + "users/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + this.apiToken);

        yield return request.SendWebRequest();

        bool isSuccess = false;
        if (request.result == UnityWebRequest.Result.Success
         && request.responseCode == 200)
        {
            // �ʐM�����������ꍇ�A�t�@�C���ɍX�V�������[�U�[����ۑ�
            this.userName = name;
            SaveUserData();
            isSuccess = true;
        }

        result?.Invoke(isSuccess); //�����ŌĂяo������result�������Ăяo��

    }


    // ���[�U�[����ۑ�����
    private void SaveUserData()
    {
        SaveData saveData = new SaveData();
        saveData.UserName = this.userName;
        //saveData.UserID = this.userID;
        saveData.ApiToken = this.apiToken;
        string json = JsonConvert.SerializeObject(saveData);
        var writer =
                new StreamWriter(Application.persistentDataPath + "/saveData.json");
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    // ���[�U�[����ǂݍ���
    public bool LoadUserData()
    {
        if (!File.Exists(Application.persistentDataPath + "/saveData.json"))
        {
            return false;
        }
        var reader =
                   new StreamReader(Application.persistentDataPath + "/saveData.json");
        string json = reader.ReadToEnd();
        reader.Close();
        SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
        //this.userID = saveData.UserID;
        this.userName = saveData.UserName;
        this.apiToken = saveData.ApiToken;
        return true;
    }


}
