using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public CharacterBehav player;

    //objeto u objetos que queremos que se activen
    public InterfaceGame[] elements;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("h");
        if (other.tag == "Player" || other.tag == "Clone")
        {
            Debug.Log("l");
            player = other.GetComponent<CharacterBehav>();
            if (player.isInteracting)
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    elements[i].Activate();
                }

                player.isInteracting = false;
            }
        }
    }
}
