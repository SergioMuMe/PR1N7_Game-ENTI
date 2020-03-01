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

    //PROFE: Porque is never asigned to...
    // Listado de profiles
    public struct Profiles
    {
        public bool profileUsed; // Indica si está en uso
        public string profileName; // Nombre profile
        public LevelData[] levelsData; // Listado de niveles y su estado
    }

    private Profiles[] profiles = new Profiles[3];

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
    // PROFE: Repasar escribir/leer ficheros binarios structs. ¿Marshalling? ¿BinarySerialization?

    //Por defecto todos los profiles están creados pero vacios, el bool profileUsed determina si el usuario puede usarlo o no para crear su profile.    
    private void createEmptyProfiles()
    {

        for (int i = 0; i < profiles.Length; i++)
        {
            profiles[i].profileUsed = false;
            profiles[i].profileName = "New profile";
            profiles[i].levelsData = new LevelData[2];
            for (int j = 0; j < profiles[i].levelsData.Length; j++)
            {
                profiles[i].levelsData[j].finished = false;
                profiles[i].levelsData[j].batteryCollected = false;
                profiles[i].levelsData[j].timeBeated = false;
                profiles[i].levelsData[j].timeRecord = 999f;
            }
        }

        for (int i = 0; i < path.Length; i++)
        {

            BinaryWriter writer = new BinaryWriter(File.Open(path[i], FileMode.Create));

            writer.Write(profiles[i].profileUsed);
            writer.Write(profiles[i].profileName);

            for (int j = 0; j < profiles[i].levelsData.Length; j++)
            {
                writer.Write(profiles[i].levelsData[j].finished);
                writer.Write(profiles[i].levelsData[j].batteryCollected);
                writer.Write(profiles[i].levelsData[j].timeBeated);
                writer.Write(profiles[i].levelsData[j].timeRecord);
            }

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

                // TODO Read struct y guardar en List<Profiles> profiles. 
            }
            else {
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

        Debug.Log(
            "Finished: " + newLevelData.finished +
            "\ntimeBeated: " + newLevelData.timeBeated +
            "\ntimeRecord: " + newLevelData.timeRecord +
            "\nbatteryCollected: " + newLevelData.batteryCollected
            );

        profiles[profileSelected].levelsData[idLevel] = newLevelData;
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
