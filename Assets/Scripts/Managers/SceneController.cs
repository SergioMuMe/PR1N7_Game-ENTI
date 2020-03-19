using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    
    //PROFE: ¿Porque entra dos veces en el trigger?
    private bool waltrapa;

    private void restartScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void loadNextScene(string scene)
    {
        setLevelResults();
        sendLevelResults();
        SceneManager.LoadScene(scene);
    }
   

    /*index
        ##########################
        #                        #
        #  CONFIGURACIÓN ESCENA  #
        #                        #
        ##########################
    */
    public int idLevel; //Para informar al GameManager de que nivel és.
    public int batteryLevelCount; // Cantidad de pilas en el nivel
    private float playerTime; // En Segundos. Tiempo que tarda el jugador en superar el nivel
    private float timeLevelLimit; // En Segundos. Tiempo objetivo del nivel para ganar la estrella del tiempo
    private float timePlayerRecord; // En Segundos. Tiempo record del jugador. 

    private GameManager scriptGM; //Obtenemos el GameManager para ir almacenando los datos de la sesion de juego

    //Info enviada al GameManager para guardar resultados del nivel. Previamente obtenemos la info actual.
    Medals sceneMedals;
   

    //Obtenemos el tiempo record del jugador en el mapa. Si es la primera vez que lo juega lo seteamos al máximo 999s.
    private void getPlayerLevelInfo()
    {
        bool firstTimeFLAG;
        firstTimeFLAG = scriptGM.getFirstTimeFLAG(idLevel);

        int profileSelected;
        profileSelected = scriptGM.profileSelected;


        sceneMedals = getPlayerLevelMedalsInfo();

        /*if (firstTimeFLAG)
        {
            sceneMedals.timeRecord = 999f;
            //timePlayerRecord = 999f;
        } else
        {
            sceneMedals.timeRecord = scriptGM.getTimeRecord(idLevel);
            //timePlayerRecord = scriptGM.getTimeRecord(idLevel);
        }*/

    }
    private Medals getPlayerLevelMedalsInfo()
    {
        return scriptGM.getLevelMedals(idLevel);
    }

    
    

    //Esta función es llamada justo antes de cambiar de nivel
    private void setLevelResults() {
        
        sceneMedals.finished = true;
        playerTime = GameObject.Find("CanvasHUD").GetComponent<HUDController>().playerTime;

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
        actualScene = SceneManager.GetActiveScene().name;
        getPlayerLevelInfo();
        timeLevelLimit = scriptGM.timeLevelLimit[idLevel];

        GameManager.Instance.idActualLevel = idLevel;

        waltrapa = true;
    }

    private void Update()
    {
        //Cronometro, cuanto tarda el jugador en superar el nivel
        playerTime += Time.deltaTime;

        

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (waltrapa && collision.tag == "Player")
        {
            waltrapa = false;
            Debug.LogWarning("TODO arreglar waltrapa");
            collision.enabled = false;
            loadNextScene(nextScene);
        }
    }
}
