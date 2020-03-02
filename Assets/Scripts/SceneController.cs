using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string sceneName;
    public string sceneRestart;


    private void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void Start()
    {
        sceneRestart = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            loadScene(sceneRestart);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            loadScene(sceneName);
        }
    }
}
