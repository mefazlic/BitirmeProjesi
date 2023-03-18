using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.WebRequestMethods;

public class DungeonManager : MonoBehaviour
{
    public GameObject[] randomItems; // Holds the items that spawn randomly
    public GameObject floorPrefab, wallPrefab, tileSpawnerPrefab, exitPrefab;

    public int totalFloorCount;
    [UnityEngine.Range(0, 100)] public int itemSpawnPercent;

    [HideInInspector] public float minX, maxX, minY, maxY; // floors bounds (min x,y and max x,y)

    private List<Vector3> floorList = new List<Vector3>();
    LayerMask floorMask, wallMask;

    void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        RandomWalker();
        wallMask = LayerMask.GetMask("Wall");

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
            switch (Random.Range(1, 5))
            {
                case 1: curPos += Vector3.up; break;
                case 2: curPos += Vector3.right; break;
                case 3: curPos += Vector3.down; break;
                case 4: curPos += Vector3.left; break;
            }

            bool inFloorList = false;
            for (int i = 0; i < floorList.Count; i++) // check if the current position is already in the list of floors
            {
                if (Vector3.Equals(curPos, floorList[i]))
                {
                    inFloorList = true;
                    break;
                }
            }

            if (!inFloorList)
            {
                floorList.Add(curPos);
            }

        }

        for (int i = 0; i < floorList.Count; i++)
        {
            GameObject goTile = Instantiate(tileSpawnerPrefab, floorList[i], Quaternion.identity) as GameObject;
            goTile.name = tileSpawnerPrefab.name;
            goTile.transform.SetParent(transform);
        }

        StartCoroutine(DelayProgress());

    }

    // spawn exit
    IEnumerator DelayProgress()
    {
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
}