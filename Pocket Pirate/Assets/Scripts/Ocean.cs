using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{
    public GameObject OceanTilePrefab;
    public TransformReference playerTransform;
    public float TilingInterval = 100f;
    public int RenderDistance = 5;

    private Dictionary<Vector2, GameObject> oceanTiles; // trans.pos.z is x, trans.pos.x is y
    private List<Vector2> currentValidTiles;
    private Vector2 lastPlayerTile;

    void Start ()
    {
        // spawn starting oceantile
        oceanTiles = new Dictionary<Vector2, GameObject>();
        currentValidTiles = new List<Vector2>();
        Retile(Vector2.zero);
    }

    void Update()
    {
        // what tile is the player in?
        int playerX = (int)(playerTransform.Value.position.x / 100);
        if (playerTransform.Value.position.x < 0)
            playerX -= 1;

        int playerY = (int)(playerTransform.Value.position.z / 100);
        if (playerTransform.Value.position.z < 0)
            playerY -= 1;

        Vector2 playerTile = new Vector2(playerX, playerY);
        //Debug.Log("player tile: " + playerTile);

        if (playerTile != lastPlayerTile) // player is in new tile, remove far ones, add new ones
        {
            Retile(playerTile);
        }
    }

    private void Retile(Vector2 newPlayerTile)
    {
        currentValidTiles.Clear();

        for (int x = -RenderDistance; x < RenderDistance + 1; x++)
        {
            for (int y = -RenderDistance; y < RenderDistance + 1; y++)
            {
                Vector2 newPos = new Vector2(newPlayerTile.x + x, newPlayerTile.y + y);
                if (!oceanTiles.ContainsKey(newPos))
                {
                    GameObject o = Instantiate(OceanTilePrefab, new Vector3(newPos.x * TilingInterval, transform.position.y, newPos.y * TilingInterval), Quaternion.identity, transform);
                    oceanTiles.Add(newPos, o);
                }
                currentValidTiles.Add(newPos);
            }
        }

        for (int x = -RenderDistance; x < RenderDistance + 1; x++)
        {
            for (int y = -RenderDistance; y < RenderDistance + 1; y++)
            {
                Vector2 lastPos = new Vector2(lastPlayerTile.x + x, lastPlayerTile.y + y);
                if (oceanTiles.ContainsKey(lastPos) && !currentValidTiles.Contains(lastPos))
                {
                    GameObject toRemove;
                    oceanTiles.TryGetValue(lastPos, out toRemove);
                    if (toRemove != null)
                        GameObject.Destroy(toRemove); // TODO replace with object pooling
                    oceanTiles.Remove(lastPos);
                }
            }
        }

        lastPlayerTile = newPlayerTile;
    }
}
