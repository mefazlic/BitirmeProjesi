using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private EnemyAttack[] attackScripts = new EnemyAttack[3];
    private EnemyAttack currentAttack;

    public Dictionary<string, float> weights = new Dictionary<string, float>();
    public Dictionary<string, float> results = new Dictionary<string, float>();

    Player player;
    LayerMask obstacleMask, walkableMask;
    Vector2 curPos;
    List<Vector2> availableMovementList = new List<Vector2>();
    List<PathNode> nodesList = new List<PathNode>();
    bool isMoving;
    Health health;

    public Vector2 patrolInterval;
    public float alertRange;
    public float chaseSpeed;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        health = GetComponent<Health>();

        weights.Add("Goblin", 0.7f);
        weights.Add("Creeper", 0.6f);
        weights.Add("Bat", 0.65f);
        weights.Add("Mage", 0.6f);
        weights.Add("Sorcerer", 0.8f);
        weights.Add("Warlock", 0.75f);
        ChooseAttacks();
        currentAttack = attackScripts[2];

        obstacleMask = LayerMask.GetMask("Wall", "Enemy", "Player");
        walkableMask = LayerMask.GetMask("Wall", "Enemy"); // this is the same as obstacleMask but without the player layer so that the enemy can move through the player

        curPos = transform.position;

        StartCoroutine(Movement());
    }
    private void Update()
    {        
        if (health.currentHealth < health.maxHealth * 3 / 10)
        {
            currentAttack = attackScripts[0];
        }
        else if (health.currentHealth < health.maxHealth * 7 / 10)
        {
            currentAttack = attackScripts[1];
        }
    }
    private void ChooseAttacks()
    {
        results.Add("Goblin",(AttackGoblin.damageFromGoblin * 1.5f - WeaponParent.damageToGoblin * 0.25f) / 2 * weights["Goblin"]);
        results.Add("Creeper", (AttackCreeper.damageFromCreeper * 1.5f - WeaponParent.damageToCreeper * 0.25f) / 2 * weights["Creeper"]);
        results.Add("Bat", (AttackBat.damageFromBat * 1.5f - WeaponParent.damageToBat * 0.25f) / 2 * weights["Bat"]);
        results.Add("Mage", (Mage_Missile.damageFromMage * 1.5f - WeaponParent.damageToMage * 0.25f) / 2 * weights["Mage"]);
        results.Add("Sorcerer", (Blast.damageFromSorcerer * 1.5f - WeaponParent.damageToSorcerer * 0.25f) / 2 * weights["Sorcerer"]);
        results.Add("Warlock", (Laser.damageFromWarlock * 1.5f  - WeaponParent.damageToWarlock * 0.25f) / 2 * weights["Warlock"]);
        foreach (var item in results) { print(item.Key+ ": " + item.Value); }
        var orderedResults = results.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        print("Selected Attack Types:");
        var index = 0;
        foreach (var result in orderedResults)
        {
            print(result.Key + ": " + result.Value);
            if (result.Key == "Goblin") { attackScripts[index] = gameObject.GetComponent<AttackGoblin>(); }
            else if (result.Key == "Creeper") { attackScripts[index] = gameObject.GetComponent<AttackCreeper>(); }
            else if (result.Key == "Bat") { attackScripts[index] = gameObject.GetComponent<AttackBat>(); }
            else if (result.Key == "Mage") { attackScripts[index] = gameObject.GetComponent<AttackMage>(); }
            else if (result.Key == "Sorcerer") { attackScripts[index] = gameObject.GetComponent<AttackSorcerer>(); }
            else if (result.Key == "Warlock") { attackScripts[index] =  gameObject.GetComponent<AttackWarlock>(); }
            index++;
            if (index == 3) { break; } 
        }
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
                    if (currentAttack.isRanged)
                    {
                        if (dist <= 6f)
                        {
                            currentAttack.InitiateAttack(player);
                            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
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
                        if (dist <= 1.1f)
                        {
                            currentAttack.InitiateAttack(player);
                            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
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
                    
                }
                else
                {
                    Patrol();
                }
            }
        }

    }
    private void Patrol()
    {
        availableMovementList.Clear();

        Vector2 size = Vector2.one * 0.8f;

        Collider2D hitUp = Physics2D.OverlapBox(curPos + Vector2.up, size, 0, obstacleMask);
        if (!hitUp) { availableMovementList.Add(Vector2.up); }

        Collider2D hitRight = Physics2D.OverlapBox(curPos + Vector2.right, size, 0, obstacleMask);
        if (!hitRight) { availableMovementList.Add(Vector2.right); }

        Collider2D hitDown = Physics2D.OverlapBox(curPos + Vector2.down, size, 0, obstacleMask);
        if (!hitDown) { availableMovementList.Add(Vector2.down); }

        Collider2D hitLeft = Physics2D.OverlapBox(curPos + Vector2.left, size, 0, obstacleMask);
        if (!hitLeft) { availableMovementList.Add(Vector2.left); }

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

        while (Vector2.Distance(transform.position, curPos) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, curPos, 5f * Time.deltaTime);
            yield return null;
        }
        transform.position = curPos;

        yield return new WaitForSeconds(0.3f);

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
            if (listIndex < nodesList.Count)
            {
                myPos = nodesList[listIndex].position;
            }
        }
        if (myPos == targetPos)
        {
            nodesList.Reverse(); // crawl backwards from the target to the start
            for (int i = 0; i < nodesList.Count; i++)
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
    
}
