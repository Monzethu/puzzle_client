using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private Transform rankingParent;      // Content
    [SerializeField] private GameObject rankingItemPrefab; // プレハブ
    [SerializeField] private Button[] stageButtons;        // ステージ切替ボタン

    private void Start()
    {
        // ボタンにイベントを追加
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int stage = i + 1; // ステージ番号（1から始まる想定）
            stageButtons[i].onClick.AddListener(() => LoadRanking(stage));
        }

        // デフォルトでステージ1を読み込み
        LoadRanking(1);
    }

    public void LoadRanking(int stage)
    {
        StartCoroutine(GetRanking(stage));
    }

    IEnumerator GetRanking(int stage)
    {
        string url = $"http://localhost/ranking/{stage}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                RankingWrapper ranking = JsonUtility.FromJson<RankingWrapper>(json);

                // 古いリストを削除
                foreach (Transform child in rankingParent)
                {
                    Destroy(child.gameObject);
                }

                // 新しいリストを生成
                for (int i = 0; i < ranking.ranking.Length; i++)
                {
                    GameObject item = Instantiate(rankingItemPrefab, rankingParent);
                    var texts = item.GetComponentsInChildren<TMP_Text>();
                    texts[0].text = $"{i + 1}位";
                    texts[1].text = $"{ranking.ranking[i].name} - {ranking.ranking[i].clear_time:F2}秒";
                }
            }
            else
            {
                Debug.Log("ランキング取得失敗: " + request.error);
            }
        }
    }
}

// JSONクラス
[System.Serializable]
public class RankingResponse
{
    public int id;
    public string name;
    public float clear_time;
}

[System.Serializable]
public class RankingWrapper
{
    public RankingResponse[] ranking;
}

