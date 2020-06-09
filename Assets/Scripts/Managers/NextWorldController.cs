using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextWorldController : MonoBehaviour
{

    public int nextScene;

    void Start()
    {
        //Desactivamos render
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SceneManager.LoadScene(GameManager.Instance.nextLevelId);
        }
    }
}
