using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWarlock : EnemyAttack
{
    public int dmg;
    public override void InitiateAttack(Player player)
    {
        // straight blast
        Debug.Log("Warlock Attack");

        

        Health health = player.GetComponent<Health>();
        health.GetHit(dmg, transform.parent.gameObject);
    }
}
