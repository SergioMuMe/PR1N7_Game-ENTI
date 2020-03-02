using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenuController : MonoBehaviour
{
    //TEST VARIABLES
    public bool[] levelsGenerated;

    //REAL VARIABLES
    public bool[] levelsLoaded;

    //public GameObject[] levelButtons;
    public Button[] levelButtons;
    private TextMeshProUGUI[] textUI;

    public TMP_ColorGradient[] statusColor = new TMP_ColorGradient[2];

    private void Start()
    {
        //TODO: Recolocar. Esto pilla el texto para cuando se activa el boton, modificar el texto.
        for (int i = 0; i < levelButtons.Length; i++)
        {
            textUI[i] = levelButtons[i].GetComponent<TextMeshProUGUI>();
        }

        //TODO: Modificar esto por load profile.bin de player.
        levelsGenerated = new bool[3] { true, true, false };

        //TODO: Mover a creación de profile.
        BinaryWriter writer = new BinaryWriter(File.Open("profile.bin", FileMode.Create));

        for (int i = 0; i < levelsGenerated.Length; i++)
        {
            writer.Write(levelsGenerated[i]);
        }

        writer.Close();

        //TODO: Mover a load de profile.

        BinaryReader reader;
        if(!File.Exists("profile.bin"))
        {
            Debug.LogError("ERROR loading file profile.bin");
            return;
        }

        reader = new BinaryReader(File.Open("profile.bin", FileMode.Open));
        
        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool value = reader.ReadBoolean();
            if (value)
            {
                levelButtons[i].interactable = true;
                textUI[i].colorGradientPreset = statusColor[0];
            }
            
            //levelsLoaded[i] = test;
            //Debug.Log(test);
        }

        reader.Close();
    }

    public void LoadLevel(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene("00_Tutorial/00_01");
    }

    public void LoadProfileInfo()
    {

    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
