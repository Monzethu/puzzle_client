using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageDataMono stageDataMono;
    public GameObject playerPrefab;
    public GameObject wallPrefab;
    public GameObject lightShardPrefab;
    public GameObject goalPrefab;

    private PlayerManager playerManager;

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        LoadStage();
    }

    void LoadStage()
    {
        if (stageDataMono == null || stageDataMono.stageData == null) return;
        var stage = stageDataMono.stageData;

        // プレイヤー
        GameObject playerObj = Instantiate(playerPrefab, stage.playerStartPos, Quaternion.identity);
        var pc = playerObj.GetComponent<PlayerColor>();
        if (pc != null)
        {
            pc.SetManager(playerManager);
            playerManager.AddPlayer(pc);
        }

        // 壁
        foreach (var wall in stage.colorWalls)
        {
            var wallObj = Instantiate(wallPrefab, wall.position, Quaternion.identity);
            var cw = wallObj.GetComponent<ColorWall>();
            if (cw != null) cw.wallColor = wall.wallColor;
        }

        // LightShard
        int shardCount = 0;
        foreach (var shard in stage.lightShards)
        {
            GameObject shardObj = Instantiate(lightShardPrefab, shard.position, Quaternion.identity);
            var shardComp = shardObj.GetComponent<LightShard>();
            if (shardComp != null)
            {
                // ←ここで色を指定
                shardComp.SetShardColor(shard.shardColor);
            }
            shardCount++;
        }


        // Goal
        GameObject goalObj = Instantiate(goalPrefab, stage.goalPos, Quaternion.identity);
        var goalComp = goalObj.GetComponent<Goal>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.goal = goalComp;
            GameManager.Instance.SetTotalShards(shardCount);
        }
    }
}
