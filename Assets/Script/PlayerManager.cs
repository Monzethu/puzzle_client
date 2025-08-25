using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerColor> players = new List<PlayerColor>();

    public void AddPlayer(PlayerColor pc)
    {
        players.Add(pc);
    }

    public void ClearPlayers()
    {
        players.Clear();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(h, v).normalized;

        foreach (var p in players)
        {
            p.Move(input);
        }
    }

    // プレイヤー分離時に呼ぶ
    public void SplitPlayer(PlayerColor origin, string colorToSplit)
    {
        bool r = origin.hasR;
        bool g = origin.hasG;
        bool b = origin.hasB;

        // 分離色成分だけ取り出し
        bool splitR = (colorToSplit == "R") && origin.hasR;
        bool splitG = (colorToSplit == "G") && origin.hasG;
        bool splitB = (colorToSplit == "B") && origin.hasB;

        if (!splitR && !splitG && !splitB) return; // 持ってなければ無視

        // 元のプレイヤーから分離色を削る
        if (splitR) origin.hasR = false;
        if (splitG) origin.hasG = false;
        if (splitB) origin.hasB = false;
        origin.SetColor(origin.hasR, origin.hasG, origin.hasB);

        // 新しいプレイヤー生成（分離色だけ持つ）
        GameObject newPObj = Instantiate(origin.gameObject, origin.transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        PlayerColor newPC = newPObj.GetComponent<PlayerColor>();
        newPC.SetColor(splitR, splitG, splitB);

        AddPlayer(newPC);
    }
}
