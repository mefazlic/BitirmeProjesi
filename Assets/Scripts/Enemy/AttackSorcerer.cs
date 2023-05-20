using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSorcerer : EnemyAttack
{
    public GameObject blastPrefab;
    public float blastRadius = 3f;

    public GameObject indicatorPrefab;
    public float indicatorDuration = 1f;

    public float cooldown = 3f;
    private bool canAttack = true;

    public override void InitiateAttack(Player player)
    {
        if (!canAttack)
        {
            return;
        }
        GameObject indicator = Instantiate(indicatorPrefab, player.transform.position, Quaternion.identity);
        Vector2 indicatorPos = indicator.transform.position;
        Destroy(indicator, indicatorDuration);

        StartCoroutine(SpawnBlastArea(indicatorPos));
        startCooldown();
    }

    IEnumerator SpawnBlastArea(Vector2 indicator)
    {
        // blast on player location
        yield return new WaitForSeconds(indicatorDuration);
        GameObject blastarea = Instantiate(blastPrefab, indicator, Quaternion.identity);
        blastarea.transform.localScale = new Vector3(blastRadius, blastRadius, 1f);
        Destroy(blastarea, 0.5f);
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
