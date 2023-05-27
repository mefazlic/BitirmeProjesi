using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTracker : MonoBehaviour
{
    private Dictionary<string, int> enemyDamage = new Dictionary<string, int>();

    public void RegisterDamage(string enemyName, int damageAmount)
    {
        if (!enemyDamage.ContainsKey(enemyName))
        {
            enemyDamage.Add(enemyName, 0);
        }

        enemyDamage[enemyName] += damageAmount;
    }

    public int GetDamageData(string enemyName)
    {
        if (enemyDamage.ContainsKey(enemyName))
        {
            return enemyDamage[enemyName];
        }
        else
        {
            return 0;
        }
    }
}
