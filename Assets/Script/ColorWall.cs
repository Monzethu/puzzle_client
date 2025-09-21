using UnityEngine;

public class ColorWall : MonoBehaviour
{
    public PlayerColorType wallColor;

    private void Start()
    {
        UpdateWallColor();
    }

    // Inspector‚Å’l‚ð•Ï‚¦‚½‚Æ‚«‚É‚à‘¦”½‰f‚³‚¹‚é
    private void OnValidate()
    {
        UpdateWallColor();
    }

    public void UpdateWallColor()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = wallColor switch
            {
                PlayerColorType.Red => Color.red,
                PlayerColorType.Green => Color.green,
                PlayerColorType.Blue => Color.blue,
                _ => Color.white
            };
        }
    }
}
