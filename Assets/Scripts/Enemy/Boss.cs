using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    
    private void Start()
    {
        Debug.Log("damages:" + AttackGoblin.damageFromGoblin + " playertogoblin:" + WeaponParent.damageToGoblin);    
    }
}
