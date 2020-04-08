using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLight : MonoBehaviour
{
    public Light lightF;
    public DoorController door;
    public Material redLight;
    public Material greenLight;
    public MeshRenderer finalLamp;

    // Update is called once per frame

    void Update()
    {
        if (door.activated && lightF.color != Color.green)
        {
            SoundManager.Instance.PlaySound("SCENARIO-exit_door");
            lightF.color = Color.green;
            finalLamp.material = greenLight;
        }
        else if (!door.activated && lightF.color != Color.red)
        {
            lightF.color = Color.red;
            finalLamp.material = redLight;
        }
    }
}
