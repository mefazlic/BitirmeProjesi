using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage_Missile : MonoBehaviour
{
    public int dmg = 4;
    public static int damageFromMage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null )
        {
            Health health = player.GetComponent<Health>();
            health.GetHit(dmg, transform.gameObject);
            damageFromMage += dmg;
            Destroy(gameObject);
        }
    }
}
