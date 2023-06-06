using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PlayerHealthBar : MonoBehaviour
{
    public float currentHealth, maxHealth;

    public UnityEvent<GameObject> onHitWithReference;
    public UnityEvent<GameObject> onDeathWithReference;

    public bool isDead = false;

    public Image healthBar;

    public void InitializeHealth(float healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void GetHit(float amount, GameObject sender)
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
    public void IncreaseHealth(float amount)
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
