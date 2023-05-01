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

    public void Attack()
    {
        if (attackBlocked){ return; }
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
                health.GetHit(3, transform.parent.gameObject);
            }
        }
    }
}
