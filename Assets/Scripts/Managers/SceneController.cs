using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /*index
        ############################
        #                          #
        #  CONTROL CAMBIO ESCENAS  #
        #                          #
        ############################
    */
    public string nextScene;
    private string actualScene;

    HUDController canvasHUD;

    //PROFE: ¿Porque entra dos veces en el trigger?
    private bool waltrapa;

    private void restartScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void loadNextScene(string scene)
    {
        GameManager.Instance.idActualLevel++;
        setLevelResults();
        sendLevelResults();
        SceneManager.LoadScene(scene);
    }

    /*index
        ############################
        #                          #
        #  END GAME SPLASH SCREEN  #
        #                          #
        ############################
    */

    #region END_GAME_SPLASH_SCREEN

    TextMeshProUGUI recordTimeEG;
    TextMeshProUGUI playerTimeEG;

    Button goMenuButton;
    Button restartButton;
    Button nextLevelButton;

    GameObject canvasEndGame;

    // Hacemos flash del numero para indicar que se aproxima al tiempo record
    private float timeFlashing;
    private float doFlashAt;
    private TextMeshProUGUI newRecordText;

    //Al terminar un nivel, podemos reiniciar escena pero conservamos el progreso
    public void restartSceneEndGame()
    {
        setLevelResults();
        sendLevelResults();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Al terminar un nivel, podemos ir al main menu pero conservamos el progreso
    public void goHomeEndGame()
    {
        setLevelResults();
        sendLevelResults();
        Utils.GoMainMenu();
    }

    //Al terminar un nivel, cargamos next level y conservamos el progreso
    public void loadNextSceneEndGame()
    {
        loadNextScene(nextScene);
    }

    //Aquí seteamos el inicio de la logica de las animaciones
    private void endGameAnimations()
    {
        playerTime = canvasHUD.playerTime;
        
        playerTimeEG.text = Utils.GetTimeFormat(Utils.RoundFloat(playerTime, 3), 1);
    }
    #endregion

    /*index
        ##########################
        #                        #
        #  CONFIGURACIÓN ESCENA  #
        #                        #
        ##########################
    */

    #region CONFIGURACION_ESCENA

    int profileSelected; //Para acceder a datos del perfil.

    public int idLevel; //Para informar al GameManager de que nivel és.
    public int batteryLevelCount; // Cantidad de pilas en el nivel
    private float playerTime; // En Segundos. Tiempo que tarda el jugador en superar el nivel
    private float timeLevelLimit; // En Segundos. Tiempo objetivo del nivel para ganar la estrella del tiempo
    private float timePlayerRecord; // En Segundos. Tiempo record del jugador. 

    private GameManager scriptGM; //Obtenemos el GameManager para ir almacenando los datos de la sesion de juego

    //Info enviada al GameManager para guardar resultados del nivel. Previamente obtenemos la info actual.
    Medals sceneMedals;
   

    //Obtenemos datos del jugador relacionados con este mapa
    private void getPlayerLevelInfo()
    {
        bool firstTimeFLAG;
        firstTimeFLAG = scriptGM.getFirstTimeFLAG(idLevel);

        profileSelected = scriptGM.profileSelected;

        sceneMedals = getPlayerLevelMedalsInfo();

    }
    private Medals getPlayerLevelMedalsInfo()
    {
        return scriptGM.getLevelMedals(idLevel);
    }

    
    

    //Esta función es llamada justo antes de cambiar de nivel
    private void setLevelResults() {
        
        sceneMedals.finished = true;

        //Si jugador NO ha obtenido aun la medalla
        if (!sceneMedals.batteryCollected)
        {
            if (batteryLevelCount == 0)
            {
                sceneMedals.batteryCollected = true;
            }
            else
            {
                sceneMedals.batteryCollected = false;
            }
        }

        //Si jugador NO ha obtenido aun la medalla
        if (!sceneMedals.timeBeated)
        {
            if (playerTime <= timeLevelLimit)
            {
                sceneMedals.timeBeated = true;
            }
            else
            {
                sceneMedals.timeBeated = false;
            }
        }

        if(playerTime < sceneMedals.timeRecord)
        {
            //Obtiene nuevo record
            sceneMedals.timeRecord = playerTime;
        } //ELSE Mantiene el record anterior
          
    }
    
    //Esta función es llamada justo antes de cambiar de nivel.
    private void sendLevelResults() {
        scriptGM.saveData(idLevel, sceneMedals);
    }
    #endregion

    /*index
        ########################
        #                      #
        #  FUNCIONES DE UNITY  #
        #                      #
        ########################
    */
    
    private void Start()
    {

        doFlashAt = 650f;

        //Play main menu music
        //TODO: Mejorar la gestion del enum, ¿musica por mundos?
        if (SoundManager.Instance.playingNow != Utils.PlayingNow.TUTORIAL)
        {
            SoundManager.Instance.setMusicNull();
            SoundManager.Instance.playingNow = Utils.PlayingNow.TUTORIAL;
        }

        //Obtenemos referencias e información
        scriptGM = GameObject.Find("GameManager").GetComponent<GameManager>();
        actualScene = SceneManager.GetActiveScene().name;
        getPlayerLevelInfo();
        timeLevelLimit = Utils.GetActualRecord(sceneMedals.timeRecord,scriptGM.timeLevelLimit[idLevel]);

        //Datos para el end game splash screen

        recordTimeEG = GameObject.Find("T-RecordTime").GetComponent<TextMeshProUGUI>();
        recordTimeEG.text = Utils.GetTimeFormat(Utils.RoundFloat(timeLevelLimit, 3), 1);
        playerTimeEG = GameObject.Find("T-Time").GetComponent<TextMeshProUGUI>();

        newRecordText = GameObject.Find("T-NewRecord").GetComponent<TextMeshProUGUI>();

        //Configuramos los botones de la splashscreen        
        goMenuButton = GameObject.Find("B-goMenuButton").GetComponent<Button>();
        goMenuButton.onClick.AddListener(goHomeEndGame);

        restartButton = GameObject.Find("B-restartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(restartSceneEndGame);

        nextLevelButton = GameObject.Find("B-nextLevelButton").GetComponent<Button>();
        nextLevelButton.onClick.AddListener(loadNextSceneEndGame);
        

        //referencia del hud
        canvasHUD = GameObject.Find("CanvasHUD").GetComponent<HUDController>();

        //Nos aseguramos de que el idLevel se ha seteado correctamente
        //PROFE: Este valor lo usa HUDController.cs, al cambiar de nivel, se ejecuta antes ese script y por lo tanto no llega a actualizar a tiempo el idLevel ¿solucion?
        //GameManager.Instance.idActualLevel = idLevel;

        //controles de fin del mapa.
        //TODO: Revisar waltrapa, ¿porque entra dos veces en el trigger de NextLevel?
        waltrapa = true;

        //Cogemos referencia del end game splashscreen y lo ocultamos
        canvasEndGame = GameObject.Find("CanvasEndGame");
        canvasEndGame.SetActive(false);
    }

    //TODO: Esto lo estmaos usando para debugar. Terminar de implementar para jugador o quitar.
    private void Update()
    {
        /*index
        !!!!!!!!!!!!!!!!
        DEV TESTING KEYS
        !!!!!!!!!!!!!!!!
        */
        if (Input.GetKey(KeyCode.P))
        {
            restartScene(actualScene);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Utils.GoMainMenu();
        }

        if(Input.GetKey(KeyCode.O))
        {
            loadNextScene(nextScene);
        }

        /*index
        !!!!!!!!!!!!!!!!!!
        END GAME ANIMATION
        !!!!!!!!!!!!!!!!!!
        */

        //Parpadeo de NEW RECORD ! en caso de superar tiempo record
        if (canvasHUD.levelEnded)
        {
            if (playerTime < timeLevelLimit)
            {
                timeFlashing += Time.deltaTime * 1000;

                if (timeFlashing > doFlashAt)
                {
                    newRecordText.enabled = true;
                    if (timeFlashing > doFlashAt * 2)
                    {
                        timeFlashing = 0;
                    }
                }
                else
                {
                    newRecordText.enabled = false;
                }
            }
            else //NO ha superado el record
            {
                newRecordText.enabled = false;
            }

        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (waltrapa && collision.tag == "Player")
        {
            waltrapa = false;

            Debug.LogWarning("TODO: arreglar waltrapa");
            collision.enabled = false;

            //TODO: bloquear Inputs de jugador
            Debug.LogWarning("TODO: bloquear Inputs de jugador.");

            canvasEndGame.SetActive(true);
            endGameAnimations();
            canvasHUD.levelEnded = true;
            
        }
    }
}
