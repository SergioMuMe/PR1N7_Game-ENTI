using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    /*index 
        #############
        #           #
        #  GENERAL  #
        #           #
        #############
    */

    private GameObject profileSelection;
    private GameObject createProfile; 

    private EventTrigger[] vectorButtons;

    /*index 
         ##############
         #            #
         #  PROFILES  #
         #            #
         ##############
     */
    #region PROFILES

    private int idProfileSelected;
    private GameObject nameAlert;
    private TMP_InputField nameTextBox;

    private EventTrigger backButtonCP;
    private EventTrigger createuttonCP;

    //Obtenemos nombres de perfiles
    private TextMeshProUGUI[] profileName;
    private void setProfileNames()
    {
        for (int i = 0; i < 3; i++)
        {
            profileName[i].text = GameManager.Instance.profiles[i].profileName;
        }

    }

    //Obtenemos total mapas superados
    private TextMeshProUGUI[] totalMaps;
    private void setTotalMaps()
    {
        for (int i = 0; i < 3; i++)
        {
            totalMaps[i].text = GameManager.Instance.getTotalMaps(i).ToString();
            totalMaps[i].text = totalMaps[i].text + " / " + (GameManager.Instance.numberOfLevels - 1).ToString();
        }
    }
    //Obtenemos total medallas conseguidas
    private TextMeshProUGUI[] totalMedals;
    private void setTotalMedals()
    {
        for (int i = 0; i < 3; i++)
        {
            totalMedals[i].text = GameManager.Instance.getTotalMedals(i).ToString();
            totalMedals[i].text = totalMedals[i].text + " / " + ((GameManager.Instance.numberOfLevels - 1) * 3).ToString();
        }
    }

    //Cargamos en pantalla la informacion de los profiles locales
    public void loadProfileSelectionData()
    {
        setProfileNames();
        setTotalMedals();
        setTotalMaps();
    }


    //Seteamos profile seleccionado, en caso de no existir creamos profile nuevo
    public void resetGMProfileVariables()
    {
        GameManager.Instance.profileSelected = 999;
        GameManager.Instance.profilePicked = false;
    }

    //Seteamos profile seleccionado, en caso de no existir creamos profile nuevo
    public void setProfileSelected(int idSelected)
    {       
        GameManager.Instance.profileSelected = idSelected;
        GameManager.Instance.profilePicked = true;
        
        getProfileSelected();
        
        if (GameManager.Instance.profiles[idSelected].profileUsed)
        {
            profileSelection.SetActive(false);
            createProfile.SetActive(false);
            //TODO: LoadMain Menu, pass: getLevelStatus
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            profileSelection.SetActive(false);
            createProfile.SetActive(true);
            nameAlert.SetActive(false);
        }
    }

    //Parche para código antiguo. Usamos alguna variable local que obtenemos del gameManager.
    public void getProfileSelected()
    {
        idProfileSelected = GameManager.Instance.profileSelected;

    }

    //Creación de nuevo profile binario local
    public void createNewProfileBIN()
    {
        int idProfile = GameManager.Instance.profileSelected;

        string name = nameTextBox.text;

        if (name.Length <= 1 || name.Length >= 16)
        {
            nameAlert.SetActive(true);
            nameTextBox.text = "";
            return;
        }

        //Definimos datos del nuevo perfil
        GameManager.Instance.profiles[idProfile].profileUsed = true;
        GameManager.Instance.profiles[idProfile].profileName = name;

        //Actualizamos perfil
        GameManager.Instance.saveDataInProfileBIN();

        //Cargamos datos del jugador
        GameManager.Instance.loadProfiles(idProfile);

        //Avanzamos al mainmenu
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayImportantButton()
    {
        SoundManager.Instance.PlaySound("MENU-ImportantButton");
    }

    public void PlayProfileSelected()
    {
        SoundManager.Instance.PlaySound("MENU-ProfileSelected");
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Obtenemos referencias...
        profileSelection = GameObject.Find("ProfileSelection");
        createProfile = GameObject.Find("CreateProfile");
        nameAlert = GameObject.Find("NameTextAlert");
        nameTextBox = GameObject.Find("NameValue").GetComponent<TMP_InputField>();

        //Referencias de los profiles
        idProfileSelected = GameManager.Instance.profileSelected;

        profileName = new TextMeshProUGUI[3];
        profileName[0] = GameObject.Find("NombreProfile0").GetComponent<TextMeshProUGUI>();
        profileName[1] = GameObject.Find("NombreProfile1").GetComponent<TextMeshProUGUI>();
        profileName[2] = GameObject.Find("NombreProfile2").GetComponent<TextMeshProUGUI>();

        totalMedals = new TextMeshProUGUI[3];
        totalMedals[0] = GameObject.Find("MedalsProfile0").GetComponent<TextMeshProUGUI>();
        totalMedals[1] = GameObject.Find("MedalsProfile1").GetComponent<TextMeshProUGUI>();
        totalMedals[2] = GameObject.Find("MedalsProfile2").GetComponent<TextMeshProUGUI>();

        totalMaps = new TextMeshProUGUI[3];
        totalMaps[0] = GameObject.Find("LevelsProfile0").GetComponent<TextMeshProUGUI>();
        totalMaps[1] = GameObject.Find("LevelsProfile1").GetComponent<TextMeshProUGUI>();
        totalMaps[2] = GameObject.Find("LevelsProfile2").GetComponent<TextMeshProUGUI>();

        //Cargamos datos a displayar de los profiles
        loadProfileSelectionData();

        //Ocultamos menus, mostramos profileSelection.SetActive(true);
        createProfile.SetActive(false);
        nameAlert.SetActive(false);

        //WELCOME TO PR1N7
        SoundManager.Instance.PlaySound("EG-Fanfarria");
    }
}
