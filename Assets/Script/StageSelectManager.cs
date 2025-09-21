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
            // ユーザーデータを更新して画面も更新
            StartCoroutine(NetworkManager.Instance.UpdateUser(
                "山上智",       // 名前
                2,              // レベル
                2,              // life
                2,              // exp
　　　　　      result =>
           {     // 登録終了後の処理
               if (result == true)
               {
                   // 表示テキストの内容を更新
                   text.text = NetworkManager.Instance.UserName;
               }
               else
               {
                   Debug.Log("ユーザー情報更新が正常に終了しませんでした。");
                   isTouched = false;
               }
           }));
        }

    }
}

