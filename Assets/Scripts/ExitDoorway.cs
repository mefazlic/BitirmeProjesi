using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))] // this will add the Rigidbody2D and BoxCollider2D components to the GameObject if they are not already there

public class ExitDoorway : MonoBehaviour
{
    void Reset()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = Vector2.one;
        box.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
