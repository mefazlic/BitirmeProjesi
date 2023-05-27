using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public UnityEvent<GameObject> onHitWithReference;
    public UnityEvent<GameObject> onDeathWithReference;

    public bool isDead = false;

    public string enemyName;
    private DamageTracker damageTracker;

    private void Start()
    {
        damageTracker = GameObject.FindObjectOfType<DamageTracker>();
        if (damageTracker == null)
        {
            Debug.LogError("DamageTracker component not found in the scene!");
        }
    }

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
            damageTracker.RegisterDamage(enemyName, amount); // Hasar miktarýný DamageTracker'a kaydet
            WriteDamageToFile(); // Hasar veri dosyasýný oluþtur
            Destroy(gameObject);
        }
    }

    private void WriteDamageToFile()
    {
        string fileName = enemyName + "DamageDone.txt";
        int damageDone = damageTracker.GetDamageData(enemyName);

        string filePath = Path.Combine(Application.dataPath, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(damageDone.ToString());
        }
    }
}
