using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public int healthAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerH = collision.GetComponent<Health>();

            if (playerH != null)
            {
                playerH.IncreaseHealth(healthAmount);
            }

            Destroy(gameObject);
        }
    }
}
