using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicBehav : MonoBehaviour
{
    public Camera kinCam;
    public Camera cam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            kinCam.rect = cam.rect;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            kinCam.gameObject.SetActive(false);
        }
    }
}
