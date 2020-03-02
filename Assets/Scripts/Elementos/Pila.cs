using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pila : MonoBehaviour
{

    private SceneController scriptSC;


    void Start()
    {
        scriptSC = GameObject.Find("NextLevel").GetComponent<SceneController>();
    }

    void Update()
    {
        transform.Rotate(0.2f, 0.5f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            scriptSC.batteryLevelCount--;
            Destroy(gameObject);
        }
    }
}
