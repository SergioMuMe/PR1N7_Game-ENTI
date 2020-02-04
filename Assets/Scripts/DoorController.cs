using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    // tipo de puerta
    public enum DOORTYPE
    {
        BOOLEAN, // La puerta está abierta o cerrada
        FREQUENCY // La puerta se abre y cierra con una frecuencia.
    };
    public DOORTYPE doorType;

    public GameObject visagra;

    // controlamos cuando se activa el movimiento de la puerta
    public bool activated;

    // Velocidad de movimiento.  0,X lento > 1 normal <  1,X 2,X 3,X rápido
    public float backwardSpeed;
    public float forwardSpeed;

    // valor maximo en segundos cada cuanto se activa el movimiento de la puerta. 
    // Del 0 al 1, se aplica movimiento, a partir de 1, es tiempo en espera hasta que se reinicia el cambio de sentido.
    public float timeLapsed;
    public float waitUntilForward;
    public float waitUntilBackward;

    public Vector2 posA;
    public Vector2 posB;

    // #---------------------------#
    // | INTERNAL SCRIPT VARIABLES |
    // #---------------------------#

    private Rigidbody2D rb2D;
    private float fixedDelta;

    // Indices de la función Lerp(Vector2, Vector2, I)....Donde I tiene un valor entre 0 y 1.
    // El Indice I le asignamos un valor Time para que vaya a una velocidad dependiente del tiempo(fixedDelta) * multiplicador(speed)
    private float forwardI;
    private float backwardI;

    private enum DIRECTION { FORWARD, BACKWARD };
    private DIRECTION direction;

    private bool boolDoor;
    private bool finishedBoolAperture;


    public void moveDoor(Vector2 posA, Vector2 posB, float t)
    {
        Vector2 newPosition = Vector2.Lerp(posA, posB, t);
        rb2D.MovePosition(newPosition);
    }

    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        //FAST TESTING VALUES
        waitUntilForward = 1.5f;
        waitUntilBackward = 1.5f;
        forwardSpeed = 1f;
        backwardSpeed = 1f;
        doorType = DOORTYPE.BOOLEAN;
        finishedBoolAperture = false;

        //TODO: Candidatos a ser cosas publicas para configurar via script desde otros sitios
        posA = transform.position;
        direction = DIRECTION.FORWARD;
    }

    void revertMovement()
    {
        timeLapsed = 0;
        forwardI = 0;
        backwardI = 0;
        direction = (direction == DIRECTION.FORWARD ? DIRECTION.BACKWARD : DIRECTION.FORWARD) ;
    }

    void FixedUpdate()
    {

        if (activated)
        {
            fixedDelta = Time.fixedDeltaTime;
            
            forwardI += fixedDelta * forwardSpeed;
            backwardI += fixedDelta * backwardSpeed;
            boolDoor = true;
        }

        if (activated && doorType == DOORTYPE.FREQUENCY)
        {

            if (direction == DIRECTION.FORWARD)
            {
                if (forwardI <= 1) {
                    moveDoor(posA, posB, forwardI);
                }
                else {
                    timeLapsed += fixedDelta;
                }
                
                if (timeLapsed >= waitUntilBackward) {
                    revertMovement();
                }
            }
            else if (direction == DIRECTION.BACKWARD)
            {
                if (backwardI <= 1) {
                    moveDoor(posB, posA, backwardI);
                }
                else {
                    timeLapsed += fixedDelta;
                }

                if (timeLapsed >= waitUntilBackward) {
                    revertMovement();
                }
            }
        }

        if (activated && doorType == DOORTYPE.BOOLEAN)
        {
            if (forwardI <= 1)
            {
                moveDoor(posA, posB, forwardI);
            } else
            {
                finishedBoolAperture = true;
            }
        } 

        

        if (!activated && boolDoor)
        {
            if (finishedBoolAperture)
            {
                finishedBoolAperture = false;
                revertMovement();
            }

            fixedDelta = Time.fixedDeltaTime;
            backwardI += fixedDelta * backwardSpeed;

            if (backwardI <= 1)
            {
                moveDoor(posB, posA, backwardI);
            }
        }
    }
}
