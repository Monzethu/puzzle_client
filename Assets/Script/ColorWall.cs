using UnityEngine;

public class ColorWall : MonoBehaviour
{
    public enum WallColor { R, G, B }
    public WallColor wallColor;
    //public StageDataMono.WallInfo.WallColor wallColor;

    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerColor pc = collision.gameObject.GetComponent<PlayerColor>();
        if (pc != null)
        {
            // 壁の色とプレイヤーの色をチェック
            bool canPass = false;
            switch (wallColor)
            {
                case WallColor.R: canPass = !pc.hasR; break;
                case WallColor.G: canPass = !pc.hasG; break;
                case WallColor.B: canPass = !pc.hasB; break;
            }

            // 通れるなら一時的にColliderを無効化
            if (canPass)
            {
                col.enabled = false;
                Invoke(nameof(EnableCollider), 0.2f); // すぐ戻す
            }
        }
    }

    void EnableCollider()
    {
        col.enabled = true;
    }
}
