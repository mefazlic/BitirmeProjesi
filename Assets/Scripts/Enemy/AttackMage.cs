using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMage : EnemyAttack
{
    public GameObject missilePrefab;
    public float missileSpeed = 2f;
    public float missileDuration = 2f;

    public AttackMage()
    {
        this.isRanged = true;
    }

    public override void InitiateAttack(Player player)
    {
        // homing missile
        GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        Vector2 dir = (player.transform.position - missile.transform.position).normalized;
        missile.GetComponent<Rigidbody2D>().velocity = dir * missileSpeed;
        missile.GetComponent<Mage_Missile>().missileTimer = missileDuration;
        missile.GetComponent<Mage_Missile>().missileSpeed = missileSpeed;
        Destroy(missile, missileDuration);
    }    
}

