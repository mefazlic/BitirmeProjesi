using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCreeper : EnemyAttack
{
    public override void InitiateAttack(Player player)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        Health health = player.GetComponent<Health>();
        health.GetHit(7, transform.parent.gameObject);
        Destroy(gameObject);
    }
}
