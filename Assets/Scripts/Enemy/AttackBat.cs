using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBat : EnemyAttack
{
    public override void InitiateAttack(Player player)
    {
        Debug.Log("Bat Attack");
    }
}
