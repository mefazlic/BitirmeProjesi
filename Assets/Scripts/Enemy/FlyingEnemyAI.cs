using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour
{
    public EnemyType enemyType;
    EnemyAttack attackScript;
    Player player;

    public float speed;
    public float checkRadius;
    public float attackRadius;

    public LayerMask whatIsPlayer;

    private Rigidbody2D rb;
    private Vector2 movement;
    public Vector3 dir;

    private bool isInChaseRange;
    private bool isInAttackRange;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        if (enemyType == EnemyType.BAT) { attackScript = gameObject.AddComponent<AttackBat>(); }
    }
    private void Update()
    {
        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);

        dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        dir.Normalize();
        movement = dir;
    }
    private void FixedUpdate()
    {
        if (isInChaseRange && !isInAttackRange)
        {
            rb.velocity = movement * speed;
        }
        if (isInAttackRange)
        {
            rb.velocity = Vector2.zero;
            attackScript.InitiateAttack(player);
        }
    }
}
