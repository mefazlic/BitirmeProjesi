using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMage : EnemyAttack
{
    public GameObject missilePrefab;
    public float missileSpeed = 2f;
    public float missileDuration = 2f;

    private float missileTimer;

    public override void InitiateAttack(Player player)
    {
        // homing missile
        GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
        Vector2 dir = (player.transform.position - missile.transform.position).normalized;
        missile.GetComponent<Rigidbody2D>().velocity = dir * missileSpeed;
        missileTimer = missileDuration;
        Destroy(missile, missileDuration);
    }

    private void Update()
    {
        if (missileTimer > 0f)
        {
            missileTimer -= Time.deltaTime;
            GameObject missile = GameObject.FindGameObjectWithTag("Missile");
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            if (missile != null || player != null)
            {
                Vector2 dir = (player.position - missile.transform.position).normalized;
                missile.GetComponent<Rigidbody2D>().velocity = dir * missileSpeed;
            }
        } 
    }
}

