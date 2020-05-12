using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pila : MonoBehaviour
{
    //Almacena el script del clon que gestiona las proyecciones
    public BubbleProjectedController bubblePlayer;

    private SceneController scriptSC;


    void Start()
    {
        scriptSC = GameObject.Find("NextLevel").GetComponent<SceneController>();
        bubblePlayer = GameObject.Find("Player").GetComponent<BubbleProjectedController>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(0.2f, 0.5f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bubblePlayer.SetProjection("HeartPila");
            SoundManager.Instance.PlaySound("PLAYER-BatteryCollected");
            scriptSC.batteryLevelCount--;
            //Destroy(gameObject);
        }
    }
}
