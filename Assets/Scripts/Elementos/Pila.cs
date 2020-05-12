using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pila : MonoBehaviour
{
    //Almacena el script del clon que gestiona las proyecciones
    public BubbleProjectedController bubblePlayer;

    private SceneController scriptSC;
    private Animator anim;
    private ParticleSystem part;

    void Start()
    {
        scriptSC = GameObject.Find("NextLevel").GetComponent<SceneController>();
        bubblePlayer = GameObject.Find("Player").GetComponent<BubbleProjectedController>();
        anim = GetComponent<Animator>();
        part = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bubblePlayer.SetProjection("HeartPila");
            SoundManager.Instance.PlaySound("PLAYER-BatteryCollected");
            scriptSC.batteryLevelCount--;
            anim.SetTrigger("PickUp");
        }
    }

    public void SetParticles()
    {
        part.Emit(30);
    }
}
