using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformOptions : MonoBehaviour
{
    public enum PLATFORMTYPE
    {
        PASSABLE, //plataforma atravessable desde abajo
        SOLID //plataforma no atravessable
    }
    public enum PLATFORMMOVEMENT
    {
        MOBILE, //plataforma movil (indicar puntos)
        STATIC //plataforma estatica
    }
    public PLATFORMTYPE platformType;
    public PLATFORMMOVEMENT platformMovement;

    private Rigidbody2D rb;
    private PlatformEffector2D pe;
    private BoxCollider2D bc;
    private MovePlatform mps;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pe = GetComponent<PlatformEffector2D>();
        bc = GetComponent<BoxCollider2D>();
        mps = GetComponent<MovePlatform>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (platformType)
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

        switch (platformMovement)
        {
            case PLATFORMMOVEMENT.STATIC:
                rb.bodyType = RigidbodyType2D.Static;
                mps.enabled = false;
                break;
            case PLATFORMMOVEMENT.MOBILE:
                rb.bodyType = RigidbodyType2D.Kinematic;
                mps.enabled = true;
                break;
        }
    }
}
