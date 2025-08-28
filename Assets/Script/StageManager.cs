using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static StageDataMono;

public class StageManager : MonoBehaviour// ステージ情報（壁の色とか）
{
    public Tilemap wallTilemap;
    public TileBase redWallTile, greenWallTile, blueWallTile;
    public GameObject lightFragmentPrefab;
    public GameObject goalPrefab;
    public GameObject playerPrefab;

    public StageDataMono[] stages;
    private int currentStage = 0;

    public PlayerManager playerManager;

    void Start()
    {
        if (stages.Length > 0)
        {
            LoadStage(0); // 最初のステージを読み込み
        }
    }

    public void LoadStage(int stageNum)
    {
        //Debug.Log("wallTilemap: " + wallTilemap);
        //Debug.Log("playerPrefab: " + playerPrefab);
        //Debug.Log("goalPrefab: " + goalPrefab);
        //Debug.Log("lightFragmentPrefab: " + lightFragmentPrefab);
        //Debug.Log("playerManager: " + playerManager);

        ClearStage();

        StageDataMono stageMono = stages[stageNum];
        StageDataMono.StageData stage = stageMono.stageData;

        // 1. 壁をTilemapに配置
        foreach (var w in stage.walls)
        {
            TileBase tile = null;
            switch (w.wallColor)
            {
                case StageDataMono.WallInfo.WallColor.R:
                    tile = redWallTile;
                    break;
                case StageDataMono.WallInfo.WallColor.G:
                    tile = greenWallTile;
                    break;
                case StageDataMono.WallInfo.WallColor.B:
                    tile = blueWallTile;
                    break;
            }
            if (tile != null) wallTilemap.SetTile(w.position, tile);
        }


        // 2. 光のかけらを配置
        foreach (var pos in stage.lightFragments)
        {
            Instantiate(lightFragmentPrefab,
                wallTilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0),
                Quaternion.identity);
        }

        // 3. ゴールを配置
        Instantiate(goalPrefab,
            wallTilemap.CellToWorld(stage.goalPos) + new Vector3(0.5f, 0.5f, 0),
            Quaternion.identity);

        // 4. プレイヤー配置
        GameObject playerObj = Instantiate(
            playerPrefab,
            wallTilemap.CellToWorld(stage.playerStartPos) + new Vector3(0.5f, 0.5f, 0),
            Quaternion.identity
        );

        PlayerColor pc = playerObj.GetComponent<PlayerColor>();

        // ステージデータの開始色を設定
        switch (stage.startColor)
        {
            case StartColor.White:
                pc.SetColor(true, true, true);
                break;
            case StartColor.Red:
                pc.SetColor(true, false, false);
                break;
            case StartColor.Green:
                pc.SetColor(false, true, false);
                break;
            case StartColor.Blue:
                pc.SetColor(false, false, true);
                break;
        }

        // 5. PlayerManager に登録
        playerManager.ClearPlayers();
        playerManager.AddPlayer(pc);
    }

    void ClearStage()
    {
        // Tilemapをクリア
        wallTilemap.ClearAllTiles();

        // 光のかけらとゴールを削除
        foreach (var frag in GameObject.FindGameObjectsWithTag("LightFragment")) Destroy(frag);
        foreach (var goal in GameObject.FindGameObjectsWithTag("Goal")) Destroy(goal);
    }
}
