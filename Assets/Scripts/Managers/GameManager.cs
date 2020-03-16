﻿using System;
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
        /*TODO hacer el idGameVersion, sirve para realizar metricas*/
        //public int idGameVersion;
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

    
    // BAD NAMING :(
    //TODO: Lo suyo seria profileSelected> idProfileSelected. Y este bool mantener a profileSelected
    
    /* Nos permite saber si un profile ha sido seleccionado, 
     * de esta manera cuando cargamos la escena de menu principal 
     * al salir desde dentro de un nivel, no forzamos al jugador a realizar 
     * seleccion de profile (again) 
     */
    public bool profilePicked;

    /*index
        #############################
        #                           #
        #  DATOS DEL NIVEL/USUARIO  #
        #                           #
        #############################
    */

    //Array con los tiempos record a batir para cada nivel.
    public float[] timeLevelLimit;

    //Trasladamos timeLevelLimit.Length a nomberOfLevels para legibilidad en otras funciones.
    public int numberOfLevels;

    private void setNumberOfLevels()
    {
        numberOfLevels = timeLevelLimit.Length;
    }

    public int getTotalMedals(int id)
    {
        int num = 0;

        for (int j = 0; j < numberOfLevels; j++)
        {
            if (profiles[id].levelsData[j].levelMedals.finished)
            {
                num += 1;
            }
            if (profiles[id].levelsData[j].levelMedals.batteryCollected)
            {
                num += 1;
            }
            if (profiles[id].levelsData[j].levelMedals.timeBeated)
            {
                num += 1;
            }
        }
        return num;
    }

    public int getTotalMaps(int id)
    {
        int num = 0;

        for (int j = 0; j < numberOfLevels; j++)
        {
            if(profiles[id].levelsData[j].levelMedals.finished)
            {
                num += 1;
            }
        }
        return num;
    }

    //Funcion interna de los DEV, seteamos el tiempo record base de cada nivel
    private void setTimeLevelLimit()
    {
        //PROFE:¿porque no puedo hacer el new en la misma linea donde declaro timeLevelLimit?
        timeLevelLimit = new float[6];

        timeLevelLimit[0] = 99f;
        timeLevelLimit[1] = 10f;
        timeLevelLimit[2] = 15f;
        timeLevelLimit[3] = 20f;
        timeLevelLimit[4] = 25f;
        timeLevelLimit[5] = 30f;
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
        ATENCION: ESTAS FUNCIONES SON PARA DEBUG. 
        EN LA BUILD FINAL NO INCLUIR EJECUCION PARA EVITAR SOBREESCRIBIR PROFILES DE JUGADOR.
    */
    // PROFE: Como crear carpetas o ficheros por defecto para las builds.

    //Flag de control, desde el inspector habilitamos si debemos o no crear perfiles.
    public bool createEmptyProfilesFLAG;

    //Por defecto todos los profiles están creados pero vacios, el bool profileUsed determina si el usuario puede usarlo o no para crear su profile.    
    private void createEmptyProfiles()
    {

        for (int i = 0; i < profiles.Length; i++)
        {
            profiles[i].profileUsed = false;
            profiles[i].profileName = "New profile";
            //ALERTA: Siempre crear 1 level más de los jugables. El [0] es de testing purposes{GameDevRoom}. 
            //El resto de lvl corresponden su ID con el de la BUILD. ¡Todo lo que no sean niveles de juego deberán ir al final de la build order!
            profiles[i].levelsData = new LevelData[numberOfLevels];
            for (int j = 0; j < numberOfLevels; j++)
            {
                
                //El primer nivel y la DEV-Room está desbloqueado para jugar
                if (j == 0 || j == 1) { profiles[i].levelsData[j].levelUnblockedFLAG = true; }
                else { profiles[i].levelsData[j].levelUnblockedFLAG = false; }

                profiles[i].levelsData[j].firstTimeFLAG = true;
                profiles[i].levelsData[j].levelMedals.finished = false;
                profiles[i].levelsData[j].levelMedals.timeBeated = false;
                profiles[i].levelsData[j].levelMedals.batteryCollected = false;
                profiles[i].levelsData[j].levelMedals.timeRecord = 999f;
                profiles[i].levelsData[j].levelMedals.allAtOnce = false;
            }
        }

        for (int i = 0; i < path.Length; i++)
        {

            BinaryWriter writer = new BinaryWriter(File.Open(path[i], FileMode.Create));

            writer.Write(profiles[i].profileUsed);
            writer.Write(profiles[i].profileName);

            for (int j = 0; j < numberOfLevels; j++)
            {
                writer.Write(profiles[i].levelsData[j].levelUnblockedFLAG);
                writer.Write(profiles[i].levelsData[j].firstTimeFLAG);
                writer.Write(profiles[i].levelsData[j].levelMedals.finished);
                writer.Write(profiles[i].levelsData[j].levelMedals.timeBeated);
                writer.Write(profiles[i].levelsData[j].levelMedals.batteryCollected);
                writer.Write(profiles[i].levelsData[j].levelMedals.timeRecord);
                writer.Write(profiles[i].levelsData[j].levelMedals.allAtOnce);
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
    public void loadProfiles(int _id)
    {
        BinaryReader reader;
        
        //load todos los perfiles
        if (_id == -1)
        {
            for (int i=0; i < path.Length; i++)
            {
                if (File.Exists(path[i]))
                {
                    reader = new BinaryReader(File.Open(path[i], FileMode.Open));
                    profiles[i].profileUsed = reader.ReadBoolean();
                    profiles[i].profileName = reader.ReadString();
                    //ALERTA: Siempre crear 1 level más de los jugables. El [0] es de testing purposes{GameDevRoom}. 
                    //El resto de lvl corresponden su ID con el de la BUILD. ¡Todo lo que no sean niveles de juego deberán ir al final de la build order!
                    profiles[i].levelsData = new LevelData[numberOfLevels];
                    for (int j = 0; j < numberOfLevels; j++)
                    {
                        profiles[i].levelsData[j].levelUnblockedFLAG = reader.ReadBoolean();
                        profiles[i].levelsData[j].firstTimeFLAG = reader.ReadBoolean();
                        profiles[i].levelsData[j].levelMedals.finished = reader.ReadBoolean();
                        profiles[i].levelsData[j].levelMedals.timeBeated = reader.ReadBoolean();
                        profiles[i].levelsData[j].levelMedals.batteryCollected = reader.ReadBoolean();
                        profiles[i].levelsData[j].levelMedals.timeRecord = reader.ReadSingle();
                        profiles[i].levelsData[j].levelMedals.allAtOnce = reader.ReadBoolean();

                        //LOGS DE LA LECTURA
                        //if(i==0 && j==1)
                        //{
                        //    Debug.LogWarning(Time.time + " LOADING PROFILE 0 - MAP 1 // DATA");
                        //    Debug.Log(Time.time + " i: " + i + " |j: " + j + "-used: " + profiles[i].profileUsed);
                        //    Debug.Log(Time.time + " i: " + i + " |j: " + j + "-name: " + profiles[i].profileName);
                        //    Debug.Log(Time.time + " i: " + i + " |j: " + j + "-unblockedFLAG: " + profiles[i].levelsData[j].levelUnblockedFLAG);
                        //    Debug.Log(Time.time + " i: " + i + " |j: " + j + "-firstTimeFLAG: " + profiles[i].levelsData[j].firstTimeFLAG);
                        //    Debug.Log(Time.time + " i: " + i + " |j: " + j + "-M.Finished: " + profiles[i].levelsData[j].levelMedals.finished);
                        //    Debug.Log(Time.time + " i: " + i + " |j: " + j + "-M.Tbeated: " + profiles[i].levelsData[j].levelMedals.timeBeated);
                        //    Debug.Log(Time.time + " i: " + i + " |j: " + j + "-M.batteryCollected: " + profiles[i].levelsData[j].levelMedals.batteryCollected);
                        //    Debug.Log(Time.time + " i: " + i + " |j: " + j + "-M.timeRecord: " + profiles[i].levelsData[j].levelMedals.timeRecord);
                        //}
                    }
                    Debug.LogWarning(Time.time + " LOADED PROFILE " + i + " DATA");
                    reader.Close();
                }
                else
                {
                    Debug.Log("Fail to open " + path[i] + " file.");
                }
            }

            return;
        }

        //load perfil concreto

        if (File.Exists(path[_id]))
        {
            reader = new BinaryReader(File.Open(path[_id], FileMode.Open));
            profiles[_id].profileUsed = reader.ReadBoolean();
            profiles[_id].profileName = reader.ReadString();
            //ALERTA: Siempre crear 1 level más de los jugables. El [0] es de testing purposes{GameDevRoom}. 
            //El resto de lvl corresponden su ID con el de la BUILD. ¡Todo lo que no sean niveles de juego deberán ir al final de la build order!
            profiles[_id].levelsData = new LevelData[numberOfLevels];
            for (int j = 0; j < numberOfLevels; j++)
            {
                profiles[_id].levelsData[j].levelUnblockedFLAG = reader.ReadBoolean();
                profiles[_id].levelsData[j].firstTimeFLAG = reader.ReadBoolean();
                profiles[_id].levelsData[j].levelMedals.finished = reader.ReadBoolean();
                profiles[_id].levelsData[j].levelMedals.timeBeated = reader.ReadBoolean();
                profiles[_id].levelsData[j].levelMedals.batteryCollected = reader.ReadBoolean();
                profiles[_id].levelsData[j].levelMedals.timeRecord = reader.ReadSingle();
                profiles[_id].levelsData[j].levelMedals.allAtOnce = reader.ReadBoolean();

                //LOGS DE LA LECTURA
                //if(i==0 && j==1)
                //{
                //    Debug.LogWarning(Time.time + " LOADING PROFILE 0 - MAP 1 // DATA");
                //    Debug.Log(Time.time + " _id: " + i + " |j: " + j + "-used: " + profiles[_id].profileUsed);
                //    Debug.Log(Time.time + " _id: " + i + " |j: " + j + "-name: " + profiles[_id].profileName);
                //    Debug.Log(Time.time + " _id: " + i + " |j: " + j + "-unblockedFLAG: " + profiles[_id].levelsData[j].levelUnblockedFLAG);
                //    Debug.Log(Time.time + " _id: " + i + " |j: " + j + "-firstTimeFLAG: " + profiles[_id].levelsData[j].firstTimeFLAG);
                //    Debug.Log(Time.time + " _id: " + i + " |j: " + j + "-M.Finished: " + profiles[_id].levelsData[j].levelMedals.finished);
                //    Debug.Log(Time.time + " _id: " + i + " |j: " + j + "-M.Tbeated: " + profiles[_id].levelsData[j].levelMedals.timeBeated);
                //    Debug.Log(Time.time + " _id: " + i + " |j: " + j + "-M.batteryCollected: " + profiles[_id].levelsData[j].levelMedals.batteryCollected);
                //    Debug.Log(Time.time + " _id: " + i + " |j: " + j + "-M.timeRecord: " + profiles[_id].levelsData[j].levelMedals.timeRecord);
                //}
            }
            Debug.LogWarning(Time.time + " LOADED PROFILE " + _id + " DATA");
            reader.Close();
        }
        else
        {
            Debug.Log("Fail to open " + path[_id] + " file.");
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
        newLevelData.levelMedals.batteryCollected = recivedMedals.batteryCollected;
        newLevelData.levelMedals.timeRecord = recivedMedals.timeRecord;

        if(recivedMedals.finished && recivedMedals.timeBeated && recivedMedals.batteryCollected)
        {
            newLevelData.levelMedals.allAtOnce = true;
        } else
        {
            newLevelData.levelMedals.allAtOnce = false;
        }

        profiles[profileSelected].levelsData[idLevel] = newLevelData;

        //SAVE NEXT LEVEL DATA
        //TODO: Controlar cuando juegue el último nivel del juego. idLevel+1 !!!
        LevelData unblockNextLevel;

        unblockNextLevel.levelUnblockedFLAG = false;

        profiles[profileSelected].levelsData[idLevel + 1].levelUnblockedFLAG = true;
        saveDataInProfileBIN();
    }


    //PROFE:¿Se puede hacer algo para ir a la posicion concreta del profile y sobreescribir solo los datos que me interesan?
    //Guarda estado actual del profile en su fichero bin. Se ejecuta al finalizar el saveData tras cada nivel.
    public void saveDataInProfileBIN()
    {
        BinaryWriter writer = new BinaryWriter(File.Open(path[profileSelected], FileMode.Create));
        writer.Write(profiles[profileSelected].profileUsed);
        writer.Write(profiles[profileSelected].profileName);

        for (int j = 0; j < numberOfLevels; j++)
        {
            writer.Write(profiles[profileSelected].levelsData[j].levelUnblockedFLAG);
            writer.Write(profiles[profileSelected].levelsData[j].firstTimeFLAG);
            writer.Write(profiles[profileSelected].levelsData[j].levelMedals.finished);
            writer.Write(profiles[profileSelected].levelsData[j].levelMedals.timeBeated);
            writer.Write(profiles[profileSelected].levelsData[j].levelMedals.batteryCollected);
            writer.Write(profiles[profileSelected].levelsData[j].levelMedals.timeRecord);
            writer.Write(profiles[profileSelected].levelsData[j].levelMedals.allAtOnce);
        }
        Debug.LogWarning(Time.time + " SAVED PROFILE " + profileSelected + " DATA");
        writer.Close();
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
        //Inicializamos tiempos a batir en cada nivel
        setTimeLevelLimit();
        setNumberOfLevels();

        //DEBUG: Creamos perfiles vacios.
        if (createEmptyProfilesFLAG)
        {
            createEmptyProfiles();
        }

        //Leemos los perfiles en la carpeta bin y los cargamos en la lista profiles
        loadProfiles(-1);

        //Init a FALSE
        profilePicked = false;
        
        //TESTING ZONE
        //END TESTING ZONE
    }
}
