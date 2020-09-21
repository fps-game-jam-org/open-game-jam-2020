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
    public float forwardSpeed = 10.0f;

    [Tooltip("Float variable to control backwards movement")]
    public float slowSpeed = 2.0f;

    [Tooltip("Float variable to control how quickly the bird ascends")]
    public float upwardSpeed = 20.0f;

    [Tooltip("Float variable to control how quickly the bird descends")]
    public float diveSpeed = 30.0f;

    [Tooltip("Float variable that specifies the number of seconds of upward movement"
                + "required to increase hunger")]
    public float hungerFrequency = 1.0f;

    [Tooltip("Int variable that specifies what hunger increments by during upward movement")]
    public int hungerDelta = 2;

    [Tooltip("Float variable that determines how long to hold backwards before turning around")]
    public float flipDuration = 1.0f;

    [Tooltip("Max velocity variable")]
    public float maxVelocity = 20.0f;

    [Tooltip("How much health is lost at max hunger")]
    public int hungerHealthLoss = -1;

    [Tooltip("Time (seconds) until you can take damage again")]
    public float healthInvincDuration = 1.0f;

    public int health { get { return currentHealth; } }
    public int hunger { get { return currentHunger; } }
    private int currentHealth;
    private int currentHunger = 0;
    private float horizontal;
    private float vertical;
    private bool movementEnabled = true;
    private float upwardTimer;
    private float flipTimer;
    private float healthInvincTimer;
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private bool flipped_f = false;
    private bool invincible_f = false;

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
        if(invincible_f)
        {
            healthInvincTimer -= Time.deltaTime;
            if (healthInvincTimer <= 0.0f)
            {
                healthInvincTimer = healthInvincDuration;
                invincible_f = false;
            }
        }
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
        rigidbody2d.AddForce(new Vector2((horizontal > 0.0f ? forwardSpeed : slowSpeed) * horizontal, 
                                (vertical > 0.0f ? upwardSpeed : diveSpeed) * vertical));
        rigidbody2d.velocity = Vector2.ClampMagnitude(rigidbody2d.velocity, maxVelocity);
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


    public void ChangeHealth(int amount)
    {
        if (!invincible_f)
        {
            healthInvincTimer = healthInvincDuration;
            invincible_f = true;
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            Debug.Log("Health: " + currentHealth + "/" + maxHealth);
            if (currentHealth <= 0)
            {
                Debug.Log("Health has dropped to 0, death awaits you");
                characterScale.y = -1.0f * characterScale.y;
                transform.localScale = characterScale;
                DisableMovement();
                Destroy(gameObject, 3);
            }
        }
    }

    void ChangeHunger(int amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger);
        if(currentHunger >= maxHunger)
        {
            Debug.Log("Losing health from hungher");
            ChangeHealth(hungerHealthLoss);
        }
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
