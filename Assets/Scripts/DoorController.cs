using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public DIRECTION direction;
    public enum DIRECTION { UP, DOWN, RIGHT, LEFT };

    public float freqDoor;
    private float freqDoorActual;
    public bool activated;


    public float delta;
    public float timeDoor;


    public Vector2 posA;
    public Vector2 posB;
    public float time;


    public void moveDoor (Vector2 posA, Vector2 posB, float t)
    {
        transform.position = Vector2.Lerp(posA, posB, t);
    }
    
    void Start()
    {
        posA = transform.position;
        Debug.Log(posA);
    }

    
    void FixedUpdate()
    {
        
    }

    
    void Update()
    {
        if (activated)
        {
            delta = Time.deltaTime;
            Debug.Log("Puerta activada");
            timeDoor += delta;


            moveDoor(posA, posB, timeDoor);
            if (timeDoor >= freqDoor)
            {
                Debug.Log("Puerta en movimiento");
                timeDoor = 0;               
            }
        }

    }
}
