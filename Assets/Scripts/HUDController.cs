using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    /*index
        #####################
        #                   #
        #  ENG GAME CANVAS  #
        #                   #
        #####################
    */

    private Button goMenuButton;

    /*index
        ################
        #              #
        #  HUD ESCENA  #
        #              #
        ################
    */

    // !!! REFERENCIA PLAYER para obtener el tiempo limite de clonación !!!
    private CharacterBehav characterBehav;

    // !!! CANVAS REC !!!
    private GameObject CanvasREC;
    private GameObject RECCircle;
    private Image RECBackGround;
    private TextMeshProUGUI recordingTime;

    private float time;

    // !!! CRONOMETRO TIEMPO !!!
    private GameObject CanvasLT;
    public float playerTime;
    private TextMeshProUGUI LTRecordTime;
    private TextMeshProUGUI LTTime;
    private float recordSelected;

    // Hacemos flash del numero para indicar que se aproxima al tiempo record
    public float secondsToFlash;
    public float timeFlashing;
    public float doFlashAt;

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
        RECBackGround = GameObject.Find("REC-BackGround").GetComponent<Image>();

        CanvasContadorClones = GameObject.Find("CanvasContadorClones");
        CCTexto = GameObject.Find("CC-Texto").GetComponent<TextMeshProUGUI>();

        CanvasLT = GameObject.Find("CanvasLT");
        LTTime = GameObject.Find("LT-Time").GetComponent<TextMeshProUGUI>();
        LTRecordTime = GameObject.Find("LT-RecordTime").GetComponent<TextMeshProUGUI>();

        goMenuButton = GameObject.Find("EG-goMenuButton").GetComponent<Button>();

        //Setup record time of level
        int idPlayer = GameManager.Instance.profileSelected;
        int idLevel = GameManager.Instance.idActualLevel;
        float levelRecordDEV = GameManager.Instance.timeLevelLimit[idLevel];
        float levelRecordPlayer = GameManager.Instance.profiles[idPlayer].levelsData[idLevel].levelMedals.timeRecord;
        
        //El record es estático al cargar escena, obtenemos la info en el START...
        LTRecordTime.text = "Record: ";

        //...verificamos si el record más bajo es el del player o el del DEV.
        if (levelRecordPlayer < levelRecordDEV)
        {
            LTRecordTime.text += Utils.GetTimeFormat(levelRecordPlayer,1).ToString();
            recordSelected = levelRecordPlayer;
        } else
        {
            LTRecordTime.text += Utils.GetTimeFormat(levelRecordDEV, 1).ToString();
            recordSelected = levelRecordDEV;
        }
    }

    private void Update()
    {
        /*index
        !!!!!!!!!!
        ENDGAME CANVAS 
        !!!!!!!!!!
        */
        /*TODO:
         * Printar record time
         * 
         * Printar tiempo actual
         * -- Superado, en amarillo con animación parpadeando, añadir palabra NEW RECORD
         * -- No superado, en rojo parpadeando
         * 
         * Añadir funcionalidad a los 3 botones de abajo.
         * 
         * Añadir animación básica:
         * 1- Poco a poco se marcan las estrellas, se reproduce un sonido al cambiar.
         * 2- Aparece el texto de los tiempos.
         */


        /*index
        !!!!!!!!!!
        CRONOMETRO 
        !!!!!!!!!!
        */
        //deltaTime
        playerTime += Time.deltaTime;

        //Cronometro level
        LTTime.text = Utils.GetTimeFormat(Utils.RoundFloat(playerTime, 3), 1);

        //Gradient de color para marcar tiempo restante
        gradientProgression = Mathf.Lerp(0f, 1f, gradientTime);
        gradientTime += Time.deltaTime / recordSelected;
        colorPlayerTime = gradientPlayerTime.Evaluate(gradientProgression);
        LTTime.color = colorPlayerTime;

        //Parpadeo de tiempo para indicar que se aproxima al record
        //Evitamos flash si el tiempo record es más bajo que el tiempo de flasheo.
        if(recordSelected>secondsToFlash)
        {
            if (recordSelected < playerTime + secondsToFlash && playerTime<recordSelected)
            {
                timeFlashing += Time.deltaTime * 1000;

                if(timeFlashing > doFlashAt)
                {
                    LTTime.enabled = true;
                    if (timeFlashing > doFlashAt * 2)
                    {
                        timeFlashing = 0;
                    }
                }
                else
                {
                    LTTime.enabled = false;
                }
            }

            if(!LTTime.enabled && playerTime > recordSelected)
            {
                LTTime.enabled = true;
            }
        }

        /*index
        !!!!!!!!!!!!!!!!!!!!!!!!!
        CANVAS CONTADOR DE CLONES 
        !!!!!!!!!!!!!!!!!!!!!!!!!
        */

        if(characterBehav.maxClones == 0)
        {
            CanvasContadorClones.SetActive(false);
        } else
        {
            CCTexto.text = characterBehav.clones.Count +" / " + characterBehav.maxClones.ToString();
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
            if (Mathf.Round(time) % 2 == 0) {
                RECCircle.SetActive(true);
            }
            else {
                RECCircle.SetActive(false);
            }

            //REC-CountDown, displayamos tiempo restante de grabación.
            recordingTime.text = Utils.GetTimeFormat(characterBehav.limitRecordingTime - time,2);

        } else
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
}
