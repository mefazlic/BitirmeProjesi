using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int dmg = 4;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            Health health = player.GetComponent<Health>();
            health.GetHit(dmg, transform.gameObject);
            Destroy(gameObject);
        }
    }
}
