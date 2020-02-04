using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterController_Roger>().jumpForce *= 2;
            other.gameObject.GetComponent<CharacterController_Roger>().maxHeight *= 2;
            other.gameObject.GetComponent<CharacterController_Roger>().maxVelocity *= 2;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterController_Roger>().jumpForce /= 2;
            //TODO: buscar la manera de que tarde un tiempo en aplicarse
            other.gameObject.GetComponent<CharacterController_Roger>().maxHeight /= 2;
            other.gameObject.GetComponent<CharacterController_Roger>().maxVelocity /= 2;
        }
    }
}
