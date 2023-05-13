using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSorcerer : EnemyAttack
{
    public override void InitiateAttack(Player player)
    {
        Debug.Log("Sorcerer Attack");
    }
}
