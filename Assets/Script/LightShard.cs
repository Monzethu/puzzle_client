using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class LightShard : MonoBehaviour
{
    [SerializeField]
    public ShardColorType shardColor;

    private static readonly Dictionary<ShardColorType, HashSet<PlayerColorType>> colorMap = new()
    {
        { ShardColorType.Red, new() { PlayerColorType.Red } },
        { ShardColorType.Green, new() { PlayerColorType.Green } },
        { ShardColorType.Blue, new() { PlayerColorType.Blue } },
        { ShardColorType.Cyan, new() { PlayerColorType.Green, PlayerColorType.Blue } },
        { ShardColorType.Magenta, new() { PlayerColorType.Red, PlayerColorType.Blue } },
        { ShardColorType.Yellow, new() { PlayerColorType.Red, PlayerColorType.Green } },
        { ShardColorType.White, new() { PlayerColorType.Red, PlayerColorType.Green, PlayerColorType.Blue } }
    };

    void Start()
    {
        ApplyVisualColor();
    }

    private void OnValidate()
    {
        ApplyVisualColor();
    }

    public void SetShardColor(ShardColorType color)
    {
        shardColor = color;
        ApplyVisualColor();
    }

    private void ApplyVisualColor()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = GetVisualColor(shardColor);
        }
    }

    private Color GetVisualColor(ShardColorType type) => type switch
    {
        ShardColorType.Red => Color.red,
        ShardColorType.Green => Color.green,
        ShardColorType.Blue => Color.blue,
        ShardColorType.Cyan => Color.cyan,
        ShardColorType.Magenta => Color.magenta,
        ShardColorType.Yellow => Color.yellow,
        ShardColorType.White => Color.white,
        _ => Color.white
    };

    public bool CanCollect(HashSet<PlayerColorType> playerColors)
    {
        if (!colorMap.ContainsKey(shardColor)) return false;

        var required = colorMap[shardColor];
        return playerColors.SetEquals(required);
    }

    public void Collect()
    {
        Debug.Log($"Collected {shardColor} shard!");
        GameManager.Instance?.OnShardCollected();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerColor player = other.GetComponent<PlayerColor>();
        if (player != null && CanCollect(player.GetCurrentColorSet()))
        {
            Collect();
        }
    }
}
