using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLight : MonoBehaviour
{
    public Light lightF;
    public DoorController door;
    
    // Update is called once per frame
    void Update()
    {
        if (door.activated && lightF.color != Color.green)
        {
            lightF.color = Color.green;
        }
        else if (!door.activated && lightF.color != Color.red)
        {
            lightF.color = Color.red;
        }
    }
}
