using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState instance;

    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameState");
                instance = go.AddComponent<GameState>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private int currentRound = 0;
    private DungeonType[] dungeonTypes = { DungeonType.Caverns, DungeonType.Rooms, DungeonType.Square };

    public DungeonType GetCurrentDungeonType()
    {
        return dungeonTypes[currentRound];
    }

    public void NextRound()
    {
        currentRound++;

        if (currentRound >= dungeonTypes.Length)
        {
            currentRound = 0; // reset to first round after last round
        }
    }
}
