using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static StageDataMono;

public class StageManager : MonoBehaviour
{
    public Tilemap wallTilemap;
    public TileBase redWallTile, greenWallTile, blueWallTile;
    public GameObject lightFragmentPrefab;
    public GameObject goalPrefab;
    public GameObject playerPrefab;

    public StageData[] stages;
    private int currentStage = 0;

    public PlayerManager playerManager;

    void Start()
    {
        LoadStage(currentStage);
    }

    public void LoadStage(int stageNum)
    {
        ClearStage();

        StageData stage = stages[stageNum];

        // 1. �ǂ��^�C���}�b�v�ɔz�u
        foreach (var w in stage.walls)
        {
            TileBase tile = null;
            switch (w.color)
            {
                case "R": tile = redWallTile; break;
                case "G": tile = greenWallTile; break;
                case "B": tile = blueWallTile; break;
            }
            if (tile != null) wallTilemap.SetTile(w.position, tile);
        }

        // 2. ���̌��Ђ�z�u
        foreach (var pos in stage.lightFragments)
        {
            Instantiate(lightFragmentPrefab,
                wallTilemap.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0),
                Quaternion.identity);
        }

        // 3. �S�[����z�u
        Instantiate(goalPrefab,
            wallTilemap.CellToWorld(stage.goalPos) + new Vector3(0.5f, 0.5f, 0),
            Quaternion.identity);

        // 4. �v���C���[����
        GameObject playerObj = Instantiate(
            playerPrefab,
            wallTilemap.CellToWorld(stage.playerStartPos) + new Vector3(0.5f, 0.5f, 0),
            Quaternion.identity
        );

        PlayerColor pc = playerObj.GetComponent<PlayerColor>();

        // �X�e�[�W�f�[�^�̏����F�𔽉f
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

        // 5. PlayerManager �ɓo�^
        playerManager.ClearPlayers();
        playerManager.AddPlayer(pc);
    }

    void ClearStage()
    {
        // �^�C���}�b�v���N���A
        wallTilemap.ClearAllTiles();

        // ���̌��ЂƃS�[�����폜
        foreach (var frag in GameObject.FindGameObjectsWithTag("LightFragment")) Destroy(frag);
        foreach (var goal in GameObject.FindGameObjectsWithTag("Goal")) Destroy(goal);
    }
}
