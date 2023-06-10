using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBat : EnemyAttack
{
    public Vector2 dmgRange = new Vector2(1f, 3f);
    public float duration = 2f;
    public static int damageFromBat;

    private bool isAttacking = false;

    public AttackBat()
    {
        this.isRanged = false;
    }

    public override void InitiateAttack(Player player)
    {
        if (!isAttacking)
        {
            int roll = Random.Range(0, 100);
            if (roll > 50)
            {
                StartCoroutine(Poison(player));
            }
        }
    }
    private IEnumerator Poison(Player player)
    {
        isAttacking = true;
        float timepassed = 0f;

        while (timepassed < duration)
        {
            int dmgAmount = (int)Mathf.Ceil(Random.Range(dmgRange.x, dmgRange.y));
            PlayerHealthBar playerHealthBar = player.GetComponent<PlayerHealthBar>();
            playerHealthBar.GetHit(dmgAmount * Time.deltaTime, transform.parent.gameObject);
            timepassed += Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
    }
}
