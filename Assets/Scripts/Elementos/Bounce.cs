using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{

    public CharacterBehav player;
    public ParticleSystem particle;
    public int morePart;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Clone")
        {
            player = other.GetComponent<CharacterBehav>();

            player.jumpForce = player.jumpForceBouncer;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Clone")
        {
            if (player.maxHeight < player.maxHeightBouncer)
            {
                player.maxHeight = player.maxHeightBouncer;
            }

            if (player.isJumping && player.isGrounded)
            {
                Debug.Log("Salto");
                particle.Emit(morePart);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Clone")
        {
            other.GetComponent<CharacterBehav>().jumpForce = player.jumpForceDefault;
        }
    }
}
