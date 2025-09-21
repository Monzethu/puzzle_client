using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Goal goal;

    private int totalShards = 0;
    private int collectedShards = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetTotalShards(int count)
    {
        totalShards = count;
        collectedShards = 0;
        goal?.LockGoal();
    }

    public void OnShardCollected()
    {
        collectedShards++;
        if (collectedShards >= totalShards)
        {
            if (goal != null)
            {
                goal.gameObject.SetActive(true);
                goal.UnlockGoal();
            }
        }
    }

    public void GameClear()
    {
        Debug.Log("[GameManager] Game Clear!");
        // リザルト画面や次ステージ呼び出し
    }

    public void GameOver()
    {
        Debug.Log("[GameManager] Game Over!");
        // リスタート処理
    }
}
