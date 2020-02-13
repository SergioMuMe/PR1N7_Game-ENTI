using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehav : MonoBehaviour
{
    public enum CharacterType
    {
        PLAYER,
        CLONE
    }
    private enum DirectionInputs
    {
        NONE,
        RIGHT,
        LEFT
    }

    public CharacterType type;

    public List<CommandsInputs> inputs;
    private CommandsInputsEnum actualInput;
    private float initInputTime = 0.0f;
    public float limitRecordingTime = 0.0f;
    private float initCloningTime;

    public float baseSpeed = 0.0f;
    public float jumpSpeed = 0.0f;
    public float jumpForce = 0.0f;

    public float jumpForceDefault = 0.0f;
    public float jumpForceBouncer = 0.0f;

    public float maxVelocity = 0.0f;
    public float maxHeight = 0.0f;

    public float maxHeightDefault = 0.0f;
    public float maxHeightBouncer = 0.0f;

    public float saveJumpTime = 0.0f;
    private float actualJumpTime = 0.0f;

    public Rigidbody2D rb;
    public ParticleSystem part;
    public ParticleSystemRenderer partRender;

    public float groundedPrecision = 0.0f;
    private float groundDistance = 0.0f;

    private DirectionInputs direction = DirectionInputs.NONE;
    private RaycastHit2D hitGround;

    public bool isJumping = true;
    public bool isFalling = true;
    public bool isRecording = false;

    public CharacterBehav player;
    public GameObject clon;
    public int iteration = 0;
    public Vector2 initPos;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        groundDistance = GetComponent<Collider2D>().bounds.extents.y;

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

                    player = Instantiate(clon, transform.position, transform.rotation).GetComponent<CharacterBehav>();
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


                break;

            case CharacterType.CLONE:
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

        hitGround = Physics2D.Raycast(transform.position, -Vector2.up, groundDistance + groundDistance);

        if (hitGround && isJumping && isFalling)
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
        else if (!hitGround && actualJumpTime < Time.time && isFalling)
        {
            isJumping = true;
        }
        else if (!hitGround && actualJumpTime < Time.time && !isFalling)
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Lever" && Input.GetKeyDown(KeyCode.F))
        {
            other.GetComponent<Lever>().Switch();
        }
    }
}