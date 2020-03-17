using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    /*index
        ################
        #              #
        #  HUD ESCENA  #
        #              #
        ################
    */

    //REFERENCIA PLAYER para obtener el tiempo limite de clonación
    CharacterBehav characterBehav;

    // CANVAS REC
    GameObject CanvasREC;
    GameObject RECCircle;
    Image RECBackGround;
    TextMeshProUGUI recordingTime;

    //CRONOMETRO
    public float playerTime;
    private TextMeshProUGUI LTRecordTime;
    private TextMeshProUGUI LTTime;

    /*index
        ###########
        #         #
        #  OTROS  #
        #         #
        ###########
    */

    //Delta Time
    float time;


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
        
        LTTime = GameObject.Find("LT-Time").GetComponent<TextMeshProUGUI>();
        LTRecordTime = GameObject.Find("LT-RecordTime").GetComponent<TextMeshProUGUI>();

        //LTRecord.text = GameManager.Instance.timeLevelLimit[]
        //LTRecord;
    }

    private void Update()
    {
        playerTime += Time.deltaTime;

        //Cronometro level
        LTTime.text = "TIME: " + Utils.GetTimeFormat(Utils.RoundFloat(playerTime, 3), 1);

        //Activamos Canvas si player isRecording
        if (characterBehav.isRecording)
        {
            time += Time.deltaTime;
            CanvasREC.SetActive(true);

            //Parpadeo del REC-Circle
            if (Mathf.Round(time) % 2 == 0) {
                RECCircle.SetActive(true);
            }
            else {
                RECCircle.SetActive(false);
            }

            //REC-CountDown, displayamos tiempo restante de grabación.
            recordingTime.text = Utils.GetTimeFormat(characterBehav.limitRecordingTime - time,1);

        } else
        {
            // !isRecording > Reseteamos time y desactivamos CanvasREC
            time = 0;
            CanvasREC.SetActive(false);
        }
    }
}
