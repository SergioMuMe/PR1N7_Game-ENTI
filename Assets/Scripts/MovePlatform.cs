using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : InterfaceGame
{
    
    //controla la velocidad a la que se mueve la plataforma entre punto y punto (si le damos un valores altos [por encima de 1] puede que de problemas)
    public float speed;
    private Rigidbody2D rb;
    private bool going = true;
    private float fixedDelta;
    private float deltaSpeed;
    public GameObject[] point;
    public bool activated;


    //aun no entiendo 100% como va, pero con delay 1, las plataformas no dejan de moverse, con delay > 1 se paran durante un tiempo cada vez que llegan a un punto de la array
    public float delay = 2;
    //durante la ejecucion la I indica la ultima posicion de la array por el que se ha pasado
    public int i = 0;
    private Vector2[] pos;

    void Start()
    {
        //nos aseguramos de que el rigidbody sea kinematic
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        int size = point.Length;
        pos = new Vector2[size];

        for (int i = 0; i < size; i++)
        {
            pos[i] = point[i].transform.position;
        }

    }


    void FixedUpdate()
    {
        if (activated)
        {
            fixedDelta = Time.fixedDeltaTime;
            deltaSpeed += fixedDelta * speed;

            if (going)
            {
                if (deltaSpeed <= 1)
                {
                    movePlatform(pos[i], pos[i + 1], deltaSpeed);
                }
                else if (deltaSpeed >= delay)
                {
                    deltaSpeed = 0;
                    i++;
                }
                if (i >= pos.Length - 1)
                {
                    going = false;
                }
            }
            else if (!going)
            {
                if (deltaSpeed <= 1)
                {
                    movePlatform(pos[i], pos[i - 1], deltaSpeed);
                }
                else if (deltaSpeed >= delay)
                {
                    deltaSpeed = 0;
                    i--;
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
        /*
        dir = posB - posA
        dir.normalize();
        rb2d.velocity = dir*speed
        */
        Vector2 newPosition = Vector2.Lerp(posA, posB, t);
        rb.MovePosition(newPosition);
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
