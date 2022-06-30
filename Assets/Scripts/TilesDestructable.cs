using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesDestructable : MonoBehaviour
{
    public Tilemap tilemapDestructable;
    private bool hasClicked;
    public Tilemap tileReplaceTilemap;
    public TileBase tileReplaceOnDestroy;
    public RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        tilemapDestructable = GetComponent<Tilemap>();
        if (tileReplaceTilemap == null)
        {
            tileReplaceTilemap = GetComponent<Tilemap>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 hitPosition = Vector3.zero;
        hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
        hitPosition.y = hit.point.y - 0.01f * hit.normal.y;

        if (tilemapDestructable != null || tileReplaceTilemap != null) {
            tilemapDestructable.SetTile(tilemapDestructable.WorldToCell(hitPosition), null);
            tileReplaceTilemap.SetTile(tileReplaceTilemap.WorldToCell(hitPosition), tileReplaceOnDestroy);
        }
        else
        {
            tilemapDestructable.SetTile(tileReplaceTilemap.WorldToCell(hitPosition), null);

        }
    }
}
