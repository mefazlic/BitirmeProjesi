using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    public int dmg = 5;
    public static int damageFromSorcerer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            Health health = player.GetComponent<Health>();
            health.GetHit(dmg, transform.gameObject);
            damageFromSorcerer += dmg;
        }
    }
}
