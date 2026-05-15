using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFader : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    [Range(0f, 1f)] 
    [SerializeField] float minAlpha = 0.2f;

    // How many columns to fade
    [SerializeField] int fadeWidth = 3;


    public void Setup()
    {
        if (tilemap == null) return;

        // Get only the used tile area
        var bounds = tilemap.cellBounds;

        int startX = GetLeftmostTileX(tilemap);
        int endX = startX + fadeWidth;

        for (int x = startX; x < endX; x++)
        {
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(pos))
                {
                    // ! unlock color modification of the tile
                    tilemap.SetTileFlags(pos, TileFlags.None);

                    // Apply fade
                    Color c = tilemap.GetColor(pos);
                    float t = Mathf.Clamp01((float)(x - startX) / fadeWidth);
                    c.a = Mathf.Lerp(minAlpha, 1f, t);

                    Debug.Log("Found tile pos: " + pos + ", c: " + c.ToString());
                    tilemap.SetColor(pos, c);
                }
            }
        }
    }

    /// <summary>
    /// Returns the X coordinate of the leftmost tile that actually exists in the tilemap.
    /// </summary>
    public static int GetLeftmostTileX(Tilemap tilemap)
    {
        var bounds = tilemap.cellBounds;
        int leftX = bounds.xMax; // start high

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                if (pos.x < leftX)
                    leftX = pos.x;
            }
        }

        return leftX;
    }
}
