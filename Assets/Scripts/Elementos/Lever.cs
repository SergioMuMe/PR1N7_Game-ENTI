using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{

    //objeto u objetos que queremos que se activen
    public InterfaceGame[] elements;

    private bool on = false;

    public void Switch()
    {
        if (on)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].Deactivate();
            }

            on = false;
        }
        else
        {
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].Activate();
            }

            on = true;
        }        
    }
}
