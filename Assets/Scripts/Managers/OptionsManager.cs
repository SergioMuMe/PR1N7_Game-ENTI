using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    /*index
     ####################
     #                  #
     #  AUDIO OPTIONS   #
     #                  #
     ####################
    */

    private MainMenuController scriptMMC;
    
    public float masterVolumenValueSaved;
    public float musicVolumenValueSaved;
    public float effectsVolumenValueSaved;

    //Al aplicar cambios: Valores guardados de las opciones
    public void saveActualValues(float _masterV, float _musicV, float _effectsV)
    {
        masterVolumenValueSaved = _masterV;
        musicVolumenValueSaved = _musicV;
        effectsVolumenValueSaved = _effectsV;
        SoundManager.Instance.setVolume();
    }
    
    //Al salir: Si el jugador ha realizado cambios y no los ha guardado, se restauran los valores.
    //TODO: Poner un popup de ¿Salir sin guardar cambios? [Aplicar cambios], [Salir]. Para conseguir el popup, comparar SavedValues con ActualValues del slider
    public void checkExitOptions()
    {    
        scriptMMC = GameObject.Find("MainMenuManager").GetComponent<MainMenuController>();

        scriptMMC.masterVolumen.value = masterVolumenValueSaved;
        scriptMMC.musicVolumen.value = musicVolumenValueSaved;
        scriptMMC.effectsVolumen.value = effectsVolumenValueSaved;
    }

    /*index
    ########################
    #                      #
    #  FUNCIONES DE UNITY  #
    #                      #
    ########################
    */

    // Instanciar OptionsManager
    public static OptionsManager Instance { get; private set; }

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
        masterVolumenValueSaved = 0.5f;
        musicVolumenValueSaved = 0.5f;
        effectsVolumenValueSaved = 0.5f;
    }
}
