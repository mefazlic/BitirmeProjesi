using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using UnityEngine;

public class Boss : MonoBehaviour
{
    EnemyAttack[] attackScripts = new EnemyAttack[3];
    public Dictionary<string, float> weights = new Dictionary<string, float>();
    public Dictionary<string, float> results = new Dictionary<string, float>();

    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
       
        weights.Add("Goblin", 0.7f);
        weights.Add("Creeper", 0.5f);
        weights.Add("Bat", 0.65f);
        weights.Add("Mage", 0.6f);
        weights.Add("Sorcerer", 0.8f);
        weights.Add("Warlock", 0.75f);
        ChooseAttacks();
    }

    private void ChooseAttacks()
    {
        results.Add("Goblin",(AttackGoblin.damageFromGoblin * 1.5f - WeaponParent.damageToGoblin * 0.25f) / 2 * weights["Goblin"]);
        results.Add("Creeper", (AttackCreeper.damageFromCreeper * 1.5f - WeaponParent.damageToCreeper * 0.25f) / 2 * weights["Creeper"]);
        results.Add("Bat", (AttackBat.damageFromBat * 1.5f - WeaponParent.damageToBat * 0.25f) / 2 * weights["Bat"]);
        results.Add("Mage", (Mage_Missile.damageFromMage * 1.5f - WeaponParent.damageToMage * 0.25f) / 2 * weights["Mage"]);
        results.Add("Sorcerer", (Blast.damageFromSorcerer * 1.5f - WeaponParent.damageToSorcerer * 0.25f) / 2 * weights["Sorcerer"]);
        results.Add("Warlock", (Laser.damageFromWarlock * 1.5f  - WeaponParent.damageToWarlock * 0.25f) / 2 * weights["Warlock"]);
        foreach (var item in results) { print(item.Key+ ": " + item.Value); }
        var orderedResults = results.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        var index = 0;
        foreach (var result in orderedResults)
        {
            print(result.Key + ": " + result.Value);
            if (result.Key == "Goblin") { attackScripts[index] = gameObject.GetComponent<AttackGoblin>(); }
            else if (result.Key == "Creeper") { attackScripts[index] = gameObject.GetComponent<AttackCreeper>(); }
            else if (result.Key == "Bat") { attackScripts[index] = gameObject.GetComponent<AttackBat>(); }
            else if (result.Key == "Mage") { attackScripts[index] = gameObject.GetComponent<AttackMage>(); }
            else if (result.Key == "Sorcerer") { attackScripts[index] = gameObject.GetComponent<AttackSorcerer>(); }
            else if (result.Key == "Warlock") { attackScripts[index] =  gameObject.GetComponent<AttackWarlock>(); }
            index++;
            if (index == 3) { break; } 
        }
    }
}
