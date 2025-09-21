using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private Transform rankingParent;      // Content
    [SerializeField] private GameObject rankingItemPrefab; // �v���n�u
    [SerializeField] private Button[] stageButtons;        // �X�e�[�W�ؑփ{�^��

    private void Start()
    {
        // �{�^���ɃC�x���g��ǉ�
        for (int i = 0; i < stageButtons.Length; i++)
        {
            int stage = i + 1; // �X�e�[�W�ԍ��i1����n�܂�z��j
            stageButtons[i].onClick.AddListener(() => LoadRanking(stage));
        }

        // �f�t�H���g�ŃX�e�[�W1��ǂݍ���
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

                // �Â����X�g���폜
                foreach (Transform child in rankingParent)
                {
                    Destroy(child.gameObject);
                }

                // �V�������X�g�𐶐�
                for (int i = 0; i < ranking.ranking.Length; i++)
                {
                    GameObject item = Instantiate(rankingItemPrefab, rankingParent);
                    var texts = item.GetComponentsInChildren<TMP_Text>();
                    texts[0].text = $"{i + 1}��";
                    texts[1].text = $"{ranking.ranking[i].name} - {ranking.ranking[i].clear_time:F2}�b";
                }
            }
            else
            {
                Debug.Log("�����L���O�擾���s: " + request.error);
            }
        }
    }
}

// JSON�N���X
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

