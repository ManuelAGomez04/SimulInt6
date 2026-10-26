using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int stunRestante;
    public int mana;
    public int manaMaximo;
    public List<string> listaCD = new List<string>();
    public List<string> habilidades = new List<string>();
    public void Start()
    {
        if (RecompenzaManager.Instance)
        {
            RecompenzaManager rm = RecompenzaManager.Instance;
            if (rm.recompenza1 && nombre == "Sofia")
            {
                podermagico += 3;
            }
            if (rm.recompenza2 && nombre == "Mastropiero")
            {
                fuerza += 3;
            }
            if (rm.recompenza1 && nombre == "Alfredo")
            {
                saludMaxima += 15;
                saludActual += 15;
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

        if (saludActual <= -0)
            return true;
        else
            return false;
    }
    public void AgregarCD(string habilidad,int n = 1)
    {
        
        for (int i = 0; i < n; i++)
        {
            listaCD.Add(habilidad);
        }
    }
    public void QuitarCD()
    {
        List<string> listaUnicos = listaCD.Distinct().ToList();
        HashSet<string> elementosVistos = new HashSet<string>();

        for (int i = 0; i < listaCD.Count; i++)
        {

            if (!elementosVistos.Add(listaCD[i]))
            {
                listaCD.RemoveAt(i);
                break;
            }
        }
    }
    public bool chequearCD(string habilidad)
    {
        if (listaCD.Contains(habilidad))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void PasoTurno()
    {
        QuitarCD();
    }
}
