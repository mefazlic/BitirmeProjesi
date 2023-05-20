using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSorcerer : EnemyAttack
{
    public int dmg;
    public override void InitiateAttack(Player player)
    {
        // blast on player location
        Debug.Log("Sorcerer Attack");

        //get floor under player position, damage if player is inside

        Health health = player.GetComponent<Health>();
        health.GetHit(dmg, transform.parent.gameObject);
    }
}
