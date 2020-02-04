using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController_Roger : MonoBehaviour
{
    enum DirectionInputs
    {
        NONE,
        RIGHT,
        LEFT
    }

    public float baseSpeed = 0.0f;
    public float jumpSpeed = 0.0f;
    public float jumpForce = 0.0f;

    public float maxVelocity = 0.0f;

    public Rigidbody2D rb;

    public float groundedPrecision = 0.0f;
    private float groundDistance = 0.0f;

    private DirectionInputs direction = DirectionInputs.NONE;
    private RaycastHit2D hitGround;

    private bool rightCollider = false;
    private bool leftCollider = false;

    public bool isJumping = true;
    public bool isFalling = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        groundDistance = GetComponent<Collider2D>().bounds.extents.y;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            direction = DirectionInputs.LEFT;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            direction = DirectionInputs.RIGHT;
        }
        else
        {
            direction = DirectionInputs.NONE;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

    }

    private void FixedUpdate()
    {

        float delta = Time.fixedDeltaTime * 1000;

        hitGround = Physics2D.Raycast(transform.position, -Vector2.up, groundDistance + groundDistance);

        if (hitGround && isJumping)
        {
            isJumping = false;
        }
        else if (!hitGround && !isJumping)
        {
            isJumping = true;
        }

        if (rb.velocity.x > maxVelocity)
        {
            rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxVelocity)
        {
            rb.velocity = new Vector2(-maxVelocity, rb.velocity.y);
        }

        switch (direction)
        {
            case DirectionInputs.NONE:
                break;
            case DirectionInputs.RIGHT:                
                if (!isJumping)
                {
                    rb.AddForce(Vector2.right * baseSpeed, ForceMode2D.Force);
                }
                else
                {
                    rb.AddForce(Vector2.right * jumpSpeed, ForceMode2D.Force);
                }
                break;
            case DirectionInputs.LEFT:                
                if (!isJumping)
                {
                    rb.AddForce(Vector2.left * baseSpeed, ForceMode2D.Force);
                }
                else
                {
                    rb.AddForce(Vector2.left * jumpSpeed, ForceMode2D.Force);
                }
                break;
            default:
                break;
        }

    }
}

//void Plataforma()
//{
//    if (plataforma > max || plataforma < min)
//    {
//        speed = -seed;
//    }
//}
