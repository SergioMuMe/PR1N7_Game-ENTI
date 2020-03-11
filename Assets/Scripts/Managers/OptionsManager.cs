using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    /*index
     ########################
     #                      #
     #  AUDIO OPTIONS       #
     #                      #
     ########################
 */

    public Slider masterVolumen;
    public Slider musicVolumen;
    public Slider effectsVolumen;

    private float masterVolumenValue;
    private float musicVolumenValue;
    private float effectsVolumenValue;

    private float masterVolumenValueSaved;
    private float musicVolumenValueSaved;
    private float effectsVolumenValueSaved;

    public int changesApplied;

    public void changeChangesApplied()
    {
        changesApplied++;
        saveActualValues();
    }

    //Actualiza las variables de audio al aplicar cambios.
    public void updateVolumenValues()
    {
        masterVolumenValue = masterVolumen.value;
        musicVolumenValue = musicVolumen.value;
        effectsVolumenValue = effectsVolumen.value;
    }

    //Guarda los valores previos
    public void saveActualValues()
    {
        masterVolumenValueSaved = masterVolumen.value;
        musicVolumenValueSaved = musicVolumen.value;
        effectsVolumenValueSaved = effectsVolumen.value;
        changesApplied = 0;
    }

    public void checkExitOptions()
    {
        if(changesApplied >=1)
        {
            return;
        }

        masterVolumen.value = masterVolumenValueSaved;
        musicVolumen.value = musicVolumenValueSaved;
        effectsVolumen.value = effectsVolumenValueSaved;
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

    void Start()
    {
        /*masterVolumen = GameObject.Find("MasterVolumeSlider").GetComponent<Slider>();
        musicVolumen = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
        effectsVolumen = GameObject.Find("effectsVolumeSlider").GetComponent<Slider>();*/

        changesApplied = 0;

        updateVolumenValues();
        saveActualValues();

    }

    
}
