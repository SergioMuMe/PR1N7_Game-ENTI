using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionTrigger : MonoBehaviour
{
    private string newText;
    private string oldText;

    public DoorController door;
    public TextMeshProUGUI text;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        oldText = GameManager.Instance.oldWorldName;
        newText = GameManager.Instance.newWorldName;

        text.text = oldText;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetTrigger("Change");
            
        }   
    }

    public void SwapText()
    {
        door.activated = true;
        text.text = newText;
    }

}
