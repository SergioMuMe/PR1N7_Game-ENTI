using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehav : MonoBehaviour
{
    /*index
        ########################
        #                      #
        # CHARACTER BEHAVIOUR  #
        #                      #
        ########################
    */

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

    public bool facingRight = true;

    //Almacena el tipo de personaje
    public CharacterType type;

    //Almacena el ultimo input durante la grvacion
    private CommandsInputsEnum actualInput;
    //Lista de inputs que le passamos al clon
    public List<CommandsInputs> inputs;
    //Almacena el tiempo en el que se ha usado un input
    public float initInputTime = 0.0f;
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

    //Rigidbody del personaje
    public Rigidbody2D rb;
    //Particulas de aterrizaje
    public ParticleSystem part;

    public ParticleSystem nanoStart;
    public ParticleSystem nanoEnd;

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

    private RaycastHit2D hitRight;
    private RaycastHit2D hitLeft;

    private bool colRight;
    private bool colLeft;

    private float lastFrameY;

    public bool isGrounded = false;
    //Almacena el estado si esta saltando
    public bool isJumping = true;

    private bool isFalling = true;
    private bool isRising = true;
    //Almacena el estado si esta grabando
    public bool isRecording = false;
    //Almacena el estado si ha interactuado
    public bool isInteracting = false;

    private float maxJumpTime = 500;
    private float jumpTime;


    //Almacena el script del clon que gestiona las proyecciones
    public BubbleProjectedController bubblePlayer;
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

    private Animator cloneAnim;
    public Animator playerAnimator;

    private Lever button;
    void Start()
    {
        bubblePlayer.ShowBubble(false);       

        rb = GetComponent<Rigidbody2D>();

        cloneAnim = GetComponent<Animator>();

        groundDistance = GetComponent<Collider2D>().bounds.extents.y;
        frontDistance = GetComponent<Collider2D>().bounds.extents.x;

        maxHeight = maxHeightDefault;
        jumpForce = jumpForceDefault;

        if (type == CharacterType.CLONE)
        {
            initPos = transform.position;

            initInputTime = Time.time * 1000;
        }

        if (facingRight)
        {
            cloneAnim.SetBool("FacingRight", true);
        }
        else
        {
            cloneAnim.SetBool("FacingRight", false);
        }
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

                    jumpTime = 0;

                    isJumping = true;
                    playerAnimator.SetBool("Jump", true);
                    SoundManager.Instance.PlaySound("PLAYER-jump");
                }

                if (Input.GetKeyDown(KeyCode.R) && !isRecording && maxClones > 0)
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

                    InputManager.Instance.actualInputs.recording = true; //Lo necesito para los fx
                    isRecording = true;

                    player.facingRight = facingRight;

                    initInputTime = (Time.time * 1000);
                    initCloningTime = Time.time;
                    inputs.Add(new CommandsInputs(CommandsInputsEnum.START, (Time.time * 1000) - initInputTime));

                    SoundManager.Instance.PlaySound("PLAYER-recording");
                    GameManager.Instance.setProfileFX("profileFX_CloneRecording");

                }
                else if (isRecording && Input.GetKeyDown(KeyCode.R) || isRecording && Time.time >= initCloningTime + limitRecordingTime)
                {
                    inputs.Add(new CommandsInputs(CommandsInputsEnum.END, (Time.time * 1000) - initInputTime));
                    InputManager.Instance.actualInputs.recording = false; //Lo necesito para los fx
                    isRecording = false;
                    if (inputs.Count > 3)
                    {
                        player.enabled = true;
                        player.inputs = inputs; 
                    }

                    SoundManager.Instance.StopSound("PLAYER-recording");
                    GameManager.Instance.restoreProfileFX();
                }


                if (Input.GetKeyDown(KeyCode.F) && !isInteracting && button != null) 
                {
                    isInteracting = true;
                    button.Switch();
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
                if (iteration >= 0 && iteration < inputs.Capacity && inputs[iteration].time <= (Time.time * 1000) - initInputTime)
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
                            isJumping = true;
                            playerAnimator.SetBool("Jump", true);
                            break;
                        case CommandsInputsEnum.INTERACT:
                            if (button != null) { button.Switch(); };
                            isInteracting = true;
                            break;
                        case CommandsInputsEnum.START:
                            direction = DirectionInputs.NONE;
                           
                            break;
                        case CommandsInputsEnum.END:
                            direction = DirectionInputs.NONE;
                            cloneAnim.SetTrigger("End");
                            iteration = -10;
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

        if (isJumping)
        {
            jumpTime += delta;

            if (type == CharacterType.PLAYER)
            {
                hitForward = Physics2D.Raycast(transform.position + (Vector3.down * groundDistance) + (Vector3.right * (frontDistance - groundedPrecision)), -Vector2.up, groundedPrecision);
                hitBack = Physics2D.Raycast(transform.position + (Vector3.down * groundDistance) + (Vector3.left * (frontDistance - groundedPrecision)), -Vector2.up, groundedPrecision);
            }
            else
            {
                hitForward = Physics2D.Raycast(transform.position + (Vector3.down * groundDistance) + (Vector3.right * (frontDistance - groundedPrecision)), -Vector2.up, groundedPrecision, 9);
                hitBack = Physics2D.Raycast(transform.position + (Vector3.down * groundDistance) + (Vector3.left * (frontDistance - groundedPrecision)), -Vector2.up, groundedPrecision, 9);
            }


            if (!hitForward && !hitBack)
            {
                isGrounded = false;
            }

            if (Physics2D.Raycast(transform.position + (Vector3.right * frontDistance) + (Vector3.down * groundDistance), Vector2.right, groundedPrecision))
            {
                colRight = true;
            }
            else
            {
                colRight = false;  
            }

            if (Physics2D.Raycast(transform.position + (Vector3.left * frontDistance) + (Vector3.down * groundDistance), Vector2.left, groundedPrecision))
            {
                colLeft = true;
            }
            else
            {
                colLeft = false; 
            }

            if (isGrounded && jumpTime > maxJumpTime)
            {
                isJumping = false;
                playerAnimator.SetBool("Jump", false);
            }
        }
        else
        {
            if (colRight != false || colLeft != false)
            {
                colRight = false;
                colLeft = false;
            }

            if (isFalling)
            {
                if (type == CharacterType.PLAYER)
                {
                    hitForward = Physics2D.Raycast(transform.position + (Vector3.down * groundDistance) + (Vector3.right * (frontDistance - groundedPrecision)), -Vector2.up, groundedPrecision);
                    hitBack = Physics2D.Raycast(transform.position + (Vector3.down * groundDistance) + (Vector3.left * (frontDistance - groundedPrecision)), -Vector2.up, groundedPrecision);
                }
                else
                {
                    hitForward = Physics2D.Raycast(transform.position + (Vector3.down * groundDistance) + (Vector3.right * (frontDistance - groundedPrecision)), -Vector2.up, groundedPrecision, 0);
                    hitBack = Physics2D.Raycast(transform.position + (Vector3.down * groundDistance) + (Vector3.left * (frontDistance - groundedPrecision)), -Vector2.up, groundedPrecision, 0);
                }

                if (!hitForward && !hitBack)
                {
                    isJumping = true;
                }                
            }

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
                if (!isJumping && !colRight || !isJumping && type == CharacterType.CLONE)
                {
                    rb.AddForce(Vector2.right * baseSpeed, ForceMode2D.Force);
                }
                else if (!colRight || type == CharacterType.CLONE)
                {
                    rb.AddForce(Vector2.right * jumpSpeed, ForceMode2D.Force);
                }
                if (!facingRight)
                {
                    bubblePlayer.SpriteFlipX(false);
                    facingRight = true;
                    cloneAnim.SetBool("FacingRight", true);
                    cloneAnim.SetTrigger("Flip");
                    //transform.Rotate(0, 180, 0);
                }
                break;
            case DirectionInputs.LEFT:
                if (!isJumping && !colLeft || !isJumping && type == CharacterType.CLONE)
                {
                    rb.AddForce(Vector2.left * baseSpeed, ForceMode2D.Force);
                }
                else if (!colLeft || type == CharacterType.CLONE)
                {
                    rb.AddForce(Vector2.left * jumpSpeed, ForceMode2D.Force);
                }
                if (facingRight)
                {
                    bubblePlayer.SpriteFlipX(true);
                    facingRight = false;
                    cloneAnim.SetBool("FacingRight", false);
                    cloneAnim.SetTrigger("Flip");
                    //transform.Rotate(0, 180, 0);
                }
                break;
            default:
                break;
        }

        if (!isFalling && lastFrameY > transform.position.y)
        {
            isFalling = true;
            isRising = false;
        }
        else if (!isRising && lastFrameY < transform.position.y)
        {
            isRising = true;
            isFalling = false;
        }
        else if (isFalling || isRising)
        {
            isFalling = false;
            isRising = false;
        }

        lastFrameY = transform.position.y;
    }

    public void EndIterationParticles()
    {
        nanoEnd.Play();
    }

    public void StartIterationParticles()
    {
        initInputTime = (Time.time * 1000);
        iteration = 0;
        transform.position = initPos;
        nanoStart.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Lever")
        {
            button = collision.GetComponent<Lever>();
            if (gameObject.tag == "Player")
            {
                bubblePlayer.SetProjection("KeyF");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && type == CharacterType.CLONE)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }

        if (collision.tag == "Lever")
        {
            bubblePlayer.ShowBubble(false);
            
            button = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (hitForward && !isGrounded && !isRising || hitBack && !isGrounded &&!isRising)
        {

            isJumping = false;
            isGrounded = true;
            playerAnimator.SetBool("Jump", false);
            SoundManager.Instance.PlaySound("PLAYER-land");

            if (maxHeight > maxHeightDefault)
            {
                maxHeight = maxHeightDefault;
            }

            part.Emit(50);
        }
        else if (!hitForward && !hitBack)
        {
            isJumping = true;
        }
    }

    private void OnDestroy()
    {
        Destroy(part.gameObject);
    }

    public void DestroyAllClones()
    {
        while (clones.Count > 0)
        {
            Destroy(clones[clones.Count - 1]);
            clones.RemoveAt(clones.Count - 1);
        }
    }
}