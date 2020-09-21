using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileSnake : MonoBehaviour
{
    [Tooltip("Float variable to control acceleration for moving forward")]
    public float acceleration = 10.0f;

    [Tooltip("Max velocity variable")]
    public float maxVelocity = 20.0f;

    [Tooltip("Float variable that determines how long until the snake turns around")]
    public float turnaroundTime = 10.0f;

    [Tooltip("How much damage is done ( e.g. -1 will reduce birb health by 1")]
    public int damage = -1;

    private float horizontal;
    private float flipTimer;
    private bool movementEnabled = true;
    private Rigidbody2D rigidbody2d;
    private bool flipped_f = false;
    private float baseWidth;
    private float baseHeight;

    private SpriteRenderer sprRend;
    private Vector3 characterScale;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        characterScale = transform.localScale;
        flipTimer = turnaroundTime;
        sprRend = GetComponent<SpriteRenderer>();
        baseWidth = sprRend.size.x;
        baseHeight = sprRend.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = flipped_f ? -1.0f : 1.0f;
    }

    void FixedUpdate()
    {
        if (this.movementEnabled)
        {
            calcMovement(horizontal);
            calcFlip();
        }
    }

    private void calcMovement(float horizontal)
    {
        //apply force to rigid body 2d
        rigidbody2d.AddForce(new Vector2(acceleration * horizontal, 0));
        rigidbody2d.velocity = Vector2.ClampMagnitude(rigidbody2d.velocity, maxVelocity);
        sprRend.size = new Vector2(baseWidth + Mathf.Pow( Mathf.Sin( Time.fixedTime ), 2) * baseWidth, baseHeight + Mathf.Pow(Mathf.Cos(Time.fixedTime), 2) * (baseHeight/2));
    }

    private void calcFlip()
    {
        flipTimer -= Time.deltaTime;
        if (flipTimer < 0.0f)
        {
            characterScale.x = -1.0f * characterScale.x;
            transform.localScale = characterScale;
            flipped_f = flipped_f ? false : true;
            flipTimer = turnaroundTime;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        BirdController player = other.gameObject.GetComponent<BirdController>();

        if (player != null)
        {
            player.ChangeHealth(damage);
        }
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
