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
    [Range(0,1)]
    public float blinkChance;
    public float blinkFreq;
    private Animator anim;
    private float actTime;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

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

        if(lightF.color != Color.green && Time.time > actTime)
        {
            actTime = Time.time + blinkFreq;

            if (Random.Range(0f,1f) <= blinkChance)
            {
                anim.SetTrigger("Blink");
            }
        }
    }
}
