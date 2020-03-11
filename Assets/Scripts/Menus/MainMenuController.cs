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

    private GameManager scriptGM;

    private int idProfileSelected;

    //Obtenemos los botones de los niveles;
    public GameObject[] levelButtons;
    public TextMeshProUGUI[] textUI;


    //A continuación, todos los arrays corresponden a [0]-Bloqueado [1]-Desbloqueado
    public TMP_ColorGradient[] statusLevelColor = new TMP_ColorGradient[2];
    private Image starMedal;
    private Image timeMedal;
    private Image batteryMedal;

    public Sprite[] statusStar = new Sprite[2];
    public Sprite[] statusTime = new Sprite[2];
    public Sprite[] statusBattery = new Sprite[2];



    /*index 
        ###################################
        #                                 #
        #  OBTENER NIVELES DESBLOQUEADOS  #
        #                                 #
        ###################################
    */

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
    
    private GameObject options;
    private GameObject credits;
    private GameObject levelSelection;

    private GameObject levelSelected;
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
        SceneManager.LoadScene(idLevel);
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
        levelSelected = GameObject.Find("LevelSelected");
        levelSelection = GameObject.Find("SelectLevel");
        credits = GameObject.Find("Credits");
        options = GameObject.Find("Options");

        levelSelectedTitle = GameObject.Find("LevelSelectedTitle").GetComponent<TextMeshProUGUI>();

        starMedal = GameObject.Find("starMedal").GetComponent<Image>();
        timeMedal = GameObject.Find("timeMedal").GetComponent<Image>();
        batteryMedal = GameObject.Find("batteryMedal").GetComponent<Image>();

        timeLevelLimit = GameObject.Find("LevelRecord").GetComponent<TextMeshProUGUI>();
        playerRecord = GameObject.Find("PlayerRecord").GetComponent<TextMeshProUGUI>();
        
        //...Y desactivamos menus.
        levelSelected.SetActive(false);
        levelSelection.SetActive(false);
        credits.SetActive(false);
        options.SetActive(false);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            textUI[i] = levelButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        }


        idProfileSelected = scriptGM.profileSelected;
        getLevelsStatus();
        
    }
    
}
