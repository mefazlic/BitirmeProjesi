using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMage : EnemyAttack
{
    public override void InitiateAttack(Player player)
    {
        Debug.Log("Mage Attack");
    }
}

