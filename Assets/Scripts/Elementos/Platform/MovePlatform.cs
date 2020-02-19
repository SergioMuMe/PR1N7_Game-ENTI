using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : InterfaceGame
{
    
    //controla la velocidad a la que se mueve la plataforma entre punto y punto (100 es algo lento, ir provando valores a partir de ahi)
    public float speed;
    private Rigidbody2D rb;
    private bool going = true;
    private float deltaSpeed;
    public GameObject[] point;
    public bool activated;
    private Vector2 direction;
    private Vector2 tempPos;
    private float magnitude1;
    private float magnitude2;
    public float delayRate;
    private float actualDelay;

    //durante la ejecucion la I indica la ultima posicion de la array por el que se ha pasado
    public int i;
    private Vector2[] pos;

    void Start()
    {
        i = 0;
        //nos aseguramos de que el rigidbody sea kinematic
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        int size = point.Length;
        pos = new Vector2[size];
        point[0] = gameObject;

        for (int i = 0; i < size; i++)
        {
            pos[i] = point[i].transform.position;
        }   
    }


    void FixedUpdate()
    {
        if(!activated && rb.velocity != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
        if (activated)
        {
            deltaSpeed = Time.fixedDeltaTime * speed;

            if (going)
            {
                direction = pos[i + 1] - pos[i];
                magnitude1 = direction.magnitude;
                tempPos = transform.position;
                direction = tempPos - pos[i];
                magnitude2 = direction.magnitude;
                if (magnitude1 <= magnitude2 && rb.velocity != Vector2.zero)
                {
                    rb.velocity = Vector2.zero;
                    actualDelay = Time.time + delayRate;
                    i++;
                }
                else if (actualDelay<Time.time && rb.velocity == Vector2.zero)
                {            
                    movePlatform(pos[i], pos[i + 1], deltaSpeed);   
                }
                if (i >= pos.Length - 1)
                {
                    going = false;
                }
            }
            else if (!going)
            {
                direction = pos[i - 1] - pos[i];
                magnitude1 = direction.magnitude;
                tempPos = transform.position;
                direction = tempPos - pos[i];
                magnitude2 = direction.magnitude;

                if (magnitude1 <= magnitude2 && rb.velocity != Vector2.zero)
                {
                    rb.velocity = Vector2.zero;
                    actualDelay = Time.time + delayRate;
                    i--;
                }
                else if(actualDelay < Time.time && rb.velocity == Vector2.zero)
                {
                    movePlatform(pos[i], pos[i - 1], deltaSpeed);
                }
                if (i <= 0)
                {
                    going = true;
                }
            }
        }
    }


    void Update()
    {


    }

    public void movePlatform(Vector2 posA, Vector2 posB, float t)
    {
        direction = posB - posA;
        direction.Normalize();
        rb.velocity = direction * t;
    }

    public override void Activate()
    {
        if (!activated)
        {
            activated = true;
        }
        else if (activated)
        {
            activated = false;
        }
    }
}
