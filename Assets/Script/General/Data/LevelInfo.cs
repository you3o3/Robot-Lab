using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

// current strategy: pass in the level object into this
// and info would be generated in runtime
// don't change the structure of the level object
public class LevelInfo
{
    // TODO maybe move this to separate file, together with VerticalBlock
    public struct HorizontalBlock
    {
        public float xStart;
        public float xEnd;
        public float y;

        public HorizontalBlock(float xStart, float xEnd, float y)
        {
            this.xStart = xStart;
            this.xEnd = xEnd;
            this.y = y;
        }
    }

    public GameObject levelObj;

    public GameObject background;
    public GameObject ground;
    public GameObject wall;
    public GameObject floatingPlatform;
    public GameObject traps;

    // TODO maybe add vertical traps in future
    public HorizontalBlock[] floatingPlatformPos;
    public HorizontalBlock[] trapsPos;

    public GameObject winningObj;
    public GameObject[] enemies;

    public Vector2 winningPos;
    public Vector2[] enemiesPos;

    public LevelInfo(GameObject level)
    {
        if (level == null) return;

        // initialize tilemap objects
        levelObj = level;

        GameObject tileGrid = level.transform.GetChild(0).gameObject;
        background = tileGrid.transform.GetChild(0).gameObject;
        ground = tileGrid.transform.GetChild(1).gameObject;
        wall = tileGrid.transform.GetChild(2).gameObject;
        floatingPlatform = tileGrid.transform.GetChild(3).gameObject;
        traps = tileGrid.transform.GetChild(4).gameObject;

        UpdateInfo();
    }

    private static List<Vector2> GetAllTilePos(Tilemap map)
    {
        List<Vector2> tilePositions = new();
        foreach (var position in map.cellBounds.allPositionsWithin)
        {
            if (!map.HasTile(position)) continue;

            TileBase tile = map.GetTile(position);
            Vector3 tilePos = map.layoutGrid.CellToWorld(position);
            tilePositions.Add(tilePos);
            //Debug.Log("x:" + tilePos.x + " y:" + tilePos.y + " tile:" + tile.name);
        }
        return tilePositions;
    }

    private static HorizontalBlock[] CombineConsecutiveX(Vector2[] arr)
    {
        if (arr.Length == 0) return new HorizontalBlock[0];

        Vector2[] sorted = arr.OrderBy(o => o.x).ThenBy(o => o.y).ToArray();
        List<HorizontalBlock> result = new();

        float xStart = sorted[0].x, xFin = sorted[0].x;
        float ySave = sorted[0].y;

        for (int i = 1; i < arr.Length; i++)
        {
            Vector2 pos = sorted[i];
            if (pos.x == xFin + 1 && pos.y == ySave)
            {
                xFin = pos.x;
            }
            else
            {
                result.Add(new HorizontalBlock(xStart, xFin, ySave));
                xStart = pos.x;
                xFin = pos.x;
                ySave = pos.y;
            }
        }

        result.Add(new HorizontalBlock(xStart, xFin, ySave));
        return result.ToArray();
    }

    public void UpdateInfo()
    {
        // winning object
        winningObj = levelObj.transform.GetChild(1).gameObject;
        winningPos = winningObj.transform.position;

        // enemies
        List<GameObject> enemies = new();
        List<Vector2> enemiesPos = new();
        Transform e = levelObj.transform.GetChild(2);
        foreach (Transform enemy in e)
        {
            if (!enemy.gameObject.activeSelf || !enemy.gameObject.GetComponent<EnemyInteraction>().IsAlive()) continue;
            enemies.Add(enemy.gameObject);
            enemiesPos.Add(enemy.position);
        }
        this.enemies = enemies.ToArray();
        this.enemiesPos = enemiesPos.ToArray();

        //Debug.Log(enemiesPos.Count);

        // floating platform positions
        Tilemap fpmap = floatingPlatform.GetComponent<Tilemap>();
        List<Vector2> fpTilePos = GetAllTilePos(fpmap);
        floatingPlatformPos = CombineConsecutiveX(fpTilePos.ToArray());

        // trap positions
        Tilemap tmap = traps.GetComponent<Tilemap>();
        List<Vector2> tTilePos = GetAllTilePos(tmap);
        trapsPos = CombineConsecutiveX(tTilePos.ToArray());
    }
}