using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    private CharacterController_Roger playerScript;
    private Collider2D cl;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<CharacterController_Roger>();
        cl = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        OnTriggerEnter2D(cl);
        OnTriggerExit2D(cl);
    }

    private void OnTriggerEnter2D(Collider2D Player)
    {
        if(Player.tag != "Player")
        {
            return;
        }
        playerScript.jumpForce = playerScript.jumpForce * 2;
    }

    private void OnTriggerExit2D(Collider2D Player)
    {
        if (Player.tag != "Player")
        {
            return;
        }
        playerScript.jumpForce = playerScript.jumpForce/2;
    }
}
