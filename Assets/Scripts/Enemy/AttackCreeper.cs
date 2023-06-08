using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCreeper : EnemyAttack
{
    public int dmg = 7;
    public static int damageFromCreeper;
    public GameObject blastPrefab;
    public float radius = 3f;

    public override void InitiateAttack(Player player)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;

        StartCoroutine(Explode(player));
    }

    IEnumerator Explode(Player player)
    {
        yield return new WaitForSeconds(1f);
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius))
        {
            Health health;
            if (health = collider.GetComponent<Health>())
            {
                health.GetHit(dmg, transform.parent.gameObject);
            }
            PlayerHealthBar playerHealth;
            if (playerHealth = collider.GetComponent<PlayerHealthBar>())
            {
                playerHealth.GetHit(dmg, transform.parent.gameObject);
                damageFromCreeper += dmg;
            }
        }
        GameObject blastArea = Instantiate(blastPrefab, transform.position, Quaternion.identity);
        blastArea.transform.localScale = new Vector3(radius, radius, 1f);
        Destroy(blastArea, 0.5f);
        if (gameObject.tag != "Boss") { Destroy(gameObject); }
    }    
}
