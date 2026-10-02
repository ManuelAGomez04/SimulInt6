using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unidad : MonoBehaviour
{
    public string nombre;
    public int nivelUnidad;
    public float danio;
    public float saludMaxima;
    public float saludActual;
    public bool Bloqueando;
    public float podermagico;
    public float fuerza;
    public void Start()
    {
        if (RecompenzaManager.Instance)
        {
            RecompenzaManager rm = RecompenzaManager.Instance;
            if (rm.recompenza1 && nombre == "Sofia")
            {
                podermagico += 3;
            }
        }
     
    }
    public bool RecibirDanio(float danio)
    {
        
        if (Bloqueando)
        {
         saludActual -= danio / 1.5f;   
        }
        else       
        {
            saludActual -= danio;
        }

        if (saludActual < -0)
            return true;
        else
            return false;
    }
}
