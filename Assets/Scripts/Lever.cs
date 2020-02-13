using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    //objeto u objetos que queremos que se activen
    public InterfaceGame[] elements;

    public void Switch()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].Activate();
        }
    }
}
