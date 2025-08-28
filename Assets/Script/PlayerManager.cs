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
            if (p == null)
            {
                Debug.LogError("PlayerColorがnullです");
                continue;
            }
            p.Move(input);
        }
    }

    // プレイヤーを分割する
    public void SplitPlayer(PlayerColor origin, string colorToSplit)
    {
        bool r = origin.hasR;
        bool g = origin.hasG;
        bool b = origin.hasB;

        // 分割する色を持っているか判定
        bool splitR = (colorToSplit == "R") && origin.hasR;
        bool splitG = (colorToSplit == "G") && origin.hasG;
        bool splitB = (colorToSplit == "B") && origin.hasB;

        if (!splitR && !splitG && !splitB) return; // 分割できない場合は終了

        // 元のプレイヤーから分割した色を外す
        if (splitR) origin.hasR = false;
        if (splitG) origin.hasG = false;
        if (splitB) origin.hasB = false;
        origin.SetColor(origin.hasR, origin.hasG, origin.hasB);

        // 新しいプレイヤーを生成（分割した色のみ持つ）
        GameObject newPObj = Instantiate(origin.gameObject, origin.transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        PlayerColor newPC = newPObj.GetComponent<PlayerColor>();
        newPC.SetColor(splitR, splitG, splitB);

        AddPlayer(newPC);
    }
}
