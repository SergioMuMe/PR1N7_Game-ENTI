using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /*
        ############################
        #                          #
        #  CONTROL CAMBIO ESCENAS  #
        #                          #
        ############################
    */
    public string nextScene;
    public string actualScene;

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

    /*
        ##########################
        #                        #
        #  CONFIGURACIÓN ESCENA  #
        #                        #
        ##########################
    */
    public int idLevel; //Para informar al GameManager de que nivel és.
    public int batteryLevelCount; // Cantidad de pilas en el nivel
    public float playerTime; // En Segundos. Tiempo que tarda el jugador en superar el nivel
    public float timeLevelLimit; // En Segundos. Tiempo objetivo del nivel para ganar la estrella del tiempo

    private GameManager scriptGM; //Obtenemos el GameManager para ir almacenando los datos de la sesion de juego

    //TODO: Esto pasarlo a struct para comunicar entre scripts.
    private bool finished; // Nivel superado Si/No
    private bool timeBeated; // Superado en menos de X tiempo Si/No
    private float timeRecord; // En segundos. Marca personal de tiempo record
    private bool batteryCollected; // Recogidas pilas del nivel Si/No

    //Esta función es llamada justo antes de cambiar de nivel
    private void setLevelResults() {
        finished = true;
        playerTime = Time.timeSinceLevelLoad;

        if (batteryLevelCount == 0) { batteryCollected = true; }
        else { batteryCollected = false; }

        if (playerTime <= timeLevelLimit) { timeBeated = true; }
        else { timeBeated = false; }

        timeRecord = playerTime;
    }
    
    //Esta función es llamada justo antes de cambiar de nivel.
    private void sendLevelResults() {
        scriptGM.saveData(idLevel,finished,timeBeated,timeRecord,batteryCollected);
    }


    /*
        ########################
        #                      #
        #  FUNCIONES DE UNITY  #
        #                      #
        ########################
    */

    private void Start()
    {
        scriptGM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            restartScene(actualScene);
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
