using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    
    public List<CharacterController_Roger.CommandsInputs> inputsClone;
    public List<float> inputTimingClone;
    CharacterController_Roger player;

    private float initTime;
    private int i = 0;
    private Vector2 initPos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(initTime + inputTimingClone[i] <= Time.time)
        {
            switch (inputsClone[i])
            {
                case CharacterController_Roger.CommandsInputs.NONE:
                    break;
                case CharacterController_Roger.CommandsInputs.RIGHT_UP:
                    break;
                case CharacterController_Roger.CommandsInputs.RIGHT_DOWN:
                    break;
                case CharacterController_Roger.CommandsInputs.LEFT_UP:
                    break;
                case CharacterController_Roger.CommandsInputs.LEFT_DOWN:
                    break;
                case CharacterController_Roger.CommandsInputs.JUMP:
                    break;
                case CharacterController_Roger.CommandsInputs.INTERACT:
                    break;
                case CharacterController_Roger.CommandsInputs.START:
                    break;
                case CharacterController_Roger.CommandsInputs.END:
                    i = -1;
                    initTime = Time.time;
                    transform.position = initPos;
                    break;
                default:
                    break;
            }
            i++;
        }
    }

    public void getAlive()
    {
        player = GetComponentInChildren<CharacterController_Roger>();
        initPos = transform.position;

        inputsClone = player.inputs;
        inputTimingClone = player.inputTiming;
        initTime = Time.time;

        transform.DetachChildren();
    }
}
