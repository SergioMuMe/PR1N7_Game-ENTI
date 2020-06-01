using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    public float vel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(vel, 0);
    }
}
