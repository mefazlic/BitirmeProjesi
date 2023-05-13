using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWarlock : EnemyAttack
{
    public override void InitiateAttack(Player player)
    {
        Debug.Log("Warlock Attack");
    }
}
