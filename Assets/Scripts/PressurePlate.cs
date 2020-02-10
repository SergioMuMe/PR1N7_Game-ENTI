using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public CharacterController_Roger player;

    //objeto u objetos que queremos que se activen
    public InterfaceGame[] elements;

    private void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].Activate();
        }
    }
}
