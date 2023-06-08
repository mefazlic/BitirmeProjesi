using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeEnemyAI : MonoBehaviour
{
    public EnemyType enemyType;
    EnemyAttack attackScript;

    public Vector2 patrolInterval;
    public float alertRange;
    public float chaseSpeed;

    Player player;
    LayerMask obstacleMask, walkableMask;
    Vector2 curPos;
    List<Vector2> availableMovementList = new List<Vector2>();
    List<PathNode> nodesList = new List<PathNode>();

    bool isMoving;

    private void Start()
    {
        if (enemyType == EnemyType.CREEPER) { attackScript = gameObject.GetComponent<AttackCreeper>(); }
        else if (enemyType == EnemyType.GOBLIN) { attackScript = gameObject.AddComponent<AttackGoblin>(); }

        player = FindObjectOfType<Player>();

        obstacleMask = LayerMask.GetMask("Wall", "Enemy", "Player");
        walkableMask = LayerMask.GetMask("Wall", "Enemy"); // this is the same as obstacleMask but without the player layer so that the enemy can move through the player

        curPos = transform.position;

        StartCoroutine(Movement());
    }

    private void Patrol()
    {
        availableMovementList.Clear();

        Vector2 size = Vector2.one * 0.8f;

        Collider2D hitUp = Physics2D.OverlapBox(curPos + Vector2.up, size, 0, obstacleMask);
        if(!hitUp){availableMovementList.Add(Vector2.up);}

        Collider2D hitRight = Physics2D.OverlapBox(curPos + Vector2.right, size, 0, obstacleMask);
        if (!hitRight) { availableMovementList.Add(Vector2.right);}

        Collider2D hitDown = Physics2D.OverlapBox(curPos + Vector2.down, size, 0, obstacleMask);
        if (!hitDown) { availableMovementList.Add(Vector2.down);}

        Collider2D hitLeft = Physics2D.OverlapBox(curPos + Vector2.left, size, 0, obstacleMask);
        if (!hitLeft) { availableMovementList.Add(Vector2.left);}

        if (availableMovementList.Count > 0) // if there is a direction to move
        {
            int randomIndex = UnityEngine.Random.Range(0, availableMovementList.Count);
            Vector2 randomDir = availableMovementList[randomIndex];
            curPos += availableMovementList[randomIndex];
            //transform.position = curPos;
        }
        StartCoroutine(SmoothMove(Random.Range(patrolInterval.x, patrolInterval.y)));
    }

    IEnumerator SmoothMove(float speed)
    {
        isMoving = true;

        while(Vector2.Distance(transform.position, curPos) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, curPos, 5f * Time.deltaTime);
            yield return null;
        } transform.position = curPos;

        yield return new WaitForSeconds(speed);

        isMoving = false;
    }

    void CheckNode(Vector2 chkPoint, Vector2 parent)
    {
        Vector2 size = Vector2.one * 0.8f;
        Collider2D hit = Physics2D.OverlapBox(chkPoint, size, 0, walkableMask);

        if (!hit)
        {
            nodesList.Add(new PathNode(chkPoint, parent));
        }
    }

    Vector2 FindNextStep(Vector2 startPos, Vector2 targetPos)
    {
        int listIndex = 0;
        Vector2 myPos = startPos;
        nodesList.Clear();
        nodesList.Add(new PathNode(startPos, startPos));

        while (myPos != targetPos && listIndex < 1000 && nodesList.Count > 0)
        {
            // check up down left right for available movement
            CheckNode(myPos + Vector2.up, myPos);
            CheckNode(myPos + Vector2.right, myPos);
            CheckNode(myPos + Vector2.down, myPos);
            CheckNode(myPos + Vector2.left, myPos);

            listIndex++;
            if(listIndex < nodesList.Count)
            {
                myPos = nodesList[listIndex].position;
            }
        }
        if (myPos == targetPos)
        {
            nodesList.Reverse(); // crawl backwards from the target to the start
            for(int i = 0; i < nodesList.Count; i++)
            {
                if (myPos == nodesList[i].position)
                {
                    if (nodesList[i].parent == startPos)
                    {
                        return myPos;
                    }
                    myPos = nodesList[i].parent;
                }
            }
        }
        return startPos;
    }

    IEnumerator Movement()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!isMoving)
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if (dist <= alertRange)
                {
                    if (dist <= 1.1f)
                    {
                        attackScript.InitiateAttack(player);
                        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.15f));
                    }

                    else
                    {
                        Vector2 newPos = FindNextStep(transform.position, player.transform.position);
                        if (newPos != curPos)
                        {
                            curPos = newPos;
                            StartCoroutine(SmoothMove(chaseSpeed));
                        }
                        else
                        {
                            Patrol();
                        }
                    }
                }
                else
                {
                    Patrol();
                }
            }
        }

    }
}
