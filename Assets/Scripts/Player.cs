using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private enum State
    {
        Normal,
        Rolling,
    }

    public float speed = 6f;
    public float dashSize = 3f;
    public LayerMask dashLayerMask;
    public Rigidbody2D rb;
    private Vector3 moveDirection;
    private Vector3 rollDirection;
    private float rollSpeed;
    private bool isDashButtonDown;
    private State state;
    private WeaponParent weaponParent;

    private void Awake()
    {
        state = State.Normal;
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    private void Update()
    {
        // input processing
        switch (state)
        {
            case State.Normal:
                ProcessInputs();
                break;
            case State.Rolling:
                float rollSpeedDrop = 2f;
                rollSpeed -= rollSpeed * rollSpeedDrop * Time.deltaTime;

                float rollSpeedMin = 8f;
                if (rollSpeed < rollSpeedMin)
                {
                    state = State.Normal;
                }
                break;
        }
    }
    private void FixedUpdate()
    {
        // physics
        switch (state)
        {
            case State.Normal:
                turnFace();
                Move();
                Dash();
                break;
            case State.Rolling:
                rb.velocity = rollDirection * rollSpeed;
                break;
        }
    }

    void ProcessInputs()
    {
        float Xaxis = Input.GetAxisRaw("Horizontal");
        float Yaxis = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(Xaxis, Yaxis).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDashButtonDown = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rollDirection = moveDirection;
            rollSpeed = 20f;
            state = State.Rolling;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Attack();
        }
    }
    void Move()
    {
        rb.velocity = moveDirection * speed;
    }

    void Attack()
    {
        weaponParent.Attack();
    }

    void Dash()
    {
        if (isDashButtonDown)
        {
            Vector3 dashPosition = transform.position + moveDirection * dashSize;
            RaycastHit2D rch2d = Physics2D.Raycast(transform.position, moveDirection, dashSize, dashLayerMask);
            if (rch2d.collider != null)
            {
                dashPosition = rch2d.point;
            }
            rb.MovePosition(dashPosition);
            isDashButtonDown = false;
        }
    }

    void turnFace()
    {
        if (moveDirection.x != 0)
        {
            transform.localScale = new Vector2(1 * moveDirection.x, transform.localScale.y);
        }
    }

}