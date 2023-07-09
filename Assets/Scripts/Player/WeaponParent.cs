using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }

    public Animator animator;
    private bool attackBlocked;
    private int attackCount;

    public Transform circleOrigin;
    public float radius;
    public int damageAmount ;

    public static int damageToCreeper;
    public static int damageToGoblin;
    public static int damageToBat;
    public static int damageToMage;
    public static int damageToSorcerer;
    public static int damageToWarlock;

    private Coroutine resetCoroutine;

    public void Attack()
    {
        if (attackBlocked) { return; }

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        animator.SetTrigger("Attack");
        attackBlocked = true;

        attackCount++;
        if (attackCount >= 3)
        {
            StartCoroutine(DelayAttack());
            attackCount = 0;
        }
        else
        {
            attackBlocked = false;
        }
        resetCoroutine = StartCoroutine(ResetAttackCount());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(0.7f);
        attackBlocked = false;
    }

    private IEnumerator ResetAttackCount()
    {
        yield return new WaitForSeconds(2.0f);
        attackCount = 0;
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
                else if (collider.CompareTag("Bat"))
                {
                    damageToBat += damageAmount;
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
