using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    /*index
      ########################
      #                      #
      #  MUSICA DE JUEGO     #
      #                      #
      ########################
    */

    private AudioSource musicSource;

    public AudioClip[] musics;

    public Utils.PlayingNow playingNow;

    public void setVolume()
    {
        musicSource.volume = OptionsManager.Instance.masterVolumenValueSaved;
        musicSource.volume *= OptionsManager.Instance.musicVolumenValueSaved;
    }

    public void playMainMenu()
    {

        if(musicSource.clip == musics[0]) {
            musicSource.clip = musics[1];
        } else if (musicSource.clip == null || musicSource.clip == musics[1]) {
            musicSource.clip = musics[0];
        }

        musicSource.Play();

    }

    /*index
      ########################
      #                      #
      #  FUNCIONES DE UNITY  #
      #                      #
      ########################
    */

    // Instanciar SoundManager
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
    }

    private void Start()
    {
        musicSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        playingNow = Utils.PlayingNow.NONE;
    }

    private void Update()
    {
        if (musicSource.isPlaying==false)
        {
            switch (playingNow)
            {
                case Utils.PlayingNow.MAINTHEME:
                playMainMenu();
                break;
            }
        }  
    }
}
