using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCreeper : EnemyAttack
{
    public int dmg = 7;
    public static int damageFromCreeper;
    public override void InitiateAttack(Player player)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;

        StartCoroutine(Explode(player));
    }

    IEnumerator Explode(Player player)
    {
        yield return new WaitForSeconds(1.5f);
        //oncollisionenter2d ile kontrol etmek lazým
        PlayerHealthBar playerHealthBar = player.GetComponent<PlayerHealthBar>();
        playerHealthBar.GetHit(dmg, transform.parent.gameObject);
        damageFromCreeper += dmg;
        Destroy(gameObject);
    }
}
