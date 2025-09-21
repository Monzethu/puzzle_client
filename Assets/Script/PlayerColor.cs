using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̈ړ��E�W�����v�E�F�Ǘ��E����E�S�[��������ꊇ�ŒS��
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class PlayerColor : MonoBehaviour
{
    [Header("�F�̏��")]
    public bool isRedActive = true;
    public bool isGreenActive = true;
    public bool isBlueActive = true;

    [Header("�ړ��E�W�����v�E�����ݒ�")]
    public float speed = 5f;
    public float jumpPower = 5f;
    public GameObject playerPrefab; // �N���[�������p
    public LayerMask blockLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PlayerManager manager;

    private bool hasSplit = false; // ����t���[���ő��d���􂵂Ȃ��悤�ɐ���
    public bool reachedGoal = false;
    public bool isClone = false;

    private BoxCollider2D boxCol;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>(); // �ǉ�
        UpdateColor();
    }

    void Update()
    {
        if (reachedGoal) return;

        // ���͏����i�ړ��j
        float x = Input.GetAxisRaw("Horizontal");
        Move(new Vector2(x, 0));

        // ���͏����i�W�����v�j
        if (IsGround() && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    public void Move(Vector2 input)
    {
        if (reachedGoal) return;

        // ���ړ���⑬�x
        float desiredVelX = input.x * speed;

        // ���E�֐i�ޓ��͂�����Ȃ�A���̕����ɕǂ������Ēʂ�Ȃ����`�F�b�N
        if (Mathf.Abs(input.x) > 0.01f)
        {
            int dir = input.x > 0 ? 1 : -1;
            if (!CanMoveInDirection(dir))
            {
                desiredVelX = 0f;
            }
        }

        // Y�͏d�͂܂����AX��������
        rb.velocity = new Vector2(desiredVelX, rb.velocity.y);
    }


    /// <summary>
    /// �W�����v����
    /// </summary>
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    /// <summary>
    /// �ڒn����
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
    /// �Փ˔���
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
    /// �F�t���ǂƂ̏Փˏ���
    /// </summary>
    private void HandleWallCollision(ColorWall wall)
    {
        hasSplit = true;
        int dir = rb.velocity.x > 0.01f ? 1 : (rb.velocity.x < -0.01f ? -1 : 0);

        // ���������F�𔻒�
        bool hitRed = isRedActive && wall.wallColor == PlayerColorType.Red;
        bool hitGreen = isGreenActive && wall.wallColor == PlayerColorType.Green;
        bool hitBlue = isBlueActive && wall.wallColor == PlayerColorType.Blue;

        if (!hitRed && !hitGreen && !hitBlue) return;

        // --- �N���[���p�i�ǈȊO�̐F�����j ---
        bool cloneRed = isRedActive && !hitRed;
        bool cloneGreen = isGreenActive && !hitGreen;
        bool cloneBlue = isBlueActive && !hitBlue;

        // --- ��Player�͕ǂ̐F�����c�� ---
        isRedActive = hitRed;
        isGreenActive = hitGreen;
        isBlueActive = hitBlue;
        UpdateColor();

        // ��Player�����F�Ȃ�폜
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

            // �N���[���͂��̕ǂ�ʂ��悤�ɂ���
            Collider2D wallCol = wall.GetComponent<Collider2D>();
            Collider2D cloneCol = clone.GetComponent<Collider2D>();
            if (wallCol != null && cloneCol != null)
            {
                Physics2D.IgnoreCollision(cloneCol, wallCol, true);
            }
        }
    }

    /// <summary>
    /// �S�[�����B����
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
    /// �F��SpriteRenderer�ɔ��f
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
    /// Shard�擾��������
    /// </summary>
    private bool CanCollectShard(LightShard shard)
    {
        return shard.CanCollect(GetCurrentColorSet());
    }

    /// <summary>
    /// ���݂̃A�N�e�B�u�J���[���擾
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
    /// PlayerManager���Z�b�g
    /// </summary>
    public void SetManager(PlayerManager pm)
    {
        manager = pm;
    }

    /// <summary>
    /// ���t���[�����Ƀt���O�����Z�b�g
    /// </summary>
    void LateUpdate()
    {
        hasSplit = false;
    }

    // dir �� +1(�E) or -1(��)
    private bool CanMoveInDirection(int dir)
    {
        if (boxCol == null) return true;

        // �L���X�g�̒����i�����]�T����������j
        float castOffset = 0.05f;
        float castDistance = Mathf.Abs(rb.velocity.x) * Time.deltaTime + castOffset;

        // BoxCast �p�̃T�C�Y�i���[���h�X�P�[���l���j
        Vector2 size = new Vector2(boxCol.size.x * Mathf.Abs(transform.lossyScale.x),
                                   boxCol.size.y * Mathf.Abs(transform.lossyScale.y));

        // BoxCast ���_�̓R���C�_���S
        Vector2 origin = transform.TransformPoint(boxCol.offset);

        // �L���X�g
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, Vector2.right * dir, castDistance, blockLayer);

#if UNITY_EDITOR
        // ���f�o�b�O�i�ҏW���̂݁j: �����������Ƃ��͗L����
        Debug.DrawLine(origin, origin + Vector2.right * dir * castDistance, Color.yellow, 0.1f);
#endif

        // �F�ǔ��肪���������B

        if (hit.collider == null) return true; if (hit.collider != null)
        {
            Debug.Log($"[CanMoveInDirection] Hit {hit.collider.name}, WallColor={hit.collider.GetComponent<ColorWall>()?.wallColor}");
        }

        // �q�b�g�������肪 ColorWall �Ȃ�F�ƍ����Ēʉ߉ۂ����߂�
        ColorWall cw = hit.collider.GetComponent<ColorWall>();
        if (cw != null)
        {
            bool blocked =
                (cw.wallColor == PlayerColorType.Red && isRedActive) ||
                (cw.wallColor == PlayerColorType.Green && isGreenActive) ||
                (cw.wallColor == PlayerColorType.Blue && isBlueActive);

            if (blocked)
            {
                Debug.Log($"�� �ʂ�Ȃ� {cw.wallColor}");

                // �����瓖���������ɂ����􏈗��𔭐�������
                if (!hasSplit)
                {
                    HandleWallCollision(cw);
                }

                // --- �����������ǃM���M���ɕ␳ ---
                float halfWidth = boxCol.size.x * 0.5f * Mathf.Abs(transform.lossyScale.x);
                Bounds wallBounds = hit.collider.bounds;

                // �ǂ́u�[�v��X���W
                float wallEdgeX = (dir > 0) ? wallBounds.min.x : wallBounds.max.x;

                // �v���C���[��ǂ̎�O�Ŏ~�߂�ʒu
                float targetX = wallEdgeX - dir * halfWidth;

                // X�����␳�AY��Z�͂��̂܂�
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

                return false;
            }

            return true;
        }

        // ColorWall �łȂ��u���b�N�i�����Q���j�ɓ����������{�u���b�N
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // LightShard �擾
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
            Debug.Log("Clear!"); // �f�o�b�O�m�F
            StopMovement();
        }
    }
}
