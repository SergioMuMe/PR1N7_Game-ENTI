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

    public Rigidbody2D rb;

    private DirectionInputs direction = DirectionInputs.NONE;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Input.GetAxis("Horizontal") < 0)
        {

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {

        }
        else
        {

        }

    }
}
