using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{

    private VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.time > 0 && !videoPlayer.isPlaying || Input.anyKey)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
