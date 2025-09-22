using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]GameManager gameManager;

    private List<PlayerColor> players = new List<PlayerColor>();
    private Vector2 inputDir;

    void Update()
    {
        inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        var currentPlayers = new List<PlayerColor>(players);
        foreach (var pc in currentPlayers)
        {
            if (pc != null && !pc.reachedGoal)
            {
                var controller = pc.GetComponent<PlayerColor>();
                if (controller != null)
                    controller.Move(inputDir);

                // 壁判定は PlayerColor が持っている
            }
        }
    }

    public void AddPlayer(PlayerColor pc)
    {
        if (pc == null) return;
        if (!players.Contains(pc))
        {
            players.Add(pc);
            pc.SetManager(this);
        }
    }

    public void RemovePlayer(PlayerColor pc)
    {
        if (pc != null && players.Contains(pc))
            players.Remove(pc);
    }

    public void NotifyPlayerReachedGoal(PlayerColor pc)
    {
        // 全員ゴールしたらクリア
        bool allReached = true;
        foreach (var p in players)
        {
            if (!p.reachedGoal)
            {
                allReached = false;
                break;
            }
        }

        if (allReached)
        {
            gameManager.GameClear();
        }
    }

    public void ClearPlayers()
    {
        foreach (var pc in players)
        {
            if (pc != null)
                Destroy(pc.gameObject);
        }
        players.Clear();
    }


}
