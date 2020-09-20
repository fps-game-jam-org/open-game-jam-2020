using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [Tooltip("Simple integer for Max Health")]
    public int maxHealth = 10;

    [Tooltip("Simple integer for Max Hunger")]
    public int maxHunger = 100;

    [Tooltip("Float variable to control speed for moving forward")]
    public float forwardSpeed = 4.0f;

    [Tooltip("Float variable to control backwards movement")]
    public float slowSpeed = 1.0f;

    [Tooltip("Float variable to control how quickly the bird ascends")]
    public float upwardSpeed = 3.0f;

    [Tooltip("Float variable to control how quickly the bird descends")]
    public float diveSpeed = 6.0f;

    [Tooltip("Float variable that specifies the number of seconds of upward movement"
                + "required to increase hunger")]
    public float hungerFrequency = 1.0f;

    [Tooltip("Int variable that specifies what hunger increments by during upward movement")]
    public int hungerDelta = 2;

    [Tooltip("Float variable that determines how long to hold backwards before turning around")]
    public float flipDuration = 1.0f;

    public int health { get { return currentHealth; } }
    public int hunger { get { return currentHunger; } }
    private int currentHealth;
    private int currentHunger = 0;
    private float horizontal;
    private float vertical;
    private bool movementEnabled = true;
    private float upwardTimer;
    private float flipTimer;
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private bool flipped_f = false;

    private Vector3 characterScale;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        upwardTimer = hungerFrequency;
        flipTimer = flipDuration;
        characterScale = transform.localScale;
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
            calcMovement(horizontal, vertical);
            calcHunger(horizontal, vertical);
            calcFlip(horizontal, vertical);
        }
    }

    private void calcMovement(float horizontal, float vertical)
    { 
        animator.SetFloat("Move X", flipped_f ? -horizontal : horizontal);
        animator.SetFloat("Move Y", vertical);
        
        //apply force to rigid body 2d
        rigidbody2d.AddForce(new Vector2((horizontal > 0.0f ? forwardSpeed : slowSpeed) * horizontal, (vertical > 0.0f ? upwardSpeed : diveSpeed) * vertical));
    }

    private void calcHunger(float horizontal, float vertical)
    {
        if (vertical > 0.0f)
        {
            upwardTimer -= Time.deltaTime;
            if (upwardTimer < 0.0f)
            {
                upwardTimer = hungerFrequency;
                ChangeHunger(hungerDelta);
            }
        }
    }

    private void calcFlip(float horizontal, float vertical)
    {
        float tempSwap;
        if (flipped_f == false && horizontal < 0.0f
                || flipped_f == true && horizontal > 0.0f)
        {
            flipTimer -= Time.deltaTime;
            if (flipTimer < 0.0f)
            {
                characterScale.x = -1.0f * characterScale.x;
                transform.localScale = characterScale;
                flipped_f = flipped_f ? false : true;
                tempSwap = forwardSpeed;
                forwardSpeed = slowSpeed;
                slowSpeed = tempSwap;
                flipTimer = flipDuration;
            }
        }
        else if (flipTimer != flipDuration)
        {
            flipTimer = flipDuration;
        }
    }


    void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    void ChangeHunger(int amount)
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
