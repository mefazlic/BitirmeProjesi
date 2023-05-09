using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGoblin : EnemyAttack
{

    public Vector2 dmgRange = new Vector2(1f, 5f);

    public override void InitiateAttack(Player player)
    {
        int roll = Random.Range(0, 100);
        if (roll > 50)
        {
            int dmgAmount = (int)Mathf.Ceil(Random.Range(dmgRange.x, dmgRange.y));
            Debug.Log(name + " attacked and hit for" + dmgAmount + "points of damage");
            Health health = player.GetComponent<Health>();
            health.GetHit(dmgAmount, transform.parent.gameObject);
        }
        else
        {
            Debug.Log(name + " attacked and missed");
        }
    }
}
