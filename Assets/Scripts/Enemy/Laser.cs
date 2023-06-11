using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int dmg = 6;
    public static int damageFromWarlock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            PlayerHealthBar playerHealthBar = player.GetComponent<PlayerHealthBar>();
            playerHealthBar.GetHit(dmg, transform.gameObject);
            damageFromWarlock += dmg;
            Destroy(gameObject);
        }
    }
}
