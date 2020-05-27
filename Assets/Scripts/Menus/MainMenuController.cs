using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{

    /*index 
        #############
        #           #
        #  GENERAL  #
        #           #
        #############
    */

    private GameObject mainMenu;
    private GameObject levelSelection;
    private GameObject levelSelected;
    private GameObject options;
    private GameObject credits;

    private int idProfileSelected;

    private GameManager scriptGM;

    private EventTrigger[] vectorButtons;

    /*index 
        ##############
        #            #
        #  OPCIONES  #
        #            #
        ##############
    */

    #region OPCIONES

    public Slider masterVolumen;
    public Slider musicVolumen;
    public Slider effectsVolumen;

    private TextMeshProUGUI masterValueDisplay;
    private TextMeshProUGUI musicValueDisplay;
    private TextMeshProUGUI effectsValueDisplay;

    //Descripción de las funciones en OptionsManager.Instance
    public void saveActualValues()
    {
        OptionsManager.Instance.saveActualValues(masterVolumen.value, musicVolumen.value, effectsVolumen.value);
    }

    public void checkExitOptions()
    {
        OptionsManager.Instance.checkExitOptions();
    }

    public void testSoundEffect()
    {
        SoundManager.Instance.TestEffect(masterVolumen.value, effectsVolumen.value);
    }


    #endregion

    /*index 
        ########################
        #                      #
        #  GESTIÓN DE NIVELES  #
        #                      #
        ########################
    */
    #region GESTION_DE_NIVELES

    //Para modificar color (bloqueado/desbloqueado): Obtenemos los botones de los niveles + textos
    public GameObject[] levelButtons;
    public TextMeshProUGUI[] textUI;

    //Detecta los niveles desbloqueados por el profile actual, y los desbloquea en la seleccion de niveles.
    private void getLevelsStatus()
    {
        for (int j = 0; j < scriptGM.numberOfLevels; j++)
        {
            levelButtons[j].GetComponent<Button>().interactable = scriptGM.profiles[idProfileSelected].levelsData[j].levelUnblockedFLAG;

            if (levelButtons[j].GetComponent<Button>().IsInteractable())
            {
                textUI[j].colorGradientPreset = statusLevelColor[1];
            }
        }
    }

    //Al cambiar de perfil, resetea el render de los niveles.
    public void resetLevelsStatus()
    {
        for (int j = 0; j < scriptGM.numberOfLevels; j++)
        {
            levelButtons[j].GetComponent<Button>().interactable = false;

            if (!levelButtons[j].GetComponent<Button>().IsInteractable())
            {
                textUI[j].colorGradientPreset = statusLevelColor[0];
            }
        }
    }
    #endregion
    /*index 
        ####################################
        #                                  #
        #  MODIFICAR LEVEL SELECTED INFO   #
        #                                  #
        ####################################
    */
    #region LEVEL_SELECTED_INFO


    //GameObject IMAGEN de la medalla de cada nivel
    //CÓDIGO PARA USAR MEDALLAS 2D
    private Image starMedal;
    private Image timeMedal;
    private Image batteryMedal;

    //CÓDIGO PARA USAR MEDALLAS 3D
    //private MeshRenderer starMedal;
    //private MeshRenderer timeMedal;
    //private MeshRenderer batteryMedal;

    //A continuación, todos los arrays corresponden a [0]-Bloqueado [1]-Desbloqueado
    public TMP_ColorGradient[] statusLevelColor = new TMP_ColorGradient[2];

    //CÓDIGO PARA USAR MEDALLAS 2D
    public Sprite[] statusStar = new Sprite[2];
    public Sprite[] statusTime = new Sprite[2];
    public Sprite[] statusBattery = new Sprite[2];

    //CÓDIGO PARA USAR MEDALLAS 3D
    //public Material[] statusTime = new Material[3];
    //public Material[] statusStar = new Material[3];
    //public Material[] statusBattery = new Material[3];

    private TextMeshProUGUI levelSelectedTitle;

    private TextMeshProUGUI timeLevelLimit;
    private TextMeshProUGUI playerRecord;

    public int idLevel;

    public void setIdLevel(int _idLevel)
    {
        GameManager.Instance.idActualLevel = _idLevel;
        idLevel = _idLevel;
    }


    private void loadMedalMaterial(bool _medal, MeshRenderer _meshRenderer, Material[] _materials)
    {
        if (_medal && scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.allAtOnce)
        {
            _meshRenderer.material = _materials[2];
        }
        else if (_medal)
        {
            _meshRenderer.material = _materials[1];
        }
        else
        {
            _meshRenderer.material = _materials[0];
        }
    }


    public void loadLevelSelected()
    {
        levelSelectedTitle.text = "LEVEL " + idLevel;

        timeLevelLimit.text = "Level record:  " + Utils.GetTimeFormat(scriptGM.timeLevelLimit[idLevel], 3);

        if (scriptGM.profiles[idProfileSelected].levelsData[idLevel].firstTimeFLAG)
        {
            playerRecord.text = "Player record: --:--:--- ";
        }
        else
        {
            playerRecord.text = "Player record: " + Utils.GetTimeFormat(Utils.RoundFloat(scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.timeRecord, 3), 3);
        }

        if (scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.allAtOnce)
        {
            starMedal.sprite = statusStar[2];
            timeMedal.sprite = statusTime[2];
            batteryMedal.sprite = statusBattery[2];
            return; //El resto de código es para obtener medallas negras/plateadas. No lo necesitamos si ya tenemos la de oro!
        }


        //CÓDIGO PARA USAR MEDALLAS 3D
        //loadMedalMaterial(scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.finished, timeMedal, statusTime);
        //loadMedalMaterial(scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.timeBeated, timeMedal, statusTime);
        //loadMedalMaterial(scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.batteryCollected, timeMedal, statusTime);

        //CÓDIGO PARA USAR MEDALLAS 2D
        if (scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.finished)
        {
            starMedal.sprite = statusStar[1];
        } else
        {
            starMedal.sprite = statusStar[0];
        }

        if (scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.timeBeated)
        {
            timeMedal.sprite = statusTime[1];
        }
        else
        {
            timeMedal.sprite = statusTime[0];
        }

        if (scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.batteryCollected)
        {
            batteryMedal.sprite = statusBattery[1];
        } else
        {
            batteryMedal.sprite = statusBattery[0];
        }



    }
    #endregion


    /*index
        #############
        #           #
        #  SONIDOS  #
        #           #
        #############
    */
    #region SONIDOS
    public void PlayButton()
    {
        SoundManager.Instance.PlaySound("MENU-MouseOver");
    }

    public void PlayImportantButton()
    {
        SoundManager.Instance.PlaySound("MENU-ImportantButton");
    }

    public void PlayProfileSelected()
    {
        SoundManager.Instance.PlaySound("MENU-ProfileSelected");
    }
    #endregion

    /*index
        ####################
        #                  #
        #  OTRAS OPCIONES  #
        #                  #
        ####################
    */

    public void changeProfile()
    {
        SceneManager.LoadScene("Login");
    }

    public void playLevel()
    {
        if (idLevel == 999)
        {
            int testRoom = 12;
            SceneManager.LoadScene(testRoom);
        } else
        {
            SceneManager.LoadScene(idLevel);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    /*index
        ########################
        #                      #
        #  FUNCIONES DE UNITY  #
        #                      #
        ########################
    */

    private void Start()
    {
        scriptGM = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Obtenemos referencias...
        mainMenu = GameObject.Find("MainMenu");
        levelSelection = GameObject.Find("SelectLevel");
        levelSelected = GameObject.Find("LevelSelected");
        credits = GameObject.Find("Credits");
        options = GameObject.Find("Options");

        levelSelectedTitle = GameObject.Find("LevelSelectedTitle").GetComponent<TextMeshProUGUI>();

        starMedal = GameObject.Find("starMedal").GetComponent<Image>();
        timeMedal = GameObject.Find("timeMedal").GetComponent<Image>();
        batteryMedal = GameObject.Find("batteryMedal").GetComponent<Image>();

        timeLevelLimit = GameObject.Find("LevelRecord").GetComponent<TextMeshProUGUI>();
        playerRecord = GameObject.Find("PlayerRecord").GetComponent<TextMeshProUGUI>();

        //Referencias y seteos de opciones
        masterVolumen = GameObject.Find("MasterVolumeSlider").GetComponent<Slider>();
        musicVolumen = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
        effectsVolumen = GameObject.Find("EffectsVolumeSlider").GetComponent<Slider>();

        masterValueDisplay = GameObject.Find("masterValueDisplay").GetComponent<TextMeshProUGUI>();
        musicValueDisplay = GameObject.Find("musicValueDisplay").GetComponent<TextMeshProUGUI>();
        effectsValueDisplay = GameObject.Find("effectsValueDisplay").GetComponent<TextMeshProUGUI>();

        masterVolumen.value = OptionsManager.Instance.masterVolumenValueSaved;
        musicVolumen.value = OptionsManager.Instance.musicVolumenValueSaved;
        effectsVolumen.value = OptionsManager.Instance.effectsVolumenValueSaved;

        //Referencias de los profiles
        idProfileSelected = scriptGM.profileSelected;

        //Habilitamos los niveles desbloqueados
        getLevelsStatus();

        //Otras referencias
        for (int i = 0; i < levelButtons.Length; i++)
        {
            textUI[i] = levelButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        //Seteamos los menus mostrados, mostramos mainMenu.SetActive(true);
        if (GameManager.Instance.profilePicked)
        {
            levelSelected.SetActive(false);
            levelSelection.SetActive(false);
            credits.SetActive(false);
            options.SetActive(false);

        } else
        {
            Debug.LogError("ERROR: No profilePicked.");
        }
        
        //Actualizamos valores iniciales de OptionsManager
        saveActualValues();

        //Play main menu music
        if(SoundManager.Instance.playingNow != Utils.PlayingNow.MAINTHEME)
        {
            SoundManager.Instance.setMusicNull();
        }
        SoundManager.Instance.playingNow = Utils.PlayingNow.MAINTHEME;
    }

    private void Update()
    {
        masterValueDisplay.text = Utils.GetPercentage(masterVolumen.value, 0).ToString() + "%";
        musicValueDisplay.text = Utils.GetPercentage(musicVolumen.value, 0).ToString() + "%";
        effectsValueDisplay.text = Utils.GetPercentage(effectsVolumen.value, 0).ToString() + "%";

        SoundManager.Instance.setVolumeTesting(masterVolumen.value, musicVolumen.value, effectsVolumen.value);
    }
}
