using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class AttackGoblin : EnemyAttack
{

    public Vector2 dmgRange = new Vector2(1f, 5f);
    public static int damageFromGoblin;

    public override void InitiateAttack(Player player)
    {
        int roll = Random.Range(0, 100);
        if (roll > 50)
        {
            int dmgAmount = (int)Mathf.Ceil(Random.Range(dmgRange.x, dmgRange.y));
            PlayerHealthBar playerHealthBar = player.GetComponent<PlayerHealthBar>();
            playerHealthBar.GetHit(dmgAmount, transform.parent.gameObject);
            damageFromGoblin += dmgAmount;
        }
    }
}
