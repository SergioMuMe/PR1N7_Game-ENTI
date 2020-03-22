using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public struct Inputs
    {
        public bool right;
        public bool left;
        public bool jump;
        public bool interaction;
        public bool recording;
        public bool deleteFirst;
        public bool deleteLast;
    }

    public Inputs actualInputs;

    public Inputs lastFrameInputs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            actualInputs.left = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            actualInputs.left = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            actualInputs.right = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            actualInputs.right = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            actualInputs.jump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            actualInputs.jump = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            actualInputs.interaction = true;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            actualInputs.interaction = false;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            actualInputs.deleteFirst = true;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            actualInputs.deleteFirst = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            actualInputs.deleteLast = true;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            actualInputs.deleteLast = false;
        }
    }
}
