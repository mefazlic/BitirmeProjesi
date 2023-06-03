using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }

    public Animator animator;
    public float delay = 0.3f;
    private bool attackBlocked;

    public Transform circleOrigin;
    public float radius;
    public int damageAmount = 3;

    public static int damageToCreeper;
    public static int damageToGoblin;
    public static int damageToMage;
    public static int damageToSorcerer;
    public static int damageToWarlock;

    public void Attack()
    {
        if (attackBlocked) { return; }
        animator.SetTrigger("Attack");
        attackBlocked = true;
        StartCoroutine(DelayAttack());
        attackBlocked = false;
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            //Debug.Log(collider.name);
            Health health;
            if (health = collider.GetComponent<Health>())
            {
                health.GetHit(damageAmount, transform.parent.gameObject);
                
                if (collider.CompareTag("Creeper"))
                {
                    damageToCreeper += damageAmount;
                }
                else if (collider.CompareTag("Goblin"))
                {
                    damageToGoblin += damageAmount;
                }
                else if (collider.CompareTag("Mage"))
                {
                    damageToMage += damageAmount;
                }
                else if (collider.CompareTag("Sorcerer"))
                {
                    damageToSorcerer += damageAmount;
                }
                else if (collider.CompareTag("Warlock"))
                {
                    damageToWarlock += damageAmount;
                }
            }
        }
    }
}
