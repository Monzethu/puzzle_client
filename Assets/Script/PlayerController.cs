using UnityEngine;

public class PlayerController : MonoBehaviour// Playerの動き
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        // 横方向のみ操作、縦は物理エンジンに任せる
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }
}
