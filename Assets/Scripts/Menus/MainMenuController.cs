using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    /*index 
        #############
        #           #
        #  GENERAL  #
        #           #
        #############
    */

    private GameObject profileSelection;
    private GameObject createProfile;
    private GameObject mainMenu;
    private GameObject levelSelection;
    private GameObject levelSelected;
    private GameObject options;
    private GameObject credits;

    private GameManager scriptGM;

    /*index 
        ##############
        #            #
        #  OPCIONES  #
        #            #
        ##############
    */


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



    /*index 
         ##############
         #            #
         #  PROFILES  #
         #            #
         ##############
     */

    private int idProfileSelected;

    //Obtenemos nombres de perfiles
    private TextMeshProUGUI[] profileName;
    private void setProfileNames()
    {
        for (int i = 0; i < 3; i++)
        {
            profileName[i].text = scriptGM.profiles[i].profileName;
        }

    }

    //Obtenemos total mapas superados
    private TextMeshProUGUI[] totalMaps;
    private void setTotalMaps()
    {
        for (int i = 0; i < 3; i++)
        {
            totalMaps[i].text = scriptGM.getTotalMaps(i).ToString();
            totalMaps[i].text = totalMaps[i].text + " / " + scriptGM.numberOfLevels.ToString();
        }
    }
    //Obtenemos total medallas conseguidas
    private TextMeshProUGUI[] totalMedals;
    private void setTotalMedals()
    {
        for (int i = 0; i < 3; i++)
        {
            totalMedals[i].text = scriptGM.getTotalMedals(i).ToString();
            totalMedals[i].text = totalMedals[i].text + " / " + (scriptGM.numberOfLevels * 3).ToString();
        }
    }

    //Cargamos en pantalla la informacion de los profiles locales
    public void loadProfileSelectionData()
    {
        setProfileNames();
        setTotalMedals();
        setTotalMaps();
    }


    //Seteamos profile seleccionado, en caso de no existir creamos profile nuevo
    public void resetGMProfileVariables()
    {
        GameManager.Instance.profileSelected = 999;
        GameManager.Instance.profilePicked = false;
    }

    //Seteamos profile seleccionado, en caso de no existir creamos profile nuevo
    public void setProfileSelected(int idSelected)
    {
        GameManager.Instance.profileSelected = idSelected;
        GameManager.Instance.profilePicked = true;
        getProfileSelected();

        if (GameManager.Instance.profiles[idSelected].profileUsed)
        {
            profileSelection.SetActive(false);
            getLevelsStatus();
            mainMenu.SetActive(true);
        } else
        {
            profileSelection.SetActive(false);
            createProfile.SetActive(true);
        }
    }

    //Parche para código antiguo. Usamos alguna variable local que obtenemos del gameManager.
    public void getProfileSelected()
    {
        idProfileSelected = GameManager.Instance.profileSelected;
    }

    //Creación de nuevo profile binario local
    public void createNewProfileBIN()
    {
        int idProfile = GameManager.Instance.profileSelected;
        string name = GameObject.Find("NameValue").GetComponent<TMP_InputField>().text;

        //Definimos datos del nuevo perfil
        GameManager.Instance.profiles[idProfile].profileUsed = true;
        GameManager.Instance.profiles[idProfile].profileName = name;

        //Actualizamos perfil
        GameManager.Instance.saveDataInProfileBIN();

        //Cargamos datos del jugador
        GameManager.Instance.loadProfiles(idProfile);
        getLevelsStatus();

        //Avanzamos al mainmenu
        createProfile.SetActive(false);
        mainMenu.SetActive(true);
    }



    /*index 
        ########################
        #                      #
        #  GESTIÓN DE NIVELES  #
        #                      #
        ########################
    */


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
        Debug.Log("getLevelsStatus for player id: " + idProfileSelected);
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

    /*index 
        ####################################
        #                                  #
        #  MODIFICAR LEVEL SELECTED INFO   #
        #                                  #
        ####################################
    */

    //GameObject IMAGEN de la medalla de cada nivel
    private Image starMedal;
    private Image timeMedal;
    private Image batteryMedal;

    //A continuación, todos los arrays corresponden a [0]-Bloqueado [1]-Desbloqueado
    public TMP_ColorGradient[] statusLevelColor = new TMP_ColorGradient[2];

    public Sprite[] statusStar = new Sprite[2];
    public Sprite[] statusTime = new Sprite[2];
    public Sprite[] statusBattery = new Sprite[2];

    private TextMeshProUGUI levelSelectedTitle;

    private TextMeshProUGUI timeLevelLimit;
    private TextMeshProUGUI playerRecord;

    public int idLevel;

    public void setIdLevel(int _idLevel)
    {
        GameManager.Instance.idActualLevel = _idLevel;
        idLevel = _idLevel;
    }

    public void loadLevelSelected()
    {
        levelSelectedTitle.text = "LEVEL " + idLevel;

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
        } else
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

        timeLevelLimit.text = "Level record:  " + Utils.GetTimeFormat(scriptGM.timeLevelLimit[idLevel], 1);

        if (scriptGM.profiles[idProfileSelected].levelsData[idLevel].firstTimeFLAG)
        {
            playerRecord.text = "Player record: --:--:--- ";
        } else
        {
            playerRecord.text = "Player record: " + Utils.GetTimeFormat(Utils.RoundFloat(scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.timeRecord, 3), 1);
        }

    }

    /*index
        ####################
        #                  #
        #  OTRAS OPCIONES  #
        #                  #
        ####################
    */


    public void playLevel()
    {
        if (idLevel == 999)
        {
            string devRoom = "DevelopRoom";
            SceneManager.LoadScene(devRoom);
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
        profileSelection = GameObject.Find("ProfileSelection");
        createProfile = GameObject.Find("CreateProfile");
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
        OptionsManager.Instance.masterVolumenValueSaved = masterVolumen.value;
        OptionsManager.Instance.musicVolumenValueSaved = musicVolumen.value;
        OptionsManager.Instance.effectsVolumenValueSaved = effectsVolumen.value;

        //Referencias de los profiles

        idProfileSelected = scriptGM.profileSelected;

        profileName = new TextMeshProUGUI[3];
        profileName[0] = GameObject.Find("NombreProfile0").GetComponent<TextMeshProUGUI>();
        profileName[1] = GameObject.Find("NombreProfile1").GetComponent<TextMeshProUGUI>();
        profileName[2] = GameObject.Find("NombreProfile2").GetComponent<TextMeshProUGUI>();

        totalMedals = new TextMeshProUGUI[3];
        totalMedals[0] = GameObject.Find("MedalsProfile0").GetComponent<TextMeshProUGUI>();
        totalMedals[1] = GameObject.Find("MedalsProfile1").GetComponent<TextMeshProUGUI>();
        totalMedals[2] = GameObject.Find("MedalsProfile2").GetComponent<TextMeshProUGUI>();

        totalMaps = new TextMeshProUGUI[3];
        totalMaps[0] = GameObject.Find("LevelsProfile0").GetComponent<TextMeshProUGUI>();
        totalMaps[1] = GameObject.Find("LevelsProfile1").GetComponent<TextMeshProUGUI>();
        totalMaps[2] = GameObject.Find("LevelsProfile2").GetComponent<TextMeshProUGUI>();

        //Cargamos datos a displayar de los profiles
        loadProfileSelectionData();

        //Habilitamos los niveles desbloqueados
        getLevelsStatus();


        //Otras referencias
        for (int i = 0; i < levelButtons.Length; i++)
        {
            textUI[i] = levelButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        }


        //...Y desactivamos menus.
        if (GameManager.Instance.profilePicked)
        {
            // El jugador ya habia seleccionado un perfil, venimos de pulsar ESC ingame
            mainMenu.SetActive(true);

            profileSelection.SetActive(false);
            createProfile.SetActive(false);
            levelSelected.SetActive(false);
            levelSelected.SetActive(false);
            levelSelection.SetActive(false);
            credits.SetActive(false);
            options.SetActive(false);
        } else
        {
            // Entramos por primera vez al juego, venimos de la SplashScreen
            profileSelection.SetActive(true);

            mainMenu.SetActive(false);
            createProfile.SetActive(false);
            levelSelected.SetActive(false);
            levelSelected.SetActive(false);
            levelSelection.SetActive(false);
            credits.SetActive(false);
            options.SetActive(false);
        }

        //Actualizamos valores iniciales de OptionsManager
        saveActualValues();

        //Play main menu music
        SoundManager.Instance.playingNow = Utils.PlayingNow.MAINTHEME;
    }

    private void Update()
    {
        //masterValueDisplay.text = GetPercentage
        //musicValueDisplay.text = 
        //effectsValueDisplay.text = 
    }  
}
