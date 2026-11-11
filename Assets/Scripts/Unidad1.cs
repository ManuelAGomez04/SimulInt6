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
    public float DebilidadFisica;
    public float DebilidadFuego;
    public float DebilidadHielo;
    public float DebilidadRayo;
    public float DebilidadTierra;
    public float DebilidadAgua;
    public float DebilidadViento;
    public float DebilidadBalance;
    public float DebilidadCaos;
    public float EXP;
    public float EXPNecesaria;
    public float LV;

    public void Start()
    {
        if (RecompenzaManager.Instance)
        {
            RecompenzaManager rm = RecompenzaManager.Instance;
            if (nombre == "Sofia")
            {
                if (rm.ListaUnidades[0] == null)
                {
                    rm.ListaUnidades[0] = this;
                }
                rm.ListaUnidades[0].saludActual = saludActual;
                rm.ListaUnidades[0].saludMaxima = saludMaxima;
                rm.ListaUnidades[0].danio = danio;
                rm.ListaUnidades[0].podermagico = podermagico;
                rm.ListaUnidades[0].mana = mana;
                rm.ListaUnidades[0].EXP = EXP;
                rm.ListaUnidades[0].EXPNecesaria = EXPNecesaria;
                rm.ListaUnidades[0].LV = LV;

            }
            if (nombre == "Mastropiero")
            {
                if (rm.ListaUnidades[1] == null)
                {
                    rm.ListaUnidades[1] = this;
                }
                rm.ListaUnidades[1].saludActual = saludActual;
                rm.ListaUnidades[1].saludMaxima = saludMaxima;
                rm.ListaUnidades[1].danio = danio;
                rm.ListaUnidades[1].podermagico = podermagico;
                rm.ListaUnidades[1].mana = mana;
                rm.ListaUnidades[1].EXP = EXP;
                rm.ListaUnidades[1].EXPNecesaria = EXPNecesaria;
                rm.ListaUnidades[1].LV = LV;

            }
            if (nombre == "Alfredo")
            {
                if (rm.ListaUnidades[2] == null)
                {
                    rm.ListaUnidades[2] = this;
                }
                rm.ListaUnidades[2].saludActual = saludActual;
                rm.ListaUnidades[2].saludMaxima = saludMaxima;
                rm.ListaUnidades[2].danio = danio;
                rm.ListaUnidades[2].podermagico = podermagico;
                rm.ListaUnidades[2].mana = mana;
                rm.ListaUnidades[2].EXP = EXP;
                rm.ListaUnidades[2].EXPNecesaria = EXPNecesaria;
                rm.ListaUnidades[2].LV = LV;

            }
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
    public bool RecibirDanio(float danio, string tipo = "")

    {
        if (tipo == "Fisico")
        {
            danio *= DebilidadFisica;
        }
        else if (tipo == "Fuego")
        {
            danio *= DebilidadFuego;
        }
        else if (tipo == "Hielo")
        {
            danio *= DebilidadHielo;
        }
        else if (tipo == "Rayo")
        {
            danio *= DebilidadRayo;
        }
        else if (tipo == "Tierra")
        {
            danio *= DebilidadTierra;
        }
        else if (tipo == "Agua")
        {
            danio *= DebilidadAgua;
        }
        else if (tipo == "Viento")
        {
            danio *= DebilidadViento;
        }
        else if (tipo == "Balance")
        {
            danio *= DebilidadBalance;
        }
        else if (tipo == "Caos")
        {
            danio *= DebilidadCaos;
        }

        if (Bloqueando)
        {
            saludActual -= danio / 1.5f;
        }
        else
        {
            saludActual -= danio;
        }
        Bloqueando = false;
        if (saludActual <= -0)
            return true;
        else
            return false;
    }
    public void AgregarCD(string habilidad, int n = 1)
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

    public void GanarEXP(float exp)
    {
        EXP += exp;
        if (EXP >= EXPNecesaria)
        {
            LV++;
            EXP = 0;
            nivelUnidad++;
            saludMaxima += 10;
            saludActual = saludMaxima;
            podermagico += 1;
            fuerza += 1;
            EXPNecesaria += 50;
        }
        if (RecompenzaManager.Instance)
        {
            RecompenzaManager rm = RecompenzaManager.Instance;
            if (nombre == "Sofia")
            {   
                    rm.ListaUnidades[0] = this;         
            }
            if (nombre == "Mastropiero")
            {             
                    rm.ListaUnidades[1] = this;                
            }
            if (nombre == "Alfredo")
            {
              rm.ListaUnidades[2] = this;
            }
        }

    }
}