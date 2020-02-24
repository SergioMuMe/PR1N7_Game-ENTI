﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehav : MonoBehaviour
{
    //Enum de tipos de personaje
    public enum CharacterType
    {
        PLAYER,
        CLONE
    }

    //Enum para los inputs de moviminento
    private enum DirectionInputs
    {
        NONE,
        RIGHT,
        LEFT
    }

    //Almacena el tipo de personaje
    public CharacterType type;

    //Almacena el ultimo input durante la grvacion
    private CommandsInputsEnum actualInput;
    //Lista de inputs que le passamos al clon
    public List<CommandsInputs> inputs;
    //Almacena el tiempo en el que se ha usado un input
    private float initInputTime = 0.0f;
    //Almacena el tiempo maximo durante en el que grabamos los inputs
    public float limitRecordingTime = 0.0f;
    //Almacena el inicio de la fase de gravado
    private float initCloningTime;
    //Lista de los gameObjects de los clones
    public List<GameObject> clones;
    //Almacena la cantidad maxima de clones posibles
    public int maxClones = 0;

    //Almacena la velocidad de movimiento
    public float baseSpeed = 0.0f;
    //Almacena la velocidad de desplazamiento durante el salto
    public float jumpSpeed = 0.0f;
    //Almacena la fuerza del salto
    public float jumpForce = 0.0f;

    //Almacena la fuerza de salto por defecto
    public float jumpForceDefault = 0.0f;
    //Almacena la fuerza de salto en un saltador
    public float jumpForceBouncer = 0.0f;

    //Almacena la velocidad maxima de desplazamiento
    public float maxVelocity = 0.0f;
    //Almacena la velocidad vertical maxima del salto
    public float maxHeight = 0.0f;

    //Almacena la velocidad vertical maxima por defecto
    public float maxHeightDefault = 0.0f;
    //Almacena la velocidad vertical maxima en un saltador
    public float maxHeightBouncer = 0.0f;

    //Almacena el tiempo en el que puedes saltar sin tocar el suelo
    public float saveJumpTime = 0.0f;
    //Almacena el tiempo en el que hemos saltado
    private float actualJumpTime = 0.0f;

    //Rigidbody del personaje
    public Rigidbody2D rb;
    //Particulas de aterrizaje
    public ParticleSystem part;
    //Color de las particulas
    public ParticleSystemRenderer partRender;

    //Almacena la distancia a la que detecta el suelo
    public float groundedPrecision = 0.0f;
    //Almacena la distancia al suelo
    private float groundDistance = 0.0f;
    private float frontDistance = 0.0f;

    //Almacena la direccion de movimiento
    private DirectionInputs direction = DirectionInputs.NONE;
    //Almacena el impacto del rayo que detecta el suelo
    private RaycastHit2D hitForward;
    private RaycastHit2D hitBack;

    //Almacena el estado si esta saltando
    public bool isJumping = true;
    //Almacena el estado de si no detecta el suelo
    public bool isFalling = true;
    //Almacena el estado si esta grabando
    public bool isRecording = false;
    //Almacena el estado si ha interactuado
    public bool isInteracting = false;

    //Almacena el script del clon que acaba de crear
    public CharacterBehav player;
    //Almacena el Prefab del clon
    public GameObject clone;
    //Amacena la iteracion de la lista de inputs en la reproduccion
    public int iteration = 0;
    //Almacena la posicion inicial del clon
    public Vector2 initPos;

    //Almacena el multiplicador que acelera el tiempo
    public float timeAcceleration = 0.0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        groundDistance = GetComponent<Collider2D>().bounds.extents.y;
        frontDistance = GetComponent<Collider2D>().bounds.extents.x;

        part = GetComponentInChildren<ParticleSystem>();
        partRender = GetComponentInChildren<ParticleSystemRenderer>();

        maxHeight = maxHeightDefault;
        jumpForce = jumpForceDefault;


    }


    void Update()
    {
        switch (type)
        {
            case CharacterType.PLAYER:

                //::::CLONACION::::
                if (isRecording)
                {
                    if (Input.GetKey(KeyCode.LeftArrow) && actualInput != CommandsInputsEnum.LEFT || Input.GetKey(KeyCode.A) && actualInput != CommandsInputsEnum.LEFT)
                    {
                        inputs.Add(new CommandsInputs(CommandsInputsEnum.LEFT, (Time.time*1000) - initInputTime));
                    }
                    else if (Input.GetKey(KeyCode.RightArrow) && actualInput != CommandsInputsEnum.RIGHT || Input.GetKey(KeyCode.D) && actualInput != CommandsInputsEnum.RIGHT)
                    {
                        inputs.Add(new CommandsInputs(CommandsInputsEnum.RIGHT, (Time.time * 1000) - initInputTime));
                    }
                    else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && inputs[inputs.Count - 1].ci != CommandsInputsEnum.NONE)
                    {
                        inputs.Add(new CommandsInputs(CommandsInputsEnum.NONE, (Time.time * 1000) - initInputTime));
                    }
                    if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
                    {
                        inputs.Add(new CommandsInputs(CommandsInputsEnum.JUMP, (Time.time * 1000) - initInputTime));
                    }
                    if (Input.GetKeyDown(KeyCode.F) && !isInteracting)
                    {
                        inputs.Add(new CommandsInputs(CommandsInputsEnum.INTERACT, (Time.time * 1000) - initInputTime));
                    }
                }

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    direction = DirectionInputs.LEFT;
                }
                else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    direction = DirectionInputs.RIGHT;
                }
                else
                {
                    direction = DirectionInputs.NONE;
                }

                if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
                {
                    rb.AddForce(Vector2.up * jumpForce);
                    direction = DirectionInputs.NONE;
                    actualJumpTime = Time.time + saveJumpTime;

                    isJumping = true;
                }

                if (Input.GetKeyDown(KeyCode.R) && !isRecording)
                {
                    //transform.SetParent(Instantiate(clon, transform.position, transform.rotation).transform);

                    player = Instantiate(clone, transform.position, transform.rotation).GetComponent<CharacterBehav>();

                    if (clones.Count >= maxClones)
                    {
                        Destroy(clones[0]);
                        clones.RemoveAt(0);
                    }

                    clones.Add(player.gameObject);
                   
                    inputs = new List<CommandsInputs>();

                    isRecording = true;

                    initInputTime = (Time.time * 1000);
                    initCloningTime = Time.time;
                    inputs.Add(new CommandsInputs(CommandsInputsEnum.START, (Time.time * 1000) - initInputTime));

                }
                else if (isRecording && Input.GetKeyDown(KeyCode.R) || isRecording && Time.time >= initCloningTime + limitRecordingTime)
                {
                    inputs.Add(new CommandsInputs(CommandsInputsEnum.END, (Time.time * 1000) - initInputTime));

                    isRecording = false;
                    player.enabled = true;
                    player.inputs = inputs;
                    player.getAlive();
                }


                if (Input.GetKeyDown(KeyCode.F) && !isInteracting)
                {
                    isInteracting = true;
                }
                else if (isInteracting)
                {
                    isInteracting = false;
                }

                if (Input.GetKeyDown(KeyCode.X) && !isRecording)
                {
                    if (clones.Count > 0)
                    {
                        Destroy(clones[0]);
                        clones.RemoveAt(0);
                    }
                }
                if (Input.GetKeyDown(KeyCode.C) && !isRecording)
                {
                    if (clones.Count > 0)
                    {
                        Destroy(clones[clones.Count-1]);
                        clones.RemoveAt(clones.Count-1);
                    }
                }
                break;

            case CharacterType.CLONE:

                if (isInteracting)
                {
                    isInteracting = false;
                }
                // Esperar a que la lista tenga "END"
                if (inputs[iteration].time <= (Time.time * 1000) - initInputTime)
                {

                    switch (inputs[iteration].ci)
                    {
                        case CommandsInputsEnum.NONE:
                            direction = DirectionInputs.NONE;
   
                            break;
                        case CommandsInputsEnum.RIGHT:
                            direction = DirectionInputs.RIGHT;
                            
                            break;
                        case CommandsInputsEnum.LEFT:
                            direction = DirectionInputs.LEFT;
                            
                            break;
                        case CommandsInputsEnum.JUMP:
                            rb.AddForce(Vector2.up * jumpForce);

                            break;
                        case CommandsInputsEnum.INTERACT:
                            isInteracting = true;
                            break;
                        case CommandsInputsEnum.START:
                            direction = DirectionInputs.NONE;
                           
                            break;
                        case CommandsInputsEnum.END:
                            direction = DirectionInputs.NONE;
                            initInputTime = (Time.time * 1000);
                            iteration = -1;
                            transform.position = initPos;
                            break;
                        default:
                            break;
                    }

                    iteration++;
                }
                break;

            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime * 1000;

        hitForward = Physics2D.Raycast(transform.position + (Vector3.down*groundDistance) + (Vector3.right * frontDistance), -Vector2.up, groundedPrecision);
        hitBack = Physics2D.Raycast(transform.position + (Vector3.down * groundDistance) + (Vector3.left * frontDistance), -Vector2.up, groundedPrecision);



        if (hitForward && isJumping && isFalling || hitBack && isJumping && isFalling)
        {
            isJumping = false;
            isFalling = false;

            if (maxHeight > maxHeightDefault)
            {
                maxHeight = maxHeightDefault;
            }

            partRender.material = GetComponent<MeshRenderer>().material;
            part.Emit(50);
        }
        else if (!hitForward && hitBack && actualJumpTime < Time.time && isFalling)
        {
            isJumping = true;
        }
        else if (!hitForward && !hitBack && actualJumpTime < Time.time && !isFalling)
        {
            actualJumpTime = Time.time + saveJumpTime;
            isFalling = true;
        }
        else if (!isFalling && isJumping && actualJumpTime < Time.time)
        {
            isFalling = true;
        }


        if (rb.velocity.x > maxVelocity)
        {
            rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxVelocity)
        {
            rb.velocity = new Vector2(-maxVelocity, rb.velocity.y);
        }

        if (rb.velocity.y > maxHeight)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxHeight);
        }
        else if (rb.velocity.y < -maxHeight)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxHeight);
        }

        switch (direction)
        {
            case DirectionInputs.NONE:
                break;
            case DirectionInputs.RIGHT:
                if (!isJumping)
                {
                    rb.AddForce(Vector2.right * baseSpeed, ForceMode2D.Force);
                }
                else
                {
                    rb.AddForce(Vector2.right * jumpSpeed, ForceMode2D.Force);
                }
                break;
            case DirectionInputs.LEFT:
                if (!isJumping)
                {
                    rb.AddForce(Vector2.left * baseSpeed, ForceMode2D.Force);
                }
                else
                {
                    rb.AddForce(Vector2.left * jumpSpeed, ForceMode2D.Force);
                }
                break;
            default:
                break;
        }
    }

    public void getAlive()
    {
        initPos = transform.position;

        initInputTime = Time.time;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && type == CharacterType.CLONE)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Lever" && isInteracting)
        {
            collision.GetComponent<Lever>().Switch();
            isInteracting = false;
        }
    }

    private void OnDestroy()
    {
        Destroy(part.gameObject);
    }
}