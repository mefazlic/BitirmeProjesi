using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    private void Update()
    {
        // input processing
        ProcessInputs();
    }
    private void FixedUpdate()
    {
        // physics
        Move();
    }

    void ProcessInputs()
    {
        float Xaxis = Input.GetAxisRaw("Horizontal");
        float Yaxis = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(Xaxis, Yaxis).normalized;
    }
    void Move()
    {
        turnFace();
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

    void turnFace()
    {
        if (moveDirection.x != 0)
        {
            transform.localScale = new Vector2(1 * moveDirection.x, transform.localScale.y);
        }
    }
}