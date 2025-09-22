using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Goal : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        LockGoal();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ステージクリア！");
            //ShowResult();
        }
    }

    public void LockGoal()
    {
        if (sr != null) sr.color = new Color(1f, 1f, 1f, 0.3f);
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    public void UnlockGoal()
    {
        if (sr != null) sr.color = new Color(1f, 1f, 1f, 1f);
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }
}
