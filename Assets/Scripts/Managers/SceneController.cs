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

    //PROFE: En el level 2, si le das al boton de next level llama a la funcion loadnextscene 2 veces! wtf
    private bool waltrapa2;

    private void restartScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void loadNextScene(string scene)
    {
        if(waltrapa2)
        {
            GameManager.Instance.idActualLevel++;
            waltrapa2 = false;
        }
        
        SoundManager.Instance.StopAllSounds();
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

    TextMeshProUGUI levelNameEG;
    TextMeshProUGUI recordTimeEG;
    TextMeshProUGUI playerTimeEG;

    Button goMenuButtonEG;
    Button restartButtonEG;
    Button nextLevelButtonEG;

    GameObject canvasEndGame;


    // Display de medallas
    private float timeDisplayMedals;
    private float doDisplayMedal;
    private int[] sprNum;
    private int nextSprite;

    //CÓDIGO PARA USAR MEDALLAS 2D
    private Image starMedal;
    private Image timeMedal;
    private Image batteryMedal;

    //CÓDIGO PARA USAR MEDALLAS 3D
    //private MeshRenderer starMedal;
    //private MeshRenderer timeMedal;
    //private MeshRenderer batteryMedal;

    //public Material[] statusStar = new Material[3];
    public Sprite[] statusStar = new Sprite[3];
    public Material[] statusTime = new Material[3];
    public Sprite[] statusBattery = new Sprite[3];
    //public Material[] statusBattery = new Material[3];


    // Flash de NEW RECORD !
    private float timeFlashing;
    private float doFlashAt;
    private TextMeshProUGUI newRecordText;

    //Al terminar un nivel, podemos reiniciar escena pero conservamos el progreso
    public void restartSceneEndGame()
    {
        if(GameManager.Instance.isGamePaused)
        {
            resumeGame();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Al terminar un nivel, podemos ir al main menu pero conservamos el progreso
    public void goHomeEndGame()
    {
        if (GameManager.Instance.isGamePaused)
        {
            resumeGame();
        }
        Utils.GoMainMenu();
    }

    //Al terminar un nivel, cargamos next level y conservamos el progreso
    public void loadNextSceneEndGame()
    {
        loadNextScene(nextScene);
    }

    private void endGamePreparations()
    {
        playerTime = canvasHUD.playerTime;
        playerTimeEG.text = Utils.GetTimeFormat(Utils.RoundFloat(playerTime, 3), 3);
    }
    #endregion

    /*index
        #######################
        #                     #
        #  MENU INGAME/PAUSE  #
        #                     #
        #######################
    */
    #region MENU_INGAME

    Button resumeButtonMI;
    Button restartButtonMI;
    Button goMenuButtonMI;

    GameObject pauseMenuMI;

    public static bool isGamePaused;

    void resumeGame()
    {
        pauseMenuMI.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.isGamePaused = false;

        if (canvasHUD.levelEnded && !canvasEndGame.activeInHierarchy)
        {
            canvasEndGame.SetActive(true);
        }

    }

    void pauseGame()
    {
        pauseMenuMI.SetActive(true);
        Time.timeScale = 0f;
        GameManager.Instance.isGamePaused = true;

        if(canvasHUD.levelEnded && canvasEndGame.activeInHierarchy)
        {            
            canvasEndGame.SetActive(false);
        }
         
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
    Medals sceneMedalsBeforeAlter;


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

    
    

    /*
     * Esta función es llamada justo antes de cambiar de nivel.
     * Guardamos datos de las medallas obtenidas como resultado, 
     * y preparamos los sprites de end game splash screen.
     */
    private void setLevelResults() {

        int check = 0;

        sceneMedalsBeforeAlter = sceneMedals;

        sprNum[0] = 1;
        // FIRST TIME RUN
        //Si o si, el jugador ha superado el nivel al llegar a aquí
        sceneMedals.finished = true;
        check++;

        //sceneMedals.timeBeated
        if (playerTime <= timeLevelLimit)
        {
            sprNum[1] = 1;
            sceneMedals.timeBeated = true;
            check++;
        }
        else
        {
            sprNum[1] = 0;
            sceneMedals.timeBeated = false;
        }


        //sceneMedals.batteryCollected
        if (batteryLevelCount == 0)
        {
            sprNum[2] = 1;
            sceneMedals.batteryCollected = true;
            check++;
        }
        else
        {
            sprNum[2] = 0;
            sceneMedals.batteryCollected = false;
        }
        
        //Si el jugador ha conseguido las tres a la vez, consigue las medallas de ORO
        if (check==3)
        {
            sceneMedals.allAtOnce = true;

            //Sprites para end game splash screen
            sprNum[0] = 2;
            sprNum[1] = 2;
            sprNum[2] = 2;
        }

        if (playerTime < sceneMedals.timeRecord)
        {
            //Obtiene nuevo record
            sceneMedals.timeRecord = playerTime;
        } //ELSE Mantiene el record anterior
    
    }
    
    //Esta función es llamada justo antes de cambiar de nivel.
    private void sendLevelResults() {

        //Revisamos los records anteriores a jugar el nivel, el jugador conservará siempre la mejor puntuación.
        
        //public bool timeBeated; // Superado en menos de X tiempo Si/No
        //public bool batteryCollected; // Recogidas pilas del nivel Si/No
        //public float timeRecord; // Marca personal de tiempo record.
        //public bool allAtOnce; // Marca si las medallas han sido ganadas todas a la vez

        if (sceneMedalsBeforeAlter.timeBeated && !sceneMedals.timeBeated)
        {
            sceneMedals.timeBeated = true;
        }

        if (sceneMedalsBeforeAlter.batteryCollected && !sceneMedals.batteryCollected)
        {
            sceneMedals.batteryCollected = true;
        }

        if (sceneMedalsBeforeAlter.timeRecord < sceneMedals.timeRecord)
        {
            sceneMedals.timeRecord = sceneMedalsBeforeAlter.timeRecord;
        }

        if (sceneMedalsBeforeAlter.allAtOnce && !sceneMedals.allAtOnce)
        {
            sceneMedals.allAtOnce = true;
        }

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
        //Varaibles de MenuIngame
        pauseMenuMI = GameObject.Find("CanvasMenuIngame");
        

        //Varaibles de end game splash screen
        doFlashAt = 650f;
        doDisplayMedal = 520f;
        sprNum = new int[3];
        sprNum[0] = -1;
        sprNum[1] = -1;
        sprNum[2] = -1;
        nextSprite = 0;

        //CÓDIGO PARA USAR MEDALLAS 2D
        starMedal = GameObject.Find("M-starMedal").GetComponent<Image>();
        timeMedal = GameObject.Find("M-timeMedal").GetComponent<Image>();
        batteryMedal = GameObject.Find("M-batteryMedal").GetComponent<Image>();
        
        //CÓDIGO PARA USAR MEDALLAS 3D
        //starMedal = GameObject.Find("M-starMedal").GetComponent<MeshRenderer>();
        //timeMedal = GameObject.Find("M-timeMedal").GetComponent<MeshRenderer>();
        //batteryMedal = GameObject.Find("M-batteryMedal").GetComponent<MeshRenderer>();

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
        levelNameEG = GameObject.Find("EG-Title").GetComponent<TextMeshProUGUI>();
        levelNameEG.text = "Level " + GameManager.Instance.idActualLevel.ToString();
        recordTimeEG = GameObject.Find("T-RecordTime").GetComponent<TextMeshProUGUI>();
        recordTimeEG.text = Utils.GetTimeFormat(Utils.RoundFloat(timeLevelLimit, 3), 3);
        playerTimeEG = GameObject.Find("T-Time").GetComponent<TextMeshProUGUI>();

        newRecordText = GameObject.Find("T-NewRecord").GetComponent<TextMeshProUGUI>();

        //Configuramos los botones de la splashscreen        
        goMenuButtonEG = GameObject.Find("B-goMenuButton").GetComponent<Button>();
        goMenuButtonEG.onClick.AddListener(goHomeEndGame);

        restartButtonEG = GameObject.Find("B-restartButton").GetComponent<Button>();
        restartButtonEG.onClick.AddListener(restartSceneEndGame);

        nextLevelButtonEG = GameObject.Find("B-nextLevelButton").GetComponent<Button>();
        nextLevelButtonEG.onClick.AddListener(loadNextSceneEndGame);

        //Configuramos los botones del Menu Ingame
        resumeButtonMI = GameObject.Find("MI-Resume").GetComponent<Button>();
        resumeButtonMI.onClick.AddListener(resumeGame);

        restartButtonMI = GameObject.Find("MI-Restart").GetComponent<Button>();
        restartButtonMI.onClick.AddListener(restartSceneEndGame);

        goMenuButtonMI = GameObject.Find("MI-Exit").GetComponent<Button>();
        goMenuButtonMI.onClick.AddListener(goHomeEndGame);

        //referencia del hud
        canvasHUD = GameObject.Find("CanvasHUD").GetComponent<HUDController>();

        //Nos aseguramos de que el idLevel se ha seteado correctamente
        //PROFE: Este valor lo usa HUDController.cs, al cambiar de nivel, se ejecuta antes ese script y por lo tanto no llega a actualizar a tiempo el idLevel ¿solucion?
        //GameManager.Instance.idActualLevel = idLevel;

        //controles de fin del mapa.
        //TODO: Revisar waltrapa, ¿porque entra dos veces en el trigger de NextLevel?
        waltrapa = true;

        //controles de cambio de escena, sólo reproduce en escenario2 ?!.
        //TODO: Revisar waltrapa, ¿porque entra dos veces en la función?
        waltrapa2 = true;

        //Cogemos referencia del end game splashscreen y lo ocultamos, junto con el menu de pausa
        canvasEndGame = GameObject.Find("CanvasEndGame");
        canvasEndGame.SetActive(false);
        pauseMenuMI.SetActive(false);
    }

    //TODO: Esto lo estmaos usando para debugar. Terminar de implementar para jugador o quitar.
    private void Update()
    {
        /*index
        !!!!!!!!!!!!!!!!
        DEV TESTING KEYS
        !!!!!!!!!!!!!!!!
        */
        if (Input.GetKey(KeyCode.L))
        {
            restartSceneEndGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.isGamePaused)
            {
                resumeGame();
            } else
            {
                pauseGame();
            }
        }

        if(Input.GetKey(KeyCode.O))
        {
            setLevelResults();
            sendLevelResults();
            loadNextScene(nextScene);
        }

        /*index
        !!!!!!!!!!!!!!!!!!
        END GAME ANIMATION
        !!!!!!!!!!!!!!!!!!
        */
        if (canvasHUD.levelEnded)
        {
            //Display de medallas
            timeDisplayMedals += Time.deltaTime * 1000;

            if(timeDisplayMedals > doDisplayMedal)
            {
                if(nextSprite == 0)
                {
                    //3D: starMedal.material = statusStar[sprNum[0]];

                    starMedal.sprite = statusStar[sprNum[0]];
                    SoundManager.Instance.PlaySound("EG-medal");
                    timeDisplayMedals = 0;
                }

                if (nextSprite == 1)
                {
                    //3D: timeMedal.material = statusTime[sprNum[1]];

                    timeMedal.sprite = statusStar[sprNum[1]];
                    SoundManager.Instance.PlaySound("EG-medal");
                    timeDisplayMedals = 0;
                }

                if(nextSprite == 2)
                {
                    //3D: batteryMedal.material = statusBattery[sprNum[2]];

                    batteryMedal.sprite = statusBattery[sprNum[2]];
                    SoundManager.Instance.PlaySound("EG-medal");
                    timeDisplayMedals = 0;
                }

                nextSprite++;
            }

            //Parpadeo de NEW RECORD ! en caso de superar tiempo record
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

            
            SoundManager.Instance.StopAllSounds();

            endGamePreparations();

            setLevelResults();
            sendLevelResults();          

            canvasHUD.levelEnded = true;

            canvasEndGame.SetActive(true);

        }
    }
}
