using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
