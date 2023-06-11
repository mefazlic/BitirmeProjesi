using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMage : EnemyAttack
{
    public GameObject missilePrefab;
    public float missileSpeed = 2f;
    public float missileDuration = 2f;

    public int missilePerBurst = 3;
    public float cooldown = 1.5f;
    private int missileCount = 0;
    private bool isCooldownActive = false;

    public AttackMage()
    {
        this.isRanged = true;
    }

    public override void InitiateAttack(Player player)
    {
        if (isCooldownActive) { return; }
        if (missileCount >= missilePerBurst) 
        {
            missileCount = 0;
            isCooldownActive = true;
            StartCoroutine(BurstCooldown());
            return;
        }
        // homing missile
        GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        Vector2 dir = (player.transform.position - missile.transform.position).normalized;
        missile.GetComponent<Rigidbody2D>().velocity = dir * missileSpeed;
        missile.GetComponent<Mage_Missile>().missileTimer = missileDuration;
        missile.GetComponent<Mage_Missile>().missileSpeed = missileSpeed;
        Destroy(missile, missileDuration);
        missileCount++;
    }    
    IEnumerator BurstCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        isCooldownActive = false;
    }
}

