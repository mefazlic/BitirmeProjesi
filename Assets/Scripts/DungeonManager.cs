using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public GameObject floorPrefab, wallPrefab, tileSpawnerPrefab, exitPrefab;
    public int totalFloorCount;

    [HideInInspector] public float minX, maxX, minY, maxY; // floors bounds (min x,y and max x,y)

    private List <Vector3> floorList = new List <Vector3>();

    void Start()
    {
        RandomWalker();

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
                case 1: curPos += Vector3.up;    break;
                case 2: curPos += Vector3.right; break;
                case 3: curPos += Vector3.down;  break;
                case 4: curPos += Vector3.left;  break;
            }

            bool inFloorList = false;
            for(int i = 0; i < floorList.Count; i++) // check if the current position is already in the list of floors
            {
                if (Vector3.Equals(curPos, floorList[i]))
                {
                    inFloorList= true;
                    break;
                }
            }

            if (!inFloorList)
            {
                floorList.Add(curPos);
            }

        }

        for(int i= 0; i<floorList.Count; i++)
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

            ExitDoorway();
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
}
