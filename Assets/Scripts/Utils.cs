using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public struct Medals
{
    public bool finished; // Nivel superado Si/No
    public bool timeBeated; // Superado en menos de X tiempo Si/No
    public bool batteryCollected; // Recogidas pilas del nivel Si/No
    public float timeRecord; // Marca personal de tiempo record.
    public bool allAtOnce; // Marca si las medallas han sido ganadas todas a la vez
}



public static class Utils
{
    
    // Returns the float number with X decimals.
    public static float RoundFloat(float number, int decimals)
    {
        float mult = Mathf.Pow(10.0f, decimals);
        float rounded = Mathf.Round(number * mult) / mult;
        return rounded;
    }

    // Define la musica actual que está sonando
    public enum PlayingNow
    {
        NONE,
        MAINTHEME,
        TUTORIAL
    };

    public static float GetActualRecord(float _playerTime, float _levelTimeDEV)
    {
        if (_playerTime <= _levelTimeDEV)
        {
            return _playerTime;
        }
        else //if (_playerTime > _levelTimeDEV)
        {
            return _levelTimeDEV;
        }
    }

    public static float GetPercentage(float _num, int _decimals)
    {
        float number;

        number = RoundFloat(_num * 100, _decimals);

        return number;
    }

    public static void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Retorna un float en formato TimeSpan. 
    /*
     * mode (1) > mm:ss:fff
     * mode (2) > mm:ss:ff
     */
    public static string GetTimeFormat (float _number, int mode)
    {
        TimeSpan time;
        time = TimeSpan.FromSeconds(_number);

        switch (mode)
        {
            case 1:
            return time.ToString("mm':'ss':'fff");
            
            case 2:
            return time.ToString("mm':'ss':'ff");

            default:
            return time.ToString("mm':'ss':'fff");
        }      
    }
}
