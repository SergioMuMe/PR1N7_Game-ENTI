using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public CharacterController_Roger player;

    //objeto u objetos que queremos que se activen
    public InterfaceGame[] elements;

    public bool activated = false;

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {        
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].Activate();
            }
        }
    }
}
