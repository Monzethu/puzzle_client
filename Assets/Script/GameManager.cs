using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Goal goal;

    private int totalShards = 0;
    private int collectedShards = 0;

    [SerializeField] GameObject GameOverText;
    [SerializeField] GameObject GameClearText;
    [SerializeField] GameObject Next;
    [SerializeField] GameObject Home;
    [SerializeField] GameObject Restart;

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
        GameClearText.SetActive(true);
        Next.SetActive(true );
        Home.SetActive(true);
    }

    public void GameOver()
    {
        Debug.Log("[GameManager] Game Over!");
        // リスタート処理
        GameOverText.SetActive(true);
        Next.SetActive(true);
        Home.SetActive(true);
        Restart.SetActive(true);
    }
}
