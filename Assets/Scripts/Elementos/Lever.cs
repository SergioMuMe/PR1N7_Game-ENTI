using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{

    //objeto u objetos que queremos que se activen
    public InterfaceGame[] elements;
    public Light spotLight;

    private bool on = false;

    public void Switch()
    {
        SoundManager.Instance.PlaySound("SCENARIO-lever");

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].ChangeActivate();

            if (spotLight.color == Color.red)
            {
            spotLight.color = Color.green;
            }
            else if (spotLight.color == Color.green)
            {
                spotLight.color = Color.red;
            }
        }

        //if (on)
        //{
        //    for (int i = 0; i < elements.Length; i++)
        //    {
        //        elements[i].Deactivate();
        //    }
        //    on = false;
        //}
        //else
        //{    
        //    for (int i = 0; i < elements.Length; i++)
        //    {
        //        elements[i].Activate();
        //    }
        //    on = true;
        //}        
    }
}
