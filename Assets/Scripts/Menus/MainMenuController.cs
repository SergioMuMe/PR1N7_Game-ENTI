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

    private int profileSelected;

    //Obtenemos los botones de los niveles;
    public GameObject[] levelButtons;
    public TextMeshProUGUI[] textUI;


    //A continuación, todos los arrays corresponden a [0]-Bloqueado [1]-Desbloqueado
    public TMP_ColorGradient[] statusLevelColor = new TMP_ColorGradient[2];
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
        //for (int j = 0; j < scriptGM.profiles[profileSelected].levelsData.Length; j++)
        //{
        //    Debug.LogWarning("Unbloqued Map [" + j + "]: " + scriptGM.profiles[profileSelected].levelsData[j].levelUnblockedFLAG);
        //}
            for (int j = 0; j < scriptGM.profiles[profileSelected].levelsData.Length; j++)
        {
            
            levelButtons[j].GetComponent<Button>().interactable = scriptGM.profiles[profileSelected].levelsData[j].levelUnblockedFLAG;

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

    public GameObject levelSelected;
    private TextMeshProUGUI levelSelectedTitle;

    private SpriteRenderer starSymbol;
    private SpriteRenderer cronometerSymbol;
    private SpriteRenderer batterySymbol;

    private TextMeshProUGUI playerRecord;

    private int idLevel;

    public void setIdLevel(int _idLevel)
    {
        idLevel = _idLevel;
    }

    public void loadLevelSelected()
    {
        levelSelectedTitle.text = "LEVEL " + idLevel;

        if(scriptGM.profiles[profileSelected].levelsData[idLevel].finished)
        {
            starSymbol.sprite = statusStar[1];
        }
        if (scriptGM.profiles[profileSelected].levelsData[idLevel].timeBeated)
        {
            cronometerSymbol.sprite = statusTime[1];
        }
        if (scriptGM.profiles[profileSelected].levelsData[idLevel].batteryCollected)
        {
            batterySymbol.sprite = statusBattery[1];
        }

        if(scriptGM.profiles[profileSelected].levelsData[idLevel].firstTimeFLAG)
        {
            playerRecord.text = "Player record: -- sec";
        } else
        {
            playerRecord.text = "Player record: " + scriptGM.profiles[profileSelected].levelsData[idLevel].timeRecord + " sec";
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
        Debug.Log("Quit");
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
        levelSelectedTitle = GameObject.Find("LevelSelectedTitle").GetComponent<TextMeshProUGUI>();

        starSymbol = GameObject.Find("starSymbol").GetComponent<SpriteRenderer>();
        cronometerSymbol = GameObject.Find("cronometerSymbol").GetComponent<SpriteRenderer>();
        batterySymbol = GameObject.Find("batterySymbol").GetComponent<SpriteRenderer>();

        playerRecord = GameObject.Find("PlayerRecord").GetComponent<TextMeshProUGUI>();
        //...Y desactivamos menu.
        levelSelected.SetActive(false);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            textUI[i] = levelButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        //TESTING ZONE BEGIN
        profileSelected = 0;
        getLevelsStatus();
        //TESTING ZONE END
    }

    
}
