using UnityEngine;

public class ColorWall : MonoBehaviour
{
    public string wallColor; //"R","G","B"

    void OnTriggerEnter2D(Collider2D col)
    {
        var pc = col.GetComponent<PlayerColor>();
        if (pc == null) return;

        // �����Ă�F�ƕǐF����v�����番������
        if ((wallColor == "R" && pc.hasR) ||
            (wallColor == "G" && pc.hasG) ||
            (wallColor == "B" && pc.hasB))
        {

            PlayerManager pm = FindObjectOfType<PlayerManager>();
            pm.SplitPlayer(pc, wallColor);
        }
    }
}
