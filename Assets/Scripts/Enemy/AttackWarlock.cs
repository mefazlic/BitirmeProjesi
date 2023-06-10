using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWarlock : EnemyAttack
{
    public GameObject indicatorPrefab;
    public float indicatorDuration = 1f;

    public GameObject laserPrefab;
    public float laserSpeed = 7f;

    public float cooldown = 2f;
    private bool canAttack = true;

    public AttackWarlock()
    {
        this.isRanged = true;
    }

    public override void InitiateAttack(Player player)
    {
        // straight blast
        if (!canAttack)
        {
            return;
        }
        
        GameObject indicator = Instantiate(indicatorPrefab, (transform.position + new Vector3(0,1,0)), Quaternion.identity);
        Destroy(indicator, indicatorDuration);

        StartCoroutine(GoLaser(player));
        startCooldown();
    }

    IEnumerator GoLaser(Player player)
    {
        yield return new WaitForSeconds(indicatorDuration);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.Euler(0f,0f, angle));
        laser.GetComponent<Rigidbody2D>().velocity = direction * laserSpeed;
        Destroy(laser, 2f);
    }

    private void startCooldown()
    {
        canAttack = false;
        Invoke("ResetCooldown", cooldown);
    }

    private void ResetCooldown()
    {
        canAttack = true;
    }
}
