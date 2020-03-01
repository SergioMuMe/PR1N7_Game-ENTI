using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
        ################################
        #                              #
        #  DATOS DENTRO DE UN PROFILE  #
        #                              #
        ################################
    */

    // Un nivel y su estado.
    public struct LevelData
    {
        public bool finished; // Nivel superado Si/No
        public bool timeBeated; // Superado en menos de X tiempo Si/No
        public float timeRecord; // Marca personal de tiempo record
        public bool batteryCollected; // Recogidas pilas del nivel Si/No
    }

    // Listado de profiles
    public struct Profiles
    {
        public bool profileUsed; // Indica si está en uso
        public string profileName; // Nombre profile
        public List<LevelData> levelsData; // Listado de niveles y su estado
    }
    private List<Profiles> profiles;

    // Paths de los profiles
    private string[] path = { "bin/profile01.bin", "bin/profile02.bin", "bin/profile03.bin" };

    /*
        ########################
        #                      #
        #  DATOS DE LA SESIÓN  #
        #                      #
        ########################
    */

    /*
     * Cuando el jugador escoge profile. Nos guardamos el id. 
     * Durante la sesión de juego, vamos actualizando los datos de profiles[profileSelected].levelsData[i] con los datos que pertoquen.
     * Para guardar de nuevo en fichero: path[profileSelected] y guardamos profiles[profileSelected] en el fichero.
     */
    public int profileSelected;
    public int test;

    // TODO: SetProfileSelected. Lo más seguro que desde UNITY inspector.

    /*
        #####################
        #                   #
        #  DATOS DEL NIVEL  #
        #                   #
        #####################
    */
      
    
    /* 
        ######################################
        #                                    #
        #  FUNCIONES DURANTE LA INSTALACIÓN  #
        #                                    #
        ######################################
    */
    // PROFE: Como crear carpetas o ficheros por defecto para las builds.

    //Por defecto todos los profiles están creados pero vacios, el bool profileUsed determina si el usuario puede usarlo o no para crear su profile.    
    private void createEmptyProfiles()
    {
        bool prfUsed = false;
        string prfName = "New profile";

        for (int i = 0; i < path.Length; i++)
        {
            BinaryWriter writer = new BinaryWriter(File.Open(path[i], FileMode.Create));
            writer.Write(prfUsed);
            writer.Write(prfName);
            writer.Close();     
        }       
    }

    /*
        ##########################
        #                        #
        #  GUARDAR|CARGAR DATOS  #
        #                        #
        ##########################
    */
    //Carga los ficheros de profiles al iniciar el juego.
    private void loadProfiles()
    {
        BinaryReader reader;
        for (int i = 0; i < path.Length; i++)
        {
            if (File.Exists(path[i])) {
                reader = new BinaryReader(File.Open(path[i], FileMode.Open));

                // PROFE Read struct y guardar en List<Profiles> profiles. ¿Marshalling?
               
            } else {
                Debug.Log("Fail to open " + path[i] + " file.");
            }
        }

    }

    //PROFE: ¿Como cambiar esto para que ambos scripts puedan enviarse la info con structs?
    //Tras superar un nivel. Guarda en fichero el progreso del profile.
    public void saveData(int idLevel, bool finished, bool timeBeated, float timeRecord, bool batteryCollected)
    {
        LevelData newLevelData;

        newLevelData.finished = finished;
        newLevelData.timeBeated = timeBeated;
        newLevelData.timeRecord = timeRecord;
        newLevelData.batteryCollected = batteryCollected;

        //profiles[profileSelected].levelsData[idLevel] = newLevelData;
        Debug.Log(
            "Finished: " + newLevelData.finished +
            "\ntimeBeated: " + newLevelData.timeBeated +
            "\ntimeRecord: " + newLevelData.timeRecord +
            "\nbatteryCollected: " + newLevelData.batteryCollected
            );
    }

    /*
        ########################
        #                      #
        #  FUNCIONES DE UNITY  #
        #                      #
        ########################
    */
    
    // Instanciar GameManager
    public static GameManager Instance { get; private set; }

    int iterat = 0;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
        
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
