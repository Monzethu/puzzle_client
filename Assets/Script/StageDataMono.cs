using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataMono : MonoBehaviour// ステージデータ設定
{
    [System.Serializable]
    public class WallInfo
    {
        public Vector3Int position;
        public enum WallColor
        {
            R,
            G,
            B
        }

        public WallColor wallColor;
    }

    [System.Serializable]
    public class StageData
    {
        public Vector3Int playerStartPos;
        public Vector3Int goalPos;
        public WallInfo[] walls;
        public Vector3Int[] lightFragments;

        public StartColor startColor;
    }

    public StageData stageData;
}
