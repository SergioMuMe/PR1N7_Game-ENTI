using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovePlatform : MonoBehaviour
{
    public Vector2[] pos;
    public float speed;
    private Rigidbody2D rb;
    private bool going = true;
    private float fixedDelta;
    private float timePlatform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }


    void FixedUpdate()
    {
        fixedDelta = Time.fixedDeltaTime;
        timePlatform += fixedDelta * speed;
        if (going)
        {
            if (timePlatform <= 1)
            {
                movePlatform(pos[0], pos[1], timePlatform);
            }
            if(timePlatform >= 2)
            {
                going = false;
                timePlatform = 0;
            }
        }

        if (!going)
        {

            if (timePlatform <= 1)
            {
                movePlatform(pos[1], pos[0], timePlatform);
            }
            if (timePlatform >= 2)
            {
                going = true;
                timePlatform = 0;
            }
        }

    }


    void Update()
    {
        

    }

    public void movePlatform(Vector2 posA, Vector2 posB, float t)
    {
        Vector2 newPosition = Vector2.Lerp(posA, posB, t);
        rb.MovePosition(newPosition);
    }
}
