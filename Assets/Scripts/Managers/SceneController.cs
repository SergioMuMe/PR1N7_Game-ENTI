using System.Collections;
using System.Collections.Generic;
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
    public float timeLevelLimit; // En Segundos. Tiempo objetivo del nivel para ganar la estrella del tiempo
    private float timePlayerRecord; // En Segundos. Tiempo record del jugador. 

    private GameManager scriptGM; //Obtenemos el GameManager para ir almacenando los datos de la sesion de juego

    //Obtenemos el tiempo record del jugador en el mapa. Si es la primera vez que lo juega lo seteamos al máximo 999s.
    private void getTimePlayerRecord()
    {
        bool firstTimeFLAG;
        firstTimeFLAG = scriptGM.getFirstTimeFLAG(idLevel);

        if (firstTimeFLAG)
        {
            timePlayerRecord = 999f;
        } else
        {
            timePlayerRecord = scriptGM.getTimeRecord(idLevel);
        }
    }

    //Info enviada al GameManager para guardar resultados del nivel
    private bool finished; // Nivel superado Si/No
    private bool timeBeated; // Superado en menos de X tiempo Si/No
    private float timeRecord; // En segundos. Marca personal de tiempo record
    private bool batteryCollected; // Recogidas pilas del nivel Si/No

    //Esta función es llamada justo antes de cambiar de nivel
    private void setLevelResults() {
        finished = true;
        
        if (batteryLevelCount == 0) { batteryCollected = true; }
        else { batteryCollected = false; }

        if (playerTime <= timeLevelLimit) { timeBeated = true; }
        else { timeBeated = false; }

        if(playerTime < timePlayerRecord)
        {
            //Obtiene nuevo record
            timeRecord = playerTime;
        } else
        {
            //Mantiene el record anterior
            timeRecord = timePlayerRecord;
        }
        Debug.LogWarning("playerTime: " + playerTime);
        Debug.LogWarning("timeRecord: " + timeRecord);
        Debug.LogWarning("timePlayerRecord: " + timePlayerRecord);
    }
    
    //Esta función es llamada justo antes de cambiar de nivel.
    private void sendLevelResults() {
        scriptGM.saveData(idLevel,finished,timeBeated,timeRecord,batteryCollected);
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
        getTimePlayerRecord();
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
            restartScene("_Scenes/Menus/MainMenu");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            loadNextScene(nextScene);
        }
    }
}
