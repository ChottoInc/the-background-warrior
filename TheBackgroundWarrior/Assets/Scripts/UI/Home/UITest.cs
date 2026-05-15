using UnityEngine;
using UnityEngine.Tilemaps;

public class UITest : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    public void OnButtonTest()
    {
        //tilemap.color = new Color(1, 1, 1, 0.5f);
        Vector3Int pos = new Vector3Int(0, 0, 0);
        if (tilemap.HasTile(pos))
        {
            tilemap.SetTileFlags(pos, TileFlags.None);
            // Apply fade
            Color c = Color.red;
            //Color c = tilemap.GetColor(pos);
            //c.a = 0.2f;

            Debug.Log("Found tile pos: " + pos + ", c: " + c.ToString());
            tilemap.SetColor(pos, c);
        }
    }
}
