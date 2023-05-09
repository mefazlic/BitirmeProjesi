using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMinotaur : EnemyAttack
{
    public override void InitiateAttack(Player player)
    {
        Debug.Log("Minotaur Attack");
    }
}
