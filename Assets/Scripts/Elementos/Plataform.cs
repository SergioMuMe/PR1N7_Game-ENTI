using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : InterfaceGame
{
    public enum PLATFORMTYPE
    {
        PASSABLE,
        SOLID
    }

    public enum PLATFORMMOVEMENT
    {
        STATIC,
        MOBILE,
        BOOL
    }

    public PLATFORMTYPE plataformType;
    public PLATFORMMOVEMENT plataformMovement;

    public float speed;
    private Rigidbody2D rb;
    private PlatformEffector2D pe;
    private Collider2D bc;
    private bool going = true;
    public Transform[] positions;
    public bool activated = false;
    private Vector2 direction;
    private float magnitude1;
    private float magnitude2;
    public float delayRate;
    private float actualDelay;

    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pe = GetComponent<PlatformEffector2D>();
        bc = GetComponent<Collider2D>();

        switch (plataformType)
        {
            case PLATFORMTYPE.SOLID:
                pe.enabled = false;
                bc.usedByEffector = false;
                break;
            case PLATFORMTYPE.PASSABLE:
                pe.enabled = true;
                bc.usedByEffector = true;
                break;
        }

        switch (plataformMovement)
        {
            case PLATFORMMOVEMENT.STATIC:
                rb.bodyType = RigidbodyType2D.Static;
                break;
            case PLATFORMMOVEMENT.MOBILE:
                rb.bodyType = RigidbodyType2D.Kinematic;
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (activated)
        {
            float deltaSpeed = Time.fixedDeltaTime * speed;

            if (going)
            {
                direction = positions[i + 1].position - positions[i].position;
                magnitude1 = direction.magnitude;
                direction = transform.position - positions[i].position;
                magnitude2 = direction.magnitude;

                if (magnitude1 <= magnitude2 && rb.velocity != Vector2.zero)
                {
                    rb.velocity = Vector2.zero;
                    actualDelay = Time.time + delayRate;
                    if (plataformMovement == PLATFORMMOVEMENT.BOOL)
                    {
                        activated = false;
                    }                    
                    i++;
                }
                else if (actualDelay < Time.time && rb.velocity == Vector2.zero)
                {
                    movePlatform(positions[i].position, positions[i + 1].position, deltaSpeed);
                }

                if (i >= positions.Length-1)
                {
                    going = false;
                }
            }
            else if (!going)
            {
                direction = positions[i - 1].position - positions[i].position;
                magnitude1 = direction.magnitude;
                direction = transform.position - positions[i].position;
                magnitude2 = direction.magnitude;

                if (magnitude1 <= magnitude2 && rb.velocity != Vector2.zero)
                {
                    rb.velocity = Vector2.zero;
                    actualDelay = Time.time + delayRate;
                    if (plataformMovement == PLATFORMMOVEMENT.BOOL)
                    {
                        activated = false;
                    }
                    i--;
                }
                else if (actualDelay < Time.time && rb.velocity == Vector2.zero)
                {
                    movePlatform(positions[i].position, positions[i - 1].position, deltaSpeed);
                }
                if (i <= 0)
                {
                    going = true;
                }
            }
        }
        else if (rb.velocity != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void movePlatform(Vector2 posA, Vector2 posB, float t)
    {
        direction = posB - posA;
        direction.Normalize();
        rb.velocity = direction * t;
    }

    public override void ChangeActivate()
    {
        activated = !activated;
    }

    public override void Activate()
    {
        activated = true;
    }

    public override void Deactivate()
    {
        activated = false;
    }
}
