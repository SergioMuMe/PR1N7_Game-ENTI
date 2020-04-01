using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class SoundEffects
{
    public string name;
    public AudioClip clip;

    private AudioSource source;

    [Range(0.5f, 1.5f)]
    public float pitch = 1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public void setSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }

    public void Play()
    {
        //Seteamos volumen del efecto
        source.volume = OptionsManager.Instance.masterVolumenValueSaved;
        source.volume *= OptionsManager.Instance.effectsVolumenValueSaved;

        //Si randomPitch=0, pitch queda sin modificar
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch /2f));

        //Al ser distintos AudioSource se pueden ejecutar a la vez, no es necesario PlayAtOnce()
        source.Play();
    }

    public void SetTest(float _master, float _effect)
    {
        source.volume = _master;
        source.volume *= _effect;
    }

    public void PlayTest()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
    
}

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

    [SerializeField]
    public SoundEffects[] sounds;

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
        Debug.LogWarning("AudioManager> Sound not found: " + _name);
        return;
    }

    public void TestEffect(float _m, float _e)
    {
        sounds[1].SetTest(_m,_e);
        sounds[1].PlayTest();
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Stop();
        }
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }
        Debug.LogWarning("AudioManager> Sound not found: " + _name);
        return;
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


    //NOTA: Los SoundEffects se ha seguido otra logica de programación, el volumen se consulta antes de realizar Play del soundeffect.

    //Configura los audiosource con el volumen aplicado por el usuario en opciones
    public void setVolume()
    {
        //Conseguimos el volumen master
        musicSource.volume = OptionsManager.Instance.masterVolumenValueSaved;

        //Seteamos volumen para el audiosource. Este volumen es el último que se haya guardado.
        musicSource.volume *= OptionsManager.Instance.musicVolumenValueSaved;
    }

    //Modifica en tiempo real el volumen para que el usuario experimente los cambios antes de guardar configuración
    public void setVolumeTesting(float _master, float _music, float _effect)
    {
        //Conseguimos el volumen master
        musicSource.volume = _master;
        
        //Seteamos volumen para cada audiosource. Este volumen corresponde al slide de configuración.
        //si el usuario lo sube o baja, se verá reflejado sin tener que dar a guardar.
        musicSource.volume *= _music;
        
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
        //Preparamos el AudioSource de la música
        musicSource = GameObject.Find("MusicAudioSource").GetComponent<AudioSource>();
        playingNow = Utils.PlayingNow.NONE;

        //Instanciamos los efectos de sonido
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("SoundEffect_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].setSource(_go.AddComponent<AudioSource>());
        }
    }


    

    private void Update()
    {
        /*index
        !!!!!!
        MUSICA
        !!!!!!
        */

        //Loop de música
        //TODO: Mejorable, evitar uso de switch y enum? investigar statemachine.
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

    }
}
