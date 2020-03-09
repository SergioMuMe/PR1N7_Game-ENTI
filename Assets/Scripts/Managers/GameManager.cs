using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*index
        ################################
        #                              #
        #  DATOS DENTRO DE UN PROFILE  #
        #                              #
        ################################
    */

    // Un nivel y su estado.
    public struct LevelData
    {
        public bool levelUnblockedFLAG; //FLAG usado en MainMenuController para displayar los niveles disponibles.
        public bool firstTimeFLAG; //FLAG usado para setear el timeRecord por primera vez desde SceneController.

        public Medals levelMedals;
    }

    // Listado de profiles
    public struct Profiles
    {
        public bool profileUsed; // Indica si está en uso
        public string profileName; // Nombre profile
        public LevelData[] levelsData; // Listado de niveles y su estado
    }

    public Profiles[] profiles = new Profiles[3];

    // Paths de los profiles
    private string[] path = { "bin/profile00.bin", "bin/profile01.bin", "bin/profile02.bin" };

    /*index
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


    // TODO: SetProfileSelected. Lo más seguro que desde UNITY inspector.

    /*index
        #############################
        #                           #
        #  DATOS DEL NIVEL/USUARIO  #
        #                           #
        #############################
    */

    public float[] timeLevelLimit;

    private void setTimeLevelLimit()
    {
        //PROFE:¿porque no puedo hacer el new en la misma linea donde declaro timeLevelLimit?
        timeLevelLimit = new float[7];

        timeLevelLimit[0] = 99f;
        timeLevelLimit[1] = 10f;
        timeLevelLimit[2] = 15f;
        timeLevelLimit[3] = 20f;
        timeLevelLimit[4] = 25f;
        timeLevelLimit[6] = 30f;
    }


    public bool getFirstTimeFLAG(int idLevel)
    {
        return profiles[profileSelected].levelsData[idLevel].firstTimeFLAG;
    }

    public Medals getLevelMedals(int idLevel)
    {
        return profiles[profileSelected].levelsData[idLevel].levelMedals;
    }

    public bool getBatterySelected(int idLevel)
    {
        return profiles[profileSelected].levelsData[idLevel].levelMedals.batteryCollected;
    }

    public float getTimeRecord(int idLevel)
    {
        return profiles[profileSelected].levelsData[idLevel].levelMedals.timeRecord;
    }

    /*index
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

        for (int i = 0; i < profiles.Length; i++)
        {
            profiles[i].profileUsed = false;
            profiles[i].profileName = "New profile";
            //ALERTA: Siempre crear 1 level más de los jugables. El [0] es de testing purposes{GameDevRoom}. El resto de lvl corresponden su ID con el de la BUILD. ¡Todo lo que no sean niveles de juego deberán ir al final de la build order!
            profiles[i].levelsData = new LevelData[6];
            for (int j = 0; j < profiles[i].levelsData.Length; j++)
            {
                
                //El primer nivel y la DEV-Room está desbloqueado para jugar
                if (j == 0 || j == 1) { profiles[i].levelsData[j].levelUnblockedFLAG = true; }
                else { profiles[i].levelsData[j].levelUnblockedFLAG = false; }

                profiles[i].levelsData[j].firstTimeFLAG = true;
                profiles[i].levelsData[j].levelMedals.finished = false;
                profiles[i].levelsData[j].levelMedals.batteryCollected = false;
                profiles[i].levelsData[j].levelMedals.timeBeated = false;
                profiles[i].levelsData[j].levelMedals.timeRecord = 999f;
            }
        }

        //TESTING ZONE BEGIN
        
        //TESTING ZONE END

        for (int i = 0; i < path.Length; i++)
        {

            BinaryWriter writer = new BinaryWriter(File.Open(path[i], FileMode.Create));

            writer.Write(profiles[i].profileUsed);
            writer.Write(profiles[i].profileName);

            for (int j = 0; j < profiles[i].levelsData.Length; j++)
            {
                writer.Write(profiles[i].levelsData[j].levelUnblockedFLAG);
                writer.Write(profiles[i].levelsData[j].firstTimeFLAG);
                writer.Write(profiles[i].levelsData[j].levelMedals.finished);
                writer.Write(profiles[i].levelsData[j].levelMedals.batteryCollected);
                writer.Write(profiles[i].levelsData[j].levelMedals.timeBeated);
                writer.Write(profiles[i].levelsData[j].levelMedals.timeRecord);
            }

            writer.Close();
        }      
    }

    /*index
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
                // SOLUCION: Hacerlo variable a variable por tipo de dato, como en c++
            }
            else {
                Debug.Log("Fail to open " + path[i] + " file.");
            }
        }
    }

    //Tras superar un nivel. Guarda en fichero el progreso del profile.
    public void saveData(int idLevel, Medals recivedMedals)
    {
        //SAVE CURRENT LEVEL DATA
        LevelData newLevelData;
        
        newLevelData.levelUnblockedFLAG = true;
        newLevelData.firstTimeFLAG = false;
        newLevelData.levelMedals.finished = recivedMedals.finished;
        newLevelData.levelMedals.timeBeated = recivedMedals.timeBeated;
        newLevelData.levelMedals.timeRecord = recivedMedals.timeRecord;
        newLevelData.levelMedals.batteryCollected = recivedMedals.batteryCollected;

        profiles[profileSelected].levelsData[idLevel] = newLevelData;

        //SAVE NEXT LEVEL DATA
        //TODO: Controlar cuando juegue el último nivel del juego. idLevel+1 !!!
        LevelData unblockNextLevel;

        unblockNextLevel.levelUnblockedFLAG = false;

        profiles[profileSelected].levelsData[idLevel + 1].levelUnblockedFLAG = true;
    }

    /*index
        ########################
        #                      #
        #  FUNCIONES DE UNITY  #
        #                      #
        ########################
    */

    // Instanciar GameManager
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
        
    }
    
    private void Start()
    {
        createEmptyProfiles();

        //Inicializamos tiempos a batir en cada nivel
        
        setTimeLevelLimit();
    }

    void Update()
    {
        
    }
}
