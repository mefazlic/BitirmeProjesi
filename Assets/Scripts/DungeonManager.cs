using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.WebRequestMethods;

public enum DungeonType { Caverns, Rooms }

public class DungeonManager : MonoBehaviour
{
    public GameObject[] randomItems; // Holds the items that spawn randomly
    public GameObject[] randomEnemies; // Holds the items that spawn randomly
    public GameObject floorPrefab, wallPrefab, tileSpawnerPrefab, exitPrefab;

    public int totalFloorCount;
    [UnityEngine.Range(0, 100)] public int itemSpawnPercent;
    [UnityEngine.Range(0, 100)] public int enemySpawnPercent;
    public DungeonType dungeonType;

    [HideInInspector] public float minX, maxX, minY, maxY; // floors bounds (min x,y and max x,y)

    private List<Vector3> floorList = new List<Vector3>();
    LayerMask floorMask, wallMask;

    void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        wallMask = LayerMask.GetMask("Wall");


        switch(dungeonType)
        {
            case DungeonType.Caverns: RandomWalker(); break;
            case DungeonType.Rooms: RoomWalker(); break;
        }
    }

    void Update()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void RandomWalker()
    {
        Vector3 curPos = Vector3.zero;

        floorList.Add(curPos); // add the current position to the list of floors

        while (floorList.Count < totalFloorCount)
        {
            curPos += RandomDirection();

            if (!InFloorList(curPos))
            {
                floorList.Add(curPos);
            }
        }
        StartCoroutine(DelayProgress());
    }

    void RoomWalker()
    {
        Vector3 curPos = Vector3.zero;

        floorList.Add(curPos); // add the current position to the list of floors

        while (floorList.Count < totalFloorCount)
        {
            Vector3 walkDir = RandomDirection();
            int walkLenght = Random.Range(9, 18);

            for(int i = 0; i < walkLenght; i++)
            {
                if (!InFloorList(curPos + walkDir))
                {
                    floorList.Add(curPos + walkDir);
                }

                curPos += walkDir; // do this until you reach the walk lenght of room
            }

            int width = Random.Range(1, 5);
            int height = Random.Range(1,5);

            for(int w = -width; w <= width; w++) 
            {
                for (int h = -height; h <= height; h++)
                {
                    Vector3 offSet = new Vector3(w, h, 0);
                    if (!InFloorList(curPos + offSet))
                    {
                        floorList.Add(curPos + offSet);
                    }
                }
            }
        }

        StartCoroutine(DelayProgress());

    }

    Vector3 RandomDirection()
    {
        switch (Random.Range(1, 5))
        {
            case 1: return Vector3.up;
            case 2: return Vector3.right;
            case 3: return Vector3.down; 
            case 4: return Vector3.left; 
        }

        return Vector3.zero; // Unity want to return something, even if this code doesn't mean anything it satisfies the return error
    }

    bool InFloorList(Vector3 myPos)
    {
        for (int i = 0; i < floorList.Count; i++) // check if the current position is already in the list of floors
        {
            if (Vector3.Equals(myPos, floorList[i]))
            {
                return true;
            }
        }

        return false;  
    }

    IEnumerator DelayProgress() // spawn exit
    {
        for (int i = 0; i < floorList.Count; i++)
        {
            GameObject goTile = Instantiate(tileSpawnerPrefab, floorList[i], Quaternion.identity) as GameObject;
            goTile.name = tileSpawnerPrefab.name;
            goTile.transform.SetParent(transform);
        }

        while (FindObjectsOfType<TileSpawner>().Length > 0)
        {
            // TileSpawner unit is converted to Floor, so we waiting for complete
            yield return null; // wait till next frame;


        }

        ExitDoorway();

        Vector2 hitSize = Vector2.one * 0.8f;

        for (int x = (int)minX - 2; x <= (int)maxX + 2; x++) // minX - 1 : is walls, minX - 2 : is outside the walls
        {
            for (int y = (int)minY - 2; y <= (int)maxY + 2; y++) // minY - 1 : is walls, minY - 2 : is outside the walls
            {
                Collider2D hitFloor = Physics2D.OverlapBox(new Vector2(x, y), hitSize, 0, floorMask);

                if (hitFloor)
                {
                    if (!Vector2.Equals(hitFloor.transform.position, floorList[floorList.Count - 1])) // if the floor is not the exit door
                    {
                        Collider2D hitTop = Physics2D.OverlapBox(new Vector2(x, y + 1), hitSize, 0, wallMask); // check if there is a wall above the floor
                        Collider2D hitRight = Physics2D.OverlapBox(new Vector2(x + 1, y), hitSize, 0, wallMask); // check if there is a wall to the right of the floor
                        Collider2D hitBottom = Physics2D.OverlapBox(new Vector2(x, y - 1), hitSize, 0, wallMask); // check if there is a wall below the floor
                        Collider2D hitLeft = Physics2D.OverlapBox(new Vector2(x - 1, y), hitSize, 0, wallMask); // check if there is a wall to the left of the floor

                        RandomItems(hitFloor,hitTop, hitRight, hitBottom, hitLeft);
                        RandomEnemies(hitFloor, hitTop, hitRight, hitBottom, hitLeft);
                    }

                }
            }
        }

    }

    void ExitDoorway()
    {
        Vector3 doorPos = floorList[floorList.Count - 1]; // One less than the number of items that there are on the list
        // also this whole script start in 0,0,0 so this is the last element of incrament; so exit door will be very far from player

        GameObject goDoor = Instantiate(exitPrefab, doorPos, Quaternion.identity) as GameObject;
        goDoor.name = exitPrefab.name;
        goDoor.transform.SetParent(transform);
    }


    void RandomItems(Collider2D hitFloor, Collider2D hitTop, Collider2D hitRight, Collider2D hitBottom, Collider2D hitLeft)
    {

        if ((hitTop || hitRight || hitBottom || hitLeft) && !(hitTop && hitBottom) && (hitLeft && hitRight))
        {
            int roll = Random.Range(0, 101);
            if (roll <= itemSpawnPercent)
            {
                int itemIndex = Random.Range(0, randomItems.Length);

                GameObject goItem = Instantiate(randomItems[itemIndex], hitFloor.transform.position, Quaternion.identity) as GameObject; // spawn the item
                goItem.name = randomItems[itemIndex].name;
                goItem.transform.SetParent(hitFloor.transform); // set the item as a child of the floor
            }

        }
    }

    void RandomEnemies(Collider2D hitFloor, Collider2D hitTop, Collider2D hitRight, Collider2D hitBottom, Collider2D hitLeft)
    {
        if(!hitTop && !hitRight&& !hitBottom && !hitLeft) 
        {
            int roll = Random.Range(0, 101);
            if (roll <= enemySpawnPercent)
            {
                int enemyIndex = Random.Range(0, randomEnemies.Length);

                GameObject goEnemy = Instantiate(randomEnemies[enemyIndex], hitFloor.transform.position, Quaternion.identity) as GameObject; // spawn the enemy
                goEnemy.name = randomEnemies[enemyIndex].name;
                goEnemy.transform.SetParent(hitFloor.transform); // set the enemy as a child of the floor
            }
        }
    }
}