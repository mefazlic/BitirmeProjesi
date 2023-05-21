using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCreeper : EnemyAttack
{
    public int dmg = 7;
    public override void InitiateAttack(Player player)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;

        StartCoroutine(Explode(player));
    }

    IEnumerator Explode(Player player)
    {
        yield return new WaitForSeconds(1f);
        Health health = player.GetComponent<Health>();
        health.GetHit(dmg, transform.parent.gameObject);
        Destroy(gameObject);
    }
}
