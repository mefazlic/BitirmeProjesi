using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using UnityEngine;

public class Boss : MonoBehaviour
{
    EnemyAttack[] attackScripts;
    public Dictionary<string, float> weights = new Dictionary<string, float>();
    public Dictionary<string, float> results = new Dictionary<string, float>();

    private void Start()
    {
        Debug.Log("damages:" + AttackGoblin.damageFromGoblin + " playertogoblin:" + WeaponParent.damageToGoblin);
        /*
        weights.Add("Bat", 0.65f);
        weights.Add("Goblin", 0.7f);
        weights.Add("Creeper", 0.5f);
        weights.Add("Mage", 0.6f);
        weights.Add("Sorcerer", 0.8f);
        weights.Add("Warlock", 0.75f);
        */
        ChooseAttacks();
    }

    private void ChooseAttacks()
    {
        /*
        results.Add("Goblin",(AttackGoblin.damageFromGoblin * 1.5f - WeaponParent.damageToGoblin) / 2 * weights["Goblin"]);
        results.Add("Creeper", (AttackCreeper.damageFromCreeper * 1.5f - WeaponParent.damageToCreeper) / 2 * weights["Creeper"]);
        results.Add("Bat", (AttackBat.damageFromBat * 1.5f - WeaponParent.damageToBat) / 2 * weights["Bat"]);
        results.Add("Mage", (Mage_Missile.damageFromMage * 1.5f - WeaponParent.damageToMage) / 2 * weights["Mage"]);
        results.Add("Sorcerer", (Blast.damageFromSorcerer * 1.5f - WeaponParent.damageToSorcerer) / 2 * weights["Sorcerer"]);
        results.Add("Warlock", (Laser.damageFromWarlock * 1.5f  - WeaponParent.damageToWarlock) / 2 * weights["Warlock"]);

        results.OrderByDescending(x => x.Value);
        results.Remove(results.Keys.Last());

        print(results);
        */
        results.Add("Goblin", 5);
        results.Add("Sorcerer", 7);
        results.Add("Creeper", 3);
        results.Add("Mage", 12);
        results.Add("Warlock", 9);
        results.Add("Bat", 4);
        results.OrderByDescending(x => x.Value);

        foreach (var result in results)
        {
            print(result.Key + ": " + result.Value);
            // attackScripts.
        }

    }
}
