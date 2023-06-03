using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private enum State
    {
        Normal,
        Rolling,
    }

    private State state;
    private WeaponParent weaponParent;

    LayerMask obstacleMask;
    Vector2 targetPos;
    Transform GFX, weapon;
    float flipGFX, flipWeapon;
    bool isMoving;
    public float speed;
    public float dashDistance;

    private void Start()
    {
        obstacleMask = LayerMask.GetMask("Wall", "Enemy");
        GFX = GetComponentInChildren<SpriteRenderer>().transform;
        weapon = GetComponentInChildren<WeaponParent>().transform;
        flipGFX = GFX.localScale.x;
        flipWeapon = weapon.localScale.x;
    }

    private void Awake()
    {
        state = State.Normal;
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    void Update()
    {
        ProcessInputs();
        if (state == State.Normal)
        {
            float horz = System.Math.Sign(Input.GetAxisRaw("Horizontal"));
            float vert = System.Math.Sign(Input.GetAxisRaw("Vertical"));

            if (Mathf.Abs(horz) > 0 || Mathf.Abs(vert) > 0)
            {
                if (Mathf.Abs(horz) > 0)
                {
                    GFX.localScale = new Vector2(flipGFX * horz, GFX.localScale.y);
                    weapon.localScale = new Vector2(flipWeapon * horz, weapon.localScale.y);
                }

                if (!isMoving)
                {
                    if (Mathf.Abs(horz) > 0)
                    {
                        targetPos = new Vector2(transform.position.x + horz, transform.position.y);
                    }
                    else if (Mathf.Abs(vert) > 0)
                    {
                        targetPos = new Vector2(transform.position.x, transform.position.y + vert);
                    }

                    Vector2 hitSize = Vector2.one * 0.8f;
                    Collider2D hit = Physics2D.OverlapBox(targetPos, hitSize, 0, obstacleMask);

                    if (!hit)
                    {
                        StartCoroutine(SmoothMove());
                    }
                }
            }
        }
    }

    IEnumerator SmoothMove()
    {
        isMoving = true;
        while (Vector2.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    void ProcessInputs()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weaponParent.Attack();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }

    void Dash()
    {
        if (!isMoving && state == State.Normal)
        {
            float horz = System.Math.Sign(Input.GetAxisRaw("Horizontal"));
            float vert = System.Math.Sign(Input.GetAxisRaw("Vertical"));

            if (Mathf.Abs(horz) > 0 || Mathf.Abs(vert) > 0)
            {
                targetPos = new Vector2(
                    transform.position.x + (Mathf.Abs(horz) > 0 ? horz * dashDistance : 0),
                    transform.position.y + (Mathf.Abs(vert) > 0 ? vert * dashDistance : 0)
                );

                // Check for obstacles before teleporting
                Vector2 hitSize = Vector2.one * 0.8f;
                Collider2D hit = Physics2D.OverlapBox(targetPos, hitSize, 0, obstacleMask);

                if (!hit)
                {
                    // Ensure there is enough distance from the enemy before teleporting
                    Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(targetPos, 0.5f, LayerMask.GetMask("Enemy"));

                    if (nearbyEnemies.Length == 0)
                    {
                        transform.position = targetPos;
                    }
                }
            }
        }
    }


}
