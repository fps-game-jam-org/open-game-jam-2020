using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public int maxHealth = 10;
    public int maxHunger = 100;
    public float forwardSpeed = 4.0f;
    public float slowSpeed = 1.0f;
    public float upwardSpeed = 3.0f;
    public float diveSpeed = 6.0f;
    public float hungerFrequency = 1.0f;
    public int health { get { return currentHealth; } }
    public int hunger { get { return currentHunger; } }
    private int currentHealth;
    private int currentHunger = 0;
    private float horizontal;
    private float vertical;
    private bool movementEnabled = true;
    private float upwardTimer;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        upwardTimer = hungerFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        if (this.movementEnabled)
        {
            Vector2 position = rigidbody2d.position;
            position.x += ( horizontal > 0.0f ? forwardSpeed : slowSpeed ) * horizontal * Time.deltaTime;
            position.y += ( vertical > 0.0f ? upwardSpeed : diveSpeed ) * vertical * Time.deltaTime;
            rigidbody2d.MovePosition(position);
            animator.SetFloat("Move X", horizontal);
            animator.SetFloat("Move Y", vertical);

            if (vertical > 0.0f)
            {
                upwardTimer -= Time.deltaTime;
                if (upwardTimer < 0)
                {
                    upwardTimer = hungerFrequency;
                    ChangeHunger(1);
                }
            }
        }
    }

    void ChangeHealth( int amount )
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    void ChangeHunger( int amount )
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger);
        Debug.Log("Hunger: " + currentHunger + "/" + maxHunger);
    }

    public void EnableMovement()
    {
        this.movementEnabled = true;
    }

    public void DisableMovement()
    {
        this.movementEnabled = false;
    }
}
