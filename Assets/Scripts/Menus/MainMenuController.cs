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
    private GameObject mainMenu;
    private GameObject levelSelection;
    private GameObject levelSelected;
    private GameObject options;
    private GameObject credits;

    private GameManager scriptGM;

    private int idProfileSelected;



    /*index 
         ##############
         #            #
         #  PROFILES  #
         #            #
         ##############
     */


    /*
    1- Guardar info de los profiles en nombres[]
    2- A cada profile darle su nombre: profile0texto = profile.name
    3- BETA: Displayar contador de estrellas desbloqueadas
    4- Cuando se pulse sobre un profile = modificar el scriptGM.profileSelected;
    */

    private TextMeshProUGUI[] profileName;
    private void setProfileNames()
    {
        for (int i = 0; i < 3; i++)
        {
            profileName[i].text = scriptGM.profiles[i].profileName;
        }
        
    }

    private TextMeshProUGUI[] totalMaps;
    private void setTotalMaps()
    {
        for (int i = 0; i < 3; i++)
        {
            totalMaps[i].text = scriptGM.getTotalMaps(i).ToString();
            totalMaps[i].text = totalMaps[i].text + " / " + scriptGM.numberOfLevels.ToString();
        }
    }

    private TextMeshProUGUI[] totalMedals;
    private void setTotalMedals()
    {
        for (int i = 0; i < 3; i++)
        {
            totalMedals[i].text = scriptGM.getTotalMedals(i).ToString();
            totalMedals[i].text = totalMedals[i].text + " / " + (scriptGM.numberOfLevels*3).ToString();
        }
    }

    

    /*index 
        ###################################
        #                                 #
        #  OBTENER NIVELES DESBLOQUEADOS  #
        #                                 #
        ###################################
    */

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

    //Para modificar color (bloqueado/desbloqueado): Obtenemos los botones de los niveles + textos
    public GameObject[] levelButtons;
    public TextMeshProUGUI[] textUI;

    private TextMeshProUGUI levelSelectedTitle; 

    private TextMeshProUGUI timeLevelLimit;
    private TextMeshProUGUI playerRecord;

    private int idLevel;

    public void setIdLevel(int _idLevel)
    {
        idLevel = _idLevel;
    }

    public void loadLevelSelected()
    {
        levelSelectedTitle.text = "LEVEL " + idLevel;

        if(scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.finished)
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
        
        timeLevelLimit.text = "Level record: " + scriptGM.timeLevelLimit[idLevel] + " sec";
        
        if (scriptGM.profiles[idProfileSelected].levelsData[idLevel].firstTimeFLAG)
        {
            playerRecord.text = "Player record: -- sec";
        } else
        {
            playerRecord.text = "Player record: " + Utils.RoundFloat(scriptGM.profiles[idProfileSelected].levelsData[idLevel].levelMedals.timeRecord, 2) + " sec";
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
        if(idLevel == 999)
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


        idProfileSelected = scriptGM.profileSelected;

        //Referencias de los profiles
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
        setProfileNames();
        setTotalMedals();
        setTotalMaps();

        //Habilitamos los niveles desbloqueados
        getLevelsStatus();
        

        //Otras referencias
        for (int i = 0; i < levelButtons.Length; i++)
        {
            textUI[i] = levelButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        }


        //...Y desactivamos menus.
        //profileSelection.SetActive(false);
        mainMenu.SetActive(false);
        levelSelected.SetActive(false);
        levelSelected.SetActive(false);
        levelSelection.SetActive(false);
        credits.SetActive(false);
        options.SetActive(false);


    }
    
}
