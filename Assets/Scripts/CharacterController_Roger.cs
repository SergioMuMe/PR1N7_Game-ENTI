using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController_Roger : MonoBehaviour
{
    private enum DirectionInputs
    {
        NONE,
        RIGHT,
        LEFT
    }
    public enum CommandsInputs
    {
        NONE,
        RIGHT_UP,
        RIGHT_DOWN,
        LEFT_UP,
        LEFT_DOWN,
        JUMP,
        INTERACT,
        START,
        END
    }
    public GameObject clon;

    public List<CommandsInputs> inputs;
    public List<float> inputTiming;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        groundDistance = GetComponent<Collider2D>().bounds.extents.y;

        maxHeight = maxHeightDefault;
        jumpForce = jumpForceDefault;

    }

    void Update()
    {

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
            isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isRecording)
        {
            transform.SetParent(Instantiate(clon, transform.position, transform.rotation).transform);
            inputTiming = new List<float>();
            inputs = new List<CommandsInputs>();

            isRecording = true;
            initInputTime = Time.time;
            
            inputs.Add(CommandsInputs.START);
            inputTiming.Add(Time.time - initInputTime);

        }
        else if(isRecording && Input.GetKeyDown(KeyCode.R) || isRecording && Time.time >= initInputTime + limitRecordingTime)
        {
            inputs.Add(CommandsInputs.END);
            inputTiming.Add(Time.time - initInputTime);
            isRecording = false;
            GetComponentInParent<CloneController>().getAlive();
        }

            //::::CLONACION::::
            if (isRecording)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                inputs.Add(CommandsInputs.LEFT_DOWN);
                inputTiming.Add(Time.time - initInputTime);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                inputs.Add(CommandsInputs.LEFT_UP);
                inputTiming.Add(Time.time - initInputTime);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                inputs.Add(CommandsInputs.RIGHT_DOWN);
                inputTiming.Add(Time.time - initInputTime);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                inputs.Add(CommandsInputs.RIGHT_UP);
                inputTiming.Add(Time.time - initInputTime);
            }
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                inputs.Add(CommandsInputs.JUMP);
                inputTiming.Add(Time.time - initInputTime);
            }
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
}
