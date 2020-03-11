using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : InterfaceGame
{

    // ###########################
    // # CONFIGURATION VARIABLES #
    // ###########################

    //controlamos cuando se activa el movimiento de la puerta
    public bool activated;

    // Tipo de puerta: Define el comportamiento de la puerta en cuanto a su apertura y cierre.
    public enum DOORTYPE
    {
        BOOLEAN, // La puerta está abierta o cerrada
        FREQUENCY // La puerta se abre y cierra con una frecuencia.
    };
    public DOORTYPE doorType;

    // Tipo de movimiento: Define el comportamiento de la puerta en cuanto a su desplazamiento en el mundo.
    public enum MOVEMENTTYPE
    {
        SLIDING, // La puerta se desliza en un eje
        HINGED // La puerta pivota sobre un eje
    };
    public MOVEMENTTYPE movementType;

    // Puntos de navegación de GameObjects vacios para obtener la posición en el editor de niveles.
    // Definen posición inicial y final de la puerta mediante su translate.
    public GameObject navPointA;
    public GameObject navPointB;

    // O--------------------O
    // |   FREQUENCY DOOR   |
    // O--------------------O

    // Velocidad de movimiento.  0,X lento > 1 normal <  1,X 2,X 3,X rápido
    public float backwardSpeed;
    public float forwardSpeed;

    // Valor maximo en segundos cada cuanto se activa el movimiento de la puerta. 
    // Una vez la puerta ha llegado a uno de sus destinos, es tiempo en espera hasta que se reinicia el cambio de sentido.
    public float waitToForward;
    public float waitToBackward;

    // #############################
    // # INTERNAL SCRIPT VARIABLES #
    // #############################

    private Rigidbody2D rb2D;

    // deltaTime
    private float fixedDelta;

    //Tiempo a partir de que la puerta haya alcanzado un punto final en su desplazamiento. Sirve para los contadores de waitToForward/Backward
    private float timeLapsed;
    
    // Indices de la función Lerp(Vector2, Vector2, I)....Donde I tiene un valor entre 0 y 1.
    // El Indice I le asignamos un valor Time para que vaya a una velocidad dependiente del tiempo(fixedDelta) * multiplicador(speed)
    // A mayor speed, antes se alcanzará el valor 1.
    private float forwardI;
    private float backwardI;

    //Reinicia variables para las puertas de tipo FREQUENCY
    void revertMovement()
    {
        timeLapsed = 0;
        forwardI = 0;
        backwardI = 0;
        direction = (direction == DIRECTION.FORWARD ? DIRECTION.BACKWARD : DIRECTION.FORWARD);
    }

    // Variables de control para definir la dirección de la puerta. FORWARD = Abrir BACKWARD = Cerrar
    private enum DIRECTION { FORWARD, BACKWARD };
    private DIRECTION direction;

    // Booleanos de control para el funcionamiento de la puerta BOOLEANA
    private bool previousActivation;
    private bool finishedBoolAperture;

    // De los GameObjects positionA y positionB, obtenemos el transform.position en la función Start()
    private Vector2 posA;
    private Vector2 posB;   

    //Función que realiza el desplazamiento de la puerta.
    public void moveDoor(Vector2 posA, Vector2 posB, float t)
    {
        Vector2 newPosition = Vector2.Lerp(posA, posB, t);
        rb2D.MovePosition(newPosition);
    }

    //Pivote para el tipo de movimiento HINGED.
    private HingeJoint2D hinge;

    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        hinge = gameObject.GetComponent<HingeJoint2D>();
        
        //FAST TESTING VALUES
        //waitToForward = 1.5f;
        //waitToBackward = 1.5f;
        //forwardSpeed = 1f;
        //backwardSpeed = 1f;
        //doorType = DOORTYPE.BOOLEAN;
        

        // Obtenemos los navPoints.
        posA = navPointA.transform.position;
        posB = navPointB.transform.position;

        direction = DIRECTION.FORWARD;
        finishedBoolAperture = false;

        forwardI = 0;
        backwardI = 1;
    }

    
    void FixedUpdate()
    {
        if (activated)
        {
            fixedDelta = Time.fixedDeltaTime;

            forwardI += fixedDelta * Mathf.Abs(forwardSpeed);
           
            backwardI += fixedDelta * Mathf.Abs(backwardSpeed);            
        }

        // DOORTYPE = FREQUENCY
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
                
                if (timeLapsed > waitToBackward) {
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

                if (timeLapsed > waitToForward) {
                    revertMovement();
                }
            }
        }

        // DOORTYPE = BOOLEAN
        if (activated && doorType == DOORTYPE.BOOLEAN && backwardI >= 1)
        {
            if (!previousActivation)
            {
                previousActivation = true;
                forwardI = 0;
            }

            if (forwardI <= 1)
            {
                moveDoor(posA, posB, forwardI);
            } else if (!finishedBoolAperture)
            {
                finishedBoolAperture = true;
            }
        } 
        else if (!activated && doorType == DOORTYPE.BOOLEAN)
        {

            if (!finishedBoolAperture && previousActivation)
            {
                backwardI = 1 - forwardI;                
            }
            else if (previousActivation)
            {
                finishedBoolAperture = false;
                backwardI = 0;
            }

            if (previousActivation)
            {
                previousActivation = false;
            }

            if (backwardI <= 1)
            {
                fixedDelta = Time.fixedDeltaTime;

                backwardI += fixedDelta * Mathf.Abs(backwardSpeed);

                moveDoor(posB, posA, backwardI);
            }
        }
        else if (doorType == DOORTYPE.BOOLEAN)
        {
            moveDoor(posB, posA, backwardI);
        }

        // MOVEMENTTYPE = HINGED
        /* Tutorial Hinge Joint 2D https://www.youtube.com/watch?v=_zmzib4xSWc */

        
    }

    public override void Activate()
    {
        if (!activated)
        {
            activated = true;
        }   
    }

    public override void Deactivate()
    {
        if (activated)
        {
            activated = false;
        }
    }
}
