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
    public Vector3 playerStartPos;       // �v���C���[�����ʒu
    public Vector3 goalPos;              // �S�[���ʒu
    public WallInfo[] colorWalls;        // �F�t���ǂ̏��
    public ShardInfo[] lightShards;      // LightShard�̏��
}

public class StageDataMono : MonoBehaviour
{
    public StageData stageData;
}
