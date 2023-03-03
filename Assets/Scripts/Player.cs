using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    Vector2 targetPos;
    Vector2 hitSize = Vector2.one * 0.8f;

    Transform GFX;

    LayerMask obstacleMask;

    float flipX;
    bool isMoving;

    void Start()
    {
        obstacleMask = LayerMask.GetMask("Wall", "Enemy"); // Get the layer mask of the Wall and Enemy layer
        GFX = GetComponentInChildren<SpriteRenderer>().transform; // Get the child object that have the SpriteRenderer component and get its transform
        flipX = GFX.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Debug.Log(horizontalInput + ", " + verticalInput); // -1, 0

        if (!isMoving)
        {
            faceTurn(horizontalInput);

            if (horizontalInput != 0 || verticalInput != 0)
            {
                targetPos = new Vector2(transform.position.x + horizontalInput, transform.position.y + verticalInput);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
            }

            Collider2D hit = Physics2D.OverlapBox(targetPos, hitSize, obstacleMask);
            if (hit != null)
            {
                StartCoroutine(SmoothMove());
            }

        }
    }

    void faceTurn(float horizontalInput)
    {
        if (horizontalInput != 0)
        {
            GFX.localScale = new Vector2(flipX * horizontalInput, GFX.localScale.y);
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



}