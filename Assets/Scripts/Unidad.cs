using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unidad : MonoBehaviour
{
    public string nombre;
    public int nivelUnidad;
    public int danio;
    public int saludMaxima;
    public int saludActual;

    public bool RecibirDanio(int danio)
    {
        saludActual -= danio;

        if (saludActual < -0)
            return true;
        else
            return false;
    }
}
