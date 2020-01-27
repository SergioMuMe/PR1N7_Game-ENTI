﻿using System.Collections;
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
    public float jumpForce = 0.0f;

    public Rigidbody2D rb;

    private DirectionInputs direction = DirectionInputs.NONE;
    private bool jumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Input.GetAxis("Horizontal") < 0)
        {
            direction = DirectionInputs.LEFT;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            direction = DirectionInputs.RIGHT;
        }
        else
        {
            direction = DirectionInputs.NONE;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            rb.AddForce(Vector2.up * jumpForce);

            jumping = true;
        }

    }

    private void FixedUpdate()
    {

        float delta = Time.fixedDeltaTime * 1000;

        switch (direction)
        {
            case DirectionInputs.NONE:
                break;
            case DirectionInputs.RIGHT:
                rb.velocity = new Vector2(baseSpeed * Input.GetAxis("Horizontal"), rb.velocity.y);
                break;
            case DirectionInputs.LEFT:
                rb.velocity = new Vector2(baseSpeed * Input.GetAxis("Horizontal"), rb.velocity.y);
                break;
            default:
                break;
        }

        Debug.Log(rb.velocity);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(collision.contactCount/2).point.y < transform.position.y)
        {
            jumping = false;
        }
    }
}
