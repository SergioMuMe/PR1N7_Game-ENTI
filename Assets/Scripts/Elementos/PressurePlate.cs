using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    //objeto u objetos que queremos que se activen
    public InterfaceGame[] elements;
    public bool on = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (!on)
        {
            SoundManager.Instance.PlaySound("SCENARIO-pressure_plate");
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].Activate();
            }

            on = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (on)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].Deactivate();
            }

            on = false;
        }
    }
}
