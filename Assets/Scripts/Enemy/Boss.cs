using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Boss : MonoBehaviour
{
    EnemyAttack attackScriptOne, attackScriptTwo, attackScriptThree;
    public Dictionary<string, float> weights;
    public Dictionary<string, float> results;
    
    private void Start()
    {
        Debug.Log("damages:" + AttackGoblin.damageFromGoblin + " playertogoblin:" + WeaponParent.damageToGoblin);
        weights.Add("Bat", 0.65f);
        weights.Add("Goblin", 0.7f);
        weights.Add("Creeper", 0.5f);
        weights.Add("Mage", 0.6f);
        weights.Add("Sorcerer", 0.8f);
        weights.Add("Warlock", 0.75f);
        ChooseAttacks();
    }

    private void ChooseAttacks()
    {
        float goblin = (AttackGoblin.damageFromGoblin * 1.5f + WeaponParent.damageToGoblin) / 2;
        float creeper = (AttackCreeper.damageFromCreeper * 1.5f +  WeaponParent.damageToCreeper) / 2;
        float bat = (AttackBat.damageFromBat * 1.5f + WeaponParent.damageToBat) / 2;
        float mage = (Mage_Missile.damageFromMage * 1.5f + WeaponParent.damageToMage) / 2;
        float sorcerer = (Blast.damageFromSorcerer * 1.5f + WeaponParent.damageToSorcerer) / 2;
        float warlock = (Laser.damageFromWarlock * 1.5f  + WeaponParent.damageToWarlock) / 2;


    }
}
