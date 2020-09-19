using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{

    public float speed = 0.25f;

    void FixedUpdate()
    {
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }


    /// <summary>
    /// Move up, down, left, right like a top-down game
    /// </summary>
    private void Move(float horizontal, float vertical)
    {
        Vector3 translationVector = new Vector3(horizontal, vertical);
        transform.Translate(speed*translationVector.normalized);
    }
}
