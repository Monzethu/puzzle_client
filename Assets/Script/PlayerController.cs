using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public LayerMask blockLayer;

    private float moveSpeed = 11f;
    private Rigidbody2D rb;
    private Vector2 move;
    float jumpPower = 400;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move = Vector2.zero;

        if (Input.GetKey(KeyCode.D)) move.x += 1;
        if (Input.GetKey(KeyCode.A)) move.x -= 1;

        // Spaceでジャンプ
        if (IsGround()&&Input.GetKeyDown("space"))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // 左右に移動
        Vector2 velocity = rb.velocity;
        velocity.x = move.normalized.x * moveSpeed;
        rb.velocity = velocity;
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpPower);
    }

    bool IsGround()
    {
        // 始点と終点を作る
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        Debug.DrawLine(leftStartPoint, endPoint);
        Debug.DrawLine(rightStartPoint, endPoint);
        return Physics2D.Linecast(leftStartPoint, endPoint,blockLayer)
            || Physics2D.Linecast(rightStartPoint, endPoint, blockLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "trap")
        {
            Debug.Log("aaaaaaaaaa");
            gameManager.GameOver();
        }
        if (collision.gameObject.tag == "finish")
        {
            Debug.Log("!!!!!!!!!!");
            gameManager.GameClear();
        }
    }
}
