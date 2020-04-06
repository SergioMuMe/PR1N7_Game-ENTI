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
        Debug.developerConsoleVisible = true;
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Play();
    }

    public float fadeInTime = 0;
    public float timeToFadeIn;
    public float volumeFade;
    float volume = 0;

    // Update is called once per frame
    void Update()
    {
        
        if (volume <0.5f)
        {
            fadeInTime += Time.deltaTime * 1000;
            if (fadeInTime > timeToFadeIn)
            {
                volume += volumeFade;
                videoPlayer.SetDirectAudioVolume(0, volume);
            }
        }
        
        
        if (videoPlayer.time > 0 && !videoPlayer.isPlaying || Input.anyKey)
        {
            SceneManager.LoadScene("Login");
        }
    }
}
