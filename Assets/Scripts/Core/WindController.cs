using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    [Tooltip("Force of the wind")]
    public float force;

    BoxCollider2D windCollider;

    // Start is called before the first frame update
    void Start()
    {
        windCollider = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D[] bodies = GameObject.FindObjectsOfType<Rigidbody2D>();

        foreach (Rigidbody2D body in bodies)
        {
            BoxCollider2D bodyCollider = body.gameObject.GetComponent<BoxCollider2D>();

            //check if it is intersecting this wind game objects collider
            if (bodyCollider.bounds.Intersects(windCollider.bounds))
            {
                body.AddForce(transform.up * force);
            }
        }

    }
}
