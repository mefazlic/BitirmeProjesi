using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TileSpawner : MonoBehaviour
{
    DungeonManager dungeonManager;

    void Awake()
    {
        dungeonManager = FindObjectOfType<DungeonManager>();

        GameObject goFloor = Instantiate(dungeonManager.floorPrefab, transform.position, Quaternion.identity) as GameObject;
        goFloor.name = dungeonManager.floorPrefab.name;
        goFloor.transform.SetParent(dungeonManager.transform); // set the parent of the floor to the dungeon manager

        //-------------------------------------- Dungeon Manager Bounds --------------------------------------//

        if (transform.position.x > dungeonManager.maxX)
        {
            dungeonManager.maxX = transform.position.x;
        }

        if (transform.position.x < dungeonManager.minX)
        {
            dungeonManager.minX = transform.position.x;
        }

        if (transform.position.y > dungeonManager.maxY)
        {
            dungeonManager.maxY = transform.position.y;
        }

        if( transform.transform.position.y < dungeonManager.minY)
        {
            dungeonManager.minY = transform.position.y;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        LayerMask envMask = LayerMask.GetMask("Wall", "Floor"); // get the layer mask for the walls and floors layer

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 targetPos = new Vector2(transform.position.x + x, transform.position.y + y); // get the position of the tile we are checking
                Collider2D hit = Physics2D.OverlapBox(targetPos, Vector2.one * 0.8f, 0, envMask);

                if (!hit)
                { // add a wall
                    GameObject goWall = Instantiate(dungeonManager.wallPrefab, targetPos, Quaternion.identity) as GameObject;
                    goWall.name = dungeonManager.wallPrefab.name;
                    goWall.transform.SetParent(dungeonManager.transform); // set tlhe parent of the floor to the dungeon manager
                }
            }
        }


        Destroy(gameObject); // destroy the tile spawner   
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        // instructions to tell this method what to draw
        Gizmos.DrawCube(transform.position, Vector3.one);    
    }
}

