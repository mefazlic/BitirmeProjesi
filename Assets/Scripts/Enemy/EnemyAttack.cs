using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    public bool isRanged;
    public abstract void InitiateAttack(Player player);
}
