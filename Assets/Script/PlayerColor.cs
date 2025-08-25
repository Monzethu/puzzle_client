using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    public bool hasR, hasG, hasB;
    public float speed = 3f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetColor(bool r, bool g, bool b)
    {
        hasR = r; hasG = g; hasB = b;
        GetComponent<SpriteRenderer>().color = new Color(r ? 1 : 0, g ? 1 : 0, b ? 1 : 0);
    }

    public void Move(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        Vector2 targetPos = rb.position + dir * speed * Time.deltaTime;

        if (CanMove(targetPos))
        {
            rb.MovePosition(targetPos);
        }
    }

    bool CanMove(Vector2 pos)
    {
        // ï«îªíËÅiColorWallÅj
        Collider2D hit = Physics2D.OverlapCircle(pos, 0.3f, LayerMask.GetMask("Wall"));
        if (hit != null)
        {
            var wall = hit.GetComponent<ColorWall>();
            if (wall != null)
            {
                if (MatchesColor(wall.wallColor)) return false;
            }
        }
        return true;
    }

    bool MatchesColor(string color)
    {
        return (color == "R" && hasR) ||
               (color == "G" && hasG) ||
               (color == "B" && hasB);
    }
}
