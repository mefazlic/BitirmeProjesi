using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage_Missile : MonoBehaviour
{
    public float missileTimer;
    public float missileSpeed;

    public int dmg = 3;
    public static int damageFromMage;

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
            damageFromMage += dmg;
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (missileTimer > 0f)
        {
            missileTimer -= Time.deltaTime;
            GameObject missile = GameObject.FindGameObjectWithTag("Missile");
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            if (missile != null || player != null)
            {
                Vector2 dir = (player.position - missile.transform.position).normalized;
                missile.GetComponent<Rigidbody2D>().velocity = dir * missileSpeed;
            }
        }
    }
}
