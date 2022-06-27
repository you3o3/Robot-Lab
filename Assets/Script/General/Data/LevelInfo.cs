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
    public struct RectBlock
    {
        public float x1, x2;
        public float y1, y2;
        public bool defined;

        public RectBlock(float x1, float x2, float y1, float y2)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            defined = true;
        }

        public RectBlock(Vector2 position)
        {
            x1 = position.x;
            x2 = position.x + 1;
            y1 = position.y;
            y2 = position.y + 1;
            defined = true;
        }

        public RectBlock Copy()
        {
            if (!defined) return new();
            return new RectBlock(x1, x2, y1, y2);
        }

        // position should be next to the block (+1/-1), otherwise do nothing
        // monodirectional extend, if already extended along x, then cannot extend along y, vise versa
        // return whether extend is successful
        public bool Extend(Vector2 position)
        {
            if (!defined)
            {
                this = new RectBlock(position);
                return true;
            }

            bool extendedX = (x1 + 1 != x2 && y1 + 1 == y2);
            bool extendedY = (x1 + 1 == x2 && y1 + 1 != y2);

            if ((extendedX && position.y != y1) ||
                (extendedY && position.x != x1))
            {
                return false;
            }

            if (!extendedX && !extendedY)
            {
                return ExtendX(position) || ExtendY(position);
            }

            if (extendedX)
            {
                return ExtendX(position);
            }

            if (extendedY)
            {
                return ExtendY(position);
            }

            return false;
        }

        private bool ExtendX(Vector2 position)
        {
            if (position.x == x1 - 1)
            {
                x1--;
                return true;
            }

            if (position.x + 1 == x2 + 1)
            {
                x2++;
                return true;
            }
            return false;
        }

        private bool ExtendY(Vector2 position)
        {
            if (position.y == y1 - 1)
            {
                y1--;
                return true;
            }

            if (position.y + 1 == y2 + 1)
            {
                y2++;
                return true;
            }
            return false;
        }
    }

    public GameObject levelObj;

    public GameObject background;
    public GameObject ground;
    public GameObject wall;
    public GameObject floatingPlatform;
    public GameObject traps;

    public RectBlock[] floatingPlatformPos;
    public RectBlock[] trapsPos;

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
        }
        return tilePositions;
    }

    private static RectBlock[] CombineTiles(Vector2[] tilePos)
    {
        if (tilePos.Length == 0) return new RectBlock[0];

        Vector2[] sorted = tilePos.OrderBy(o => o.x).ThenBy(o => o.y).ToArray();
        List<RectBlock> result = new();

        bool nextBlock = false;
        RectBlock block = new();
        foreach (Vector2 tile in tilePos)
        {
            nextBlock = !block.Extend(tile);
            if (nextBlock)
            {
                result.Add(block.Copy());
                block = new RectBlock(tile);
                nextBlock = false;
            }
        }
        if (block.defined)
        {
            result.Add(block.Copy());
        }
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
        floatingPlatformPos = CombineTiles(fpTilePos.ToArray());

        // trap positions
        Tilemap tmap = traps.GetComponent<Tilemap>();
        List<Vector2> tTilePos = GetAllTilePos(tmap);
        trapsPos = CombineTiles(tTilePos.ToArray());
    }
}
