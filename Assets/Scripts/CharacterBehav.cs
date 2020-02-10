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

    private bool rightCollider = false;
    private bool leftCollider = false;

    public bool isJumping = true;
    public bool isFalling = true;
    public bool isRecording = false;

    public CharacterBehav player;
    public GameObject clon;
    //private float initTime;
    private int iteration = 0;
    private Vector2 initPos;


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
                        inputs.Add(new CommandsInputs(actualInput, Time.time - initInputTime));

                        initInputTime = Time.time;
                        actualInput = CommandsInputsEnum.LEFT;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow) && actualInput != CommandsInputsEnum.RIGHT || Input.GetKey(KeyCode.D) && actualInput != CommandsInputsEnum.RIGHT)
                    {
                        inputs.Add(new CommandsInputs(actualInput, Time.time - initInputTime));

                        initInputTime = Time.time;
                        actualInput = CommandsInputsEnum.RIGHT;
                    }
                    else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
                    {
                        inputs.Add(new CommandsInputs(actualInput, Time.time - initInputTime));

                        initInputTime = Time.time;
                        actualInput = CommandsInputsEnum.NONE;
                    }
                    if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
                    {
                        inputs.Add(new CommandsInputs(actualInput, Time.time - initInputTime));

                        initInputTime = Time.time;
                        actualInput = CommandsInputsEnum.JUMP;
                    }

                    Debug.Log(actualInput);
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

                    initInputTime = Time.time;
                    actualInput = CommandsInputsEnum.START;

                }
                else if (isRecording && Input.GetKeyDown(KeyCode.R) || isRecording && Time.time >= initInputTime + limitRecordingTime)
                {
                    inputs.Add(new CommandsInputs(actualInput, Time.time - initInputTime));

                    initInputTime = Time.time;
                    actualInput = CommandsInputsEnum.END;

                    inputs.Add(new CommandsInputs(actualInput, Time.time - initInputTime));

                    isRecording = false;
                    GetComponentInParent<CharacterBehav>().getAlive();
                }

                
                break;

            case CharacterType.CLONE:
                // Esperar a que la lista tenga "END"
                if (inputs[iteration].time < Time.time - initInputTime)
                {
                    switch (inputs[iteration].ci)
                    {
                        case CommandsInputsEnum.NONE:
                            break;
                        case CommandsInputsEnum.RIGHT:
                            break;
                        case CommandsInputsEnum.LEFT:
                            break;
                        case CommandsInputsEnum.JUMP:
                            break;
                        case CommandsInputsEnum.INTERACT:
                            break;
                        case CommandsInputsEnum.START:
                            break;
                        case CommandsInputsEnum.END:
                            iteration = -1;
                            initInputTime = Time.time;
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
        //player = GetComponentInChildren<CharacterBehav>();
        initPos = transform.position;

        inputs = player.inputs;
        initInputTime = Time.time;

        //transform.DetachChildren();
    }
}
