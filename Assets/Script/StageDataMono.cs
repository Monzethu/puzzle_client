using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataMono : MonoBehaviour
{
    [System.Serializable]
    public class WallInfo
    {
        public Vector3Int position;
        public string color; // "R","G","B"
    }

    [System.Serializable]
    public class StageData
    {
        public Vector3Int playerStartPos;   // プレイヤー開始位置
        public Vector3Int goalPos;          // ゴール位置
        public StageDataMono.WallInfo[] walls; // 壁情報
        public Vector3Int[] lightFragments; // 光の欠片の位置

        //インスペクターで色を選択
        public StartColor startColor = StartColor.White;
    }


}
