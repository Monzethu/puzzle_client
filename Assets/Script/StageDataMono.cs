using UnityEngine;

[System.Serializable]
public class WallInfo
{
    public Vector3 position;
    public PlayerColorType wallColor;
}

[System.Serializable]
public class ShardInfo
{
    public Vector2 position;
    public ShardColorType shardColor;
}


[System.Serializable]
public class StageData
{
    public Vector3 playerStartPos;       // プレイヤー初期位置
    public Vector3 goalPos;              // ゴール位置
    public WallInfo[] colorWalls;        // 色付き壁の情報
    public ShardInfo[] lightShards;      // LightShardの情報
}

public class StageDataMono : MonoBehaviour
{
    public StageData stageData;
}
