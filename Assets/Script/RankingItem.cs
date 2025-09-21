using TMPro;
using UnityEngine;

public class RankingItem : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nameText;

    public void SetData(int rank, string playerName, float clearTime)
    {
        rankText.text = $"{rank}ˆÊ";
        nameText.text = $"{playerName} - {clearTime:F2}•b";
    }
}
