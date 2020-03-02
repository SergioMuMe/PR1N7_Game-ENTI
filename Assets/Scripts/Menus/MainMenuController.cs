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
    public Button[] levelButtons;
    private TextMeshProUGUI[] textUI;


    //A continuación, todos los arrays corresponden a [0]-Bloqueado [1]-Desbloqueado
    public TMP_ColorGradient[] statusLevelColor = new TMP_ColorGradient[2];
    public Image[] statusStar = new Image[2];
    public Image[] statusTime = new Image[2];
    public Image[] statusBattery = new Image[2];

    public void LoadLevel(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }


    public void LoadProfileInfo()
    {

    }

    /*index 
        ###################################
        #                                 #
        #  OBTENER NIVELES DESBLOQUEADOS  #
        #                                 #
        ###################################
    */

    private void getLevelsStatus()
    {
        for (int j = 0; j < scriptGM.profiles[profileSelected].levelsData.Length; j++)
        {
            levelButtons[j].interactable = scriptGM.profiles[profileSelected].levelsData[j].levelUnblockedFLAG;
            if (levelButtons[j].IsInteractable())
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

    private SpriteRenderer starSymbol;
    private SpriteRenderer cronometerSymbol;
    private SpriteRenderer batterySymbol;

    private TextMeshProUGUI levelRecord;

    private void loadLevelSelected()
    {
        
    }

    /*index
        ####################
        #                  #
        #  OTRAS OPCIONES  #
        #                  #
        ####################
    */

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

        starSymbol = GameObject.Find("starSymbol").GetComponent<SpriteRenderer>();
        cronometerSymbol = GameObject.Find("cronometerSymbol").GetComponent<SpriteRenderer>();
        batterySymbol = GameObject.Find("batterySymbol").GetComponent<SpriteRenderer>();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            textUI[i] = levelButtons[i].GetComponent<TextMeshProUGUI>();
        }

        //TESTING ZONE BEGIN
        profileSelected = 0;
        //TESTING ZONE END
    }

    
}
