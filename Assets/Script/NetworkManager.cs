using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    // WebAPIの接続先を設定
#if DEBUG
    // 開発環境で使用する値をセット
    const string API_BASE_URL = "http://localhost:8000/api/";
#else
  // 本番環境で使用する値をセット
  const string API_BASE_URL = "https://…azure.com/api/";
#endif

    //private int userID; // 自分のユーザーID
    private string userName; // 入力される想定の自分のユーザー名
    public string apiToken;// APIトークン

    // プロパティ
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

    // 通信用の関数

    //ユーザー登録処理
    public IEnumerator RegistUser(string name, int level, int life, int exp, Action<bool> result)
    {
        //サーバーに送信するオブジェクトを作成
        RegistUserRequest requestData = new RegistUserRequest();
        requestData.Name = name;
        requestData.Level = level;
        requestData.Exp = exp;
        requestData.Life = life;
        //サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(requestData);
        //送信
        UnityWebRequest request = UnityWebRequest.Post(
                    API_BASE_URL + "users/store", json, "application/json");


        yield return request.SendWebRequest();
        bool isSuccess = false;
        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            //通信が成功した場合、返ってきたJSONをオブジェクトに変換
            string resultJson = request.downloadHandler.text;
            RegistUserResponse response =
                         JsonConvert.DeserializeObject<RegistUserResponse>(resultJson);
            //ファイルにAPIトークンを保存
            this.userName = name;
            this.apiToken = response.APIToken;
            SaveUserData();
            isSuccess = true;
        }
        result?.Invoke(isSuccess); //ここで呼び出し元のresult処理を呼び出す
    }

    //ユーザー情報更新
    public IEnumerator UpdateUser(string name, int level, int exp, int life, Action<bool> result)
    {
        //サーバーに送信するオブジェクトを作成
        UpdateUserRequest requestData = new UpdateUserRequest();
        requestData.Name = name;
        requestData.Level = 1;
        requestData.Exp = 1;
        requestData.Life = 1;
        //サーバーに送信するオブジェクトをJSONに変換
        string json = JsonConvert.SerializeObject(requestData);
        //送信
        UnityWebRequest request = UnityWebRequest.Post(
                    API_BASE_URL + "users/update", json, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + this.apiToken);

        yield return request.SendWebRequest();

        bool isSuccess = false;
        if (request.result == UnityWebRequest.Result.Success
         && request.responseCode == 200)
        {
            // 通信が成功した場合、ファイルに更新したユーザー名を保存
            this.userName = name;
            SaveUserData();
            isSuccess = true;
        }

        result?.Invoke(isSuccess); //ここで呼び出し元のresult処理を呼び出す

    }


    // ユーザー情報を保存する
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

    // ユーザー情報を読み込む
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
