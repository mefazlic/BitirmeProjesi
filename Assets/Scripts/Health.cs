using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int currentHealth, maxHealth;

    public UnityEvent<GameObject> onHitWithReference;
    public UnityEvent<GameObject> onDeathWithReference;

    public bool isDead = false;

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead)
        {
            return;
        }
        if (sender.layer == gameObject.layer)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            onHitWithReference?.Invoke(sender);
        }
        else
        {
            onDeathWithReference?.Invoke(sender);
            isDead = true;
            Destroy(gameObject);
        }
    }
    public void IncreaseHealth(int amount)
    {
        if (currentHealth + amount > maxHealth) 
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }
    }
}
