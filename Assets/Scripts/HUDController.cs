using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public bool levelEnded;

    /*index
        ################
        #              #
        #  HUD ESCENA  #
        #              #
        ################
    */

    public TextMeshProUGUI PauseTitle;

    // !!! REFERENCIA PLAYER para obtener el tiempo limite de clonación !!!
    private CharacterBehav characterBehav;

    // !!! CANVAS REC !!!
    private GameObject CanvasREC;
    private GameObject RECCircle;
    //private Image RECBackGround;
    private TextMeshProUGUI recordingTime;

    private float time;

    // !!! CRONOMETRO TIEMPO !!!
    private GameObject CanvasLT;
    public float playerTime;
    private TextMeshProUGUI LTRecordTime;
    public TextMeshProUGUI LTTime;
    public float recordSelected;

    // Modificamos color según se aproxima al tiempo record
    public Gradient gradientPlayerTime;
    private float gradientTime;
    private float gradientProgression;
    private Color colorPlayerTime;

    // !!! CONTADOR DE CLONES !!!
    private GameObject CanvasContadorClones;
    private TextMeshProUGUI CCTexto;


    /*index
        ###########
        #         #
        #  OTROS  #
        #         #
        ###########
    */


    /*index
        #####################
        #                   #
        #  FUNCIONES UNITY  #
        #                   #
        #####################
    */

    private void Start()
    {                       
        //Obtenemos referencias de los GameObjects
        characterBehav = GameObject.Find("Player").GetComponent<CharacterBehav>();
         
        CanvasREC = GameObject.Find("CanvasREC");
        recordingTime = GameObject.Find("REC-CountDown").GetComponent<TextMeshProUGUI>();
        RECCircle = GameObject.Find("REC-Circle");
        //RECBackGround = GameObject.Find("REC-BackGround").GetComponent<Image>();

        CanvasContadorClones = GameObject.Find("CanvasContadorClones");
        CCTexto = GameObject.Find("CC-Texto").GetComponent<TextMeshProUGUI>();

        CanvasLT = GameObject.Find("CanvasLT");
        LTTime = GameObject.Find("LT-Time").GetComponent<TextMeshProUGUI>();
        LTRecordTime = GameObject.Find("LT-RecordTime").GetComponent<TextMeshProUGUI>();

        

        //goMenuButton = GameObject.Find("EG-goMenuButton").GetComponent<Button>();

        //Setup record time of level
        int idPlayer = GameManager.Instance.profileSelected;
        int idLevel = GameManager.Instance.idActualLevel;
        float levelRecordDEV = GameManager.Instance.timeLevelLimit[idLevel];
        float levelRecordPlayer = GameManager.Instance.profiles[idPlayer].levelsData[idLevel].levelMedals.timeRecord;
        bool allAtOnce= GameManager.Instance.profiles[idPlayer].levelsData[idLevel].levelMedals.allAtOnce;

        //Pause displaya el nivel actual
        PauseTitle.text = "LEVEL " + idLevel;

        //El record es estático al cargar escena, obtenemos la info en el START...
        LTRecordTime.text = "Record: ";

        //...verificamos si el record más bajo es el del player o el del DEV.
        if (allAtOnce)
        {
            //Si ya tienes la medalla de ORO, compites contra tu tiempo.
            recordSelected = levelRecordPlayer;
        }
        else
        {
            //Si aun no tinees la medalla de ORO, compites contra el record del mapa
            recordSelected = levelRecordDEV;
        }

        LTRecordTime.text += Utils.GetTimeFormat(recordSelected, 3).ToString();

        // Decisión de la ALPHA, desactivamos contador de tiempo.
        //CanvasLT.SetActive(false);

        levelEnded = false;
        
    }

    private void Update()
    {
        if (!levelEnded)
        {

            /*index
            !!!!!!!!!!
            CRONOMETRO 
            !!!!!!!!!!
            */
            //deltaTime
            playerTime += Time.deltaTime;

            //Cronometro level
            LTTime.text = Utils.GetTimeFormat(Utils.RoundFloat(playerTime, 3), 3);

            //Gradient de color para marcar tiempo restante
            gradientProgression = Mathf.Lerp(0f, 1f, gradientTime);
            gradientTime += Time.deltaTime / recordSelected;
            colorPlayerTime = gradientPlayerTime.Evaluate(gradientProgression);
            LTTime.color = colorPlayerTime;

            /*index
            !!!!!!!!!!!!!!!!!!!!!!!!!
            CANVAS CONTADOR DE CLONES 
            !!!!!!!!!!!!!!!!!!!!!!!!!
            */

            if (characterBehav.maxClones == 0)
            {
                CanvasContadorClones.SetActive(false);
            }
            else
            {
                CCTexto.text = characterBehav.clones.Count + "/" + characterBehav.maxClones.ToString();
            }

            if (characterBehav.clones.Count == characterBehav.maxClones)
            {
                CCTexto.color = Color.red;
            }
            else
            {
                CCTexto.color = Color.white;
            }

            /*index
            !!!!!!!!!!
            CANVAS REC 
            !!!!!!!!!!
            */

            //Activamos Canvas si player isRecording
            if (characterBehav.isRecording)
            {
                time += Time.deltaTime;
                if (characterBehav.maxClones != 0)
                {
                    CanvasContadorClones.SetActive(false);
                }
                CanvasLT.SetActive(false);
                CanvasREC.SetActive(true);


                //Parpadeo del REC-Circle
                if (Mathf.Round(time) % 2 == 0)
                {
                    RECCircle.SetActive(true);
                }
                else
                {
                    RECCircle.SetActive(false);
                }

                //REC-CountDown, displayamos tiempo restante de grabación.
                recordingTime.text = Utils.GetTimeFormat(characterBehav.limitRecordingTime - time, 3);

            }
            else
            {
                // !isRecording > Desactivamos CanvasREC
                time = 0;
                CanvasREC.SetActive(false);
                if (characterBehav.maxClones != 0)
                {
                    CanvasContadorClones.SetActive(true);
                }
                CanvasLT.SetActive(true);
            } 
        }
        else
        {
            //Partida finalizada, desactivamos HUD de juego:
            CanvasREC.SetActive(false);
            if (characterBehav.maxClones != 0)
            {
                CanvasContadorClones.SetActive(false);
            }
            CanvasLT.SetActive(false);
        }
    }
}
