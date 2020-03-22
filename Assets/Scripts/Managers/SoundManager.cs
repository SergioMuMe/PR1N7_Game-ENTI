﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /*index
      ######################
      #                    #
      #  SOUND EFFECTS     #
      #                    #
      ######################
    */

 
    //Referencia del personaje para obtener estados
    

    public AudioClip recording;

    public AudioClip jump;
    private bool controlJump;

    public AudioClip land;

    private AudioSource effectsSource;

    public void playRecordingFX()
    {
        effectsSource.clip = recording;
        effectsSource.Play();
    }

    public void stopRecordingFX()
    {
        effectsSource.Stop();
    }

    /*index
      ########################
      #                      #
      #  MUSICA DE JUEGO     #
      #                      #
      ########################
    */
    #region MUSICA

    
    private AudioSource musicSource;
    
    public AudioClip[] musicsMainMenu;
    public AudioClip[] musicsTutorial;

    public Utils.PlayingNow playingNow;   

    //Configura los audiosource con el volumen aplicado por el usuario en opciones
    public void setVolume()
    {
        //Conseguimos el volumen master para ambos audiosource
        musicSource.volume = OptionsManager.Instance.masterVolumenValueSaved;
        effectsSource.volume = OptionsManager.Instance.masterVolumenValueSaved;

        //Seteamos volumen para cada audiosource. Este volumen es el último que se haya guardado.
        musicSource.volume *= OptionsManager.Instance.musicVolumenValueSaved;
        effectsSource.volume *= OptionsManager.Instance.effectsVolumenValueSaved;
    }

    //Modifica en tiempo real el volumen para que el usuario experimente los cambios antes de guardar configuración
    public void setVolumeTesting(float _master, float _music, float _effect)
    {
        //Conseguimos el volumen master para ambos audiosource
        musicSource.volume = _master;
        effectsSource.volume = _master;

        //Seteamos volumen para cada audiosource. Este volumen corresponde al slide de configuración.
        //si el usuario lo sube o baja, se verá reflejado sin tener que dar a guardar.
        musicSource.volume *= _music;
        effectsSource.volume *= _effect;
        
    }

    //Gestion de audioclips para main menu
    public void playMainMenu()
    {
        if (musicSource.clip == musicsMainMenu[0]) {
            musicSource.clip = musicsMainMenu[1];
        } else if (musicSource.clip == null || musicSource.clip == musicsMainMenu[1]) {
            musicSource.clip = musicsMainMenu[0];
        }

        musicSource.Play();

    }

    //Gestion de audioclips para tutorial
    public void playTutorial()
    {
        musicSource.Stop();
        musicSource.clip = null;

        if (musicSource.clip == musicsTutorial[0])
        {
            musicSource.clip = musicsTutorial[1];
        }
        else if (musicSource.clip == null || musicSource.clip == musicsTutorial[1])
        {
            musicSource.clip = musicsTutorial[0];
        }

        musicSource.Play();
    }


    public void stopMusic()
    {
        musicSource.Stop();
    }

    public void setMusicNull()
    {
        musicSource.clip = null;
    }
    #endregion

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
        musicSource = GameObject.Find("MusicAudioSource").GetComponent<AudioSource>();
        effectsSource = GameObject.Find("EffectsAudioSource").GetComponent<AudioSource>();
        playingNow = Utils.PlayingNow.NONE;

        controlJump = true;
    }

    private void Update()
    {
        /*index
        !!!!!!
        MUSICA
        !!!!!!
        */

        if (musicSource.isPlaying == false)
        {
            switch (playingNow)
            {
                case Utils.PlayingNow.MAINTHEME:
                playMainMenu();
                break;

                case Utils.PlayingNow.TUTORIAL:
                playTutorial();
                break;
            }
        }


        /*index
        !!!!!!!!!!!!!
        SOUND EFFECTS
        !!!!!!!!!!!!!
        */

        if (InputManager.Instance.actualInputs.recording)
        {
            if(!effectsSource.isPlaying)
            {
                playRecordingFX();
            }
        } else
        {
            if (effectsSource.isPlaying)
            {
                stopRecordingFX();
            }
        }

    }
}
