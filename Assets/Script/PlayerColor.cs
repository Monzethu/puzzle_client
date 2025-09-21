using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの移動・ジャンプ・色管理・分裂・ゴール判定を一括で担当
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class PlayerColor : MonoBehaviour
{
    [Header("色の状態")]
    public bool isRedActive = true;
    public bool isGreenActive = true;
    public bool isBlueActive = true;

    [Header("移動・ジャンプ・分裂設定")]
    public float speed = 5f;
    public float jumpPower = 5f;
    public GameObject playerPrefab; // クローン生成用
    public LayerMask blockLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PlayerManager manager;

    private bool hasSplit = false; // 同一フレームで多重分裂しないように制御
    public bool reachedGoal = false;
    public bool isClone = false;

    private BoxCollider2D boxCol;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>(); // 追加
        UpdateColor();
    }

    void Update()
    {
        if (reachedGoal) return;

        // 入力処理（移動）
        float x = Input.GetAxisRaw("Horizontal");
        Move(new Vector2(x, 0));

        // 入力処理（ジャンプ）
        if (IsGround() && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move(Vector2 input)
    {
        if (reachedGoal) return;

        // 横移動候補速度
        float desiredVelX = input.x * speed;

        // 左右へ進む入力があるなら、その方向に壁があって通れないかチェック
        if (Mathf.Abs(input.x) > 0.01f)
        {
            int dir = input.x > 0 ? 1 : -1;
            if (!CanMoveInDirection(dir))
            {
                desiredVelX = 0f;
            }
        }

        // Yは重力まかせ、Xだけ制御
        rb.velocity = new Vector2(desiredVelX, rb.velocity.y);
    }


    /// <summary>
    /// ジャンプ処理
    /// </summary>
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    bool IsGround()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Bounds bounds = col.bounds;

        Vector3 left = new Vector3(bounds.min.x + 0.05f, bounds.min.y, 0);
        Vector3 right = new Vector3(bounds.max.x - 0.05f, bounds.min.y, 0);
        Vector3 end = new Vector3(bounds.center.x, bounds.min.y - 0.2f, 0);

        return Physics2D.Linecast(left, end, blockLayer) || Physics2D.Linecast(right, end, blockLayer);
    }

    /// <summary>
    /// 衝突判定
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ColorWall wall = collision.collider.GetComponent<ColorWall>();
        if (wall != null && !hasSplit)
        {
            HandleWallCollision(wall);
        }
    }

    /// <summary>
    /// 色付き壁との衝突処理
    /// </summary>
    private void HandleWallCollision(ColorWall wall)
    {
        hasSplit = true;
        int dir = rb.velocity.x > 0.01f ? 1 : (rb.velocity.x < -0.01f ? -1 : 0);

        // 当たった色を判定
        bool hitRed = isRedActive && wall.wallColor == PlayerColorType.Red;
        bool hitGreen = isGreenActive && wall.wallColor == PlayerColorType.Green;
        bool hitBlue = isBlueActive && wall.wallColor == PlayerColorType.Blue;

        if (!hitRed && !hitGreen && !hitBlue) return;

        // --- クローン用（壁以外の色を持つ） ---
        bool cloneRed = isRedActive && !hitRed;
        bool cloneGreen = isGreenActive && !hitGreen;
        bool cloneBlue = isBlueActive && !hitBlue;

        // --- 元Playerは壁の色だけ残す ---
        isRedActive = hitRed;
        isGreenActive = hitGreen;
        isBlueActive = hitBlue;
        UpdateColor();

        // 元Playerが無色なら削除
        if (!isRedActive && !isGreenActive && !isBlueActive)
        {
            manager?.RemovePlayer(this);
            Destroy(gameObject);
            return;
        }

        Vector2 cloneOffset = new Vector2(dir * 0.4f, 0);
        GameObject clone = Instantiate(playerPrefab, (Vector2)transform.position + cloneOffset, Quaternion.identity);

        var pc = clone.GetComponent<PlayerColor>();
        pc.isRedActive = cloneRed;
        pc.isGreenActive = cloneGreen;
        pc.isBlueActive = cloneBlue;
        pc.playerPrefab = playerPrefab;
        pc.UpdateColor();

        if (!pc.isRedActive && !pc.isGreenActive && !pc.isBlueActive)
        {
            Destroy(clone);
            Debug.Log("[PlayerColor] Black clone destroyed");
        }
        else
        {
            pc.SetManager(manager);
            manager?.AddPlayer(pc);

            // クローンはこの壁を通れるようにする
            Collider2D wallCol = wall.GetComponent<Collider2D>();
            Collider2D cloneCol = clone.GetComponent<Collider2D>();
            if (wallCol != null && cloneCol != null)
            {
                Physics2D.IgnoreCollision(cloneCol, wallCol, true);
            }
        }
    }

    /// <summary>
    /// ゴール到達処理
    /// </summary>
    public void StopMovement()
    {
        reachedGoal = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        manager?.NotifyPlayerReachedGoal(this);
    }

    /// <summary>
    /// 色をSpriteRendererに反映
    /// </summary>
    public void UpdateColor()
    {
        sr.color = new Color(
            isRedActive ? 1f : 0f,
            isGreenActive ? 1f : 0f,
            isBlueActive ? 1f : 0f,
            1f
        );
    }

    /// <summary>
    /// Shard取得条件判定
    /// </summary>
    private bool CanCollectShard(LightShard shard)
    {
        return shard.CanCollect(GetCurrentColorSet());
    }

    /// <summary>
    /// 現在のアクティブカラーを取得
    /// </summary>
    public HashSet<PlayerColorType> GetCurrentColorSet()
    {
        var set = new HashSet<PlayerColorType>();
        if (isRedActive) set.Add(PlayerColorType.Red);
        if (isGreenActive) set.Add(PlayerColorType.Green);
        if (isBlueActive) set.Add(PlayerColorType.Blue);
        return set;
    }

    /// <summary>
    /// PlayerManagerをセット
    /// </summary>
    public void SetManager(PlayerManager pm)
    {
        manager = pm;
    }

    /// <summary>
    /// 毎フレーム末にフラグをリセット
    /// </summary>
    void LateUpdate()
    {
        hasSplit = false;
    }

    // dir は +1(右) or -1(左)
    private bool CanMoveInDirection(int dir)
    {
        if (boxCol == null) return true;

        // キャストの長さ（少し余裕を持たせる）
        float castOffset = 0.05f;
        float castDistance = Mathf.Abs(rb.velocity.x) * Time.deltaTime + castOffset;

        // BoxCast 用のサイズ（ワールドスケール考慮）
        Vector2 size = new Vector2(boxCol.size.x * Mathf.Abs(transform.lossyScale.x),
                                   boxCol.size.y * Mathf.Abs(transform.lossyScale.y));

        // BoxCast 原点はコライダ中心
        Vector2 origin = transform.TransformPoint(boxCol.offset);

        // キャスト
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, Vector2.right * dir, castDistance, blockLayer);

#if UNITY_EDITOR
        // 可視デバッグ（編集中のみ）: 線を見たいときは有効に
        Debug.DrawLine(origin, origin + Vector2.right * dir * castDistance, Color.yellow, 0.1f);
#endif

        // 色壁判定がおかしい。

        if (hit.collider == null) return true; if (hit.collider != null)
        {
            Debug.Log($"[CanMoveInDirection] Hit {hit.collider.name}, WallColor={hit.collider.GetComponent<ColorWall>()?.wallColor}");
        }

        // ヒットした相手が ColorWall なら色照合して通過可否を決める
        ColorWall cw = hit.collider.GetComponent<ColorWall>();
        if (cw != null)
        {
            bool blocked =
                (cw.wallColor == PlayerColorType.Red && isRedActive) ||
                (cw.wallColor == PlayerColorType.Green && isGreenActive) ||
                (cw.wallColor == PlayerColorType.Blue && isBlueActive);

            if (blocked)
            {
                Debug.Log($"→ 通れない {cw.wallColor}");

                // 横から当たった時にも分裂処理を発生させる
                if (!hasSplit)
                {
                    HandleWallCollision(cw);
                }

                // --- 横方向だけ壁ギリギリに補正 ---
                float halfWidth = boxCol.size.x * 0.5f * Mathf.Abs(transform.lossyScale.x);
                Bounds wallBounds = hit.collider.bounds;

                // 壁の「端」のX座標
                float wallEdgeX = (dir > 0) ? wallBounds.min.x : wallBounds.max.x;

                // プレイヤーを壁の手前で止める位置
                float targetX = wallEdgeX - dir * halfWidth;

                // Xだけ補正、YとZはそのまま
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

                return false;
            }

            return true;
        }

        // ColorWall でないブロック（床や障害物）に当たったら基本ブロック
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // LightShard 取得
        LightShard shard = other.GetComponent<LightShard>();
        if (shard != null && CanCollectShard(shard))
        {
            shard.Collect();
            GameManager.Instance.OnShardCollected();
        }

        // Trap
        if (other.CompareTag("trap"))
        {
            manager?.RemovePlayer(this);
            GameManager.Instance.GameOver();
            Destroy(gameObject);
        }

        // Goal
        if (other.CompareTag("goal"))
        {
            Debug.Log("Clear!"); // デバッグ確認
            StopMovement();
        }
    }
}
