using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlatformV2 : MonoBehaviour
{
    
    public float speed;
    private Rigidbody2D rb;
    private bool going = true;
    private float fixedDelta;
    private float timePlatform;
    public int i = 0;
    public Vector2[] pos;
    public int posSize;

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
                movePlatform(pos[i], pos[i + 1], timePlatform);
            }
            else if (timePlatform >= 2)
            {
                timePlatform = 0;
                i++;               
            }
            if (i >= posSize - 1)
            {
                going = false;
            }

        }
        if (!going)
        {
            if (timePlatform <= 1)
            {
                movePlatform(pos[i], pos[i-1], timePlatform);
            }
            else if (timePlatform >= 2)
            {
                timePlatform = 0;
                i--;
            }
            if (i <= 0)
            {
                going = true;
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
