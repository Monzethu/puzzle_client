using UnityEngine;

public class ColorWall : MonoBehaviour
{
    public string wallColor; //"R","G","B"

    void OnTriggerEnter2D(Collider2D col)
    {
        var pc = col.GetComponent<PlayerColor>();
        if (pc == null) return;

        // ‚Á‚Ä‚éF‚Æ•ÇF‚ªˆê’v‚µ‚½‚ç•ª—£”»’è
        if ((wallColor == "R" && pc.hasR) ||
            (wallColor == "G" && pc.hasG) ||
            (wallColor == "B" && pc.hasB))
        {

            PlayerManager pm = FindObjectOfType<PlayerManager>();
            pm.SplitPlayer(pc, wallColor);
        }
    }
}
