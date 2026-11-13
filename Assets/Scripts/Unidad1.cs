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
              
                if (rm.ListaUnidades[0].FirstTime)
                {
                    rm.ListaUnidades[0].FirstTime = false;
                }
                else 
                {
                     CargarDatos(rm.ListaUnidades[0]);
                }

            }
            if (nombre == "Mastropiero")
            {
                if (rm.ListaUnidades[1].FirstTime)
                {
                    rm.ListaUnidades[1].FirstTime = false;
                }
                else
                {
                    CargarDatos(rm.ListaUnidades[1]);
                }

            }
            if (nombre == "Alfredo")
            {
                if (rm.ListaUnidades[2].FirstTime)
                {
                    rm.ListaUnidades[2].FirstTime = false;
                }
                else
                {
                    CargarDatos(rm.ListaUnidades[2]);
                }

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
               GuardarDatos(rm.ListaUnidades[0]);
            }
            if (nombre == "Mastropiero")
            {             
              GuardarDatos(rm.ListaUnidades[1]);
            }
            if (nombre == "Alfredo")
            {
              GuardarDatos(rm.ListaUnidades[2]);
            }
        }

    }
    public void GuardarDatos(PlayerData data)
    {
        data.nombre = nombre;
        data.nivelUnidad = nivelUnidad;
        data.danio = danio;
        data.saludMaxima = saludMaxima;
        data.saludActual = saludActual;
        data.Bloqueando = Bloqueando;
        data.podermagico = podermagico;
        data.fuerza = fuerza;
        data.stunRestante = stunRestante;
        data.mana = mana;
        data.manaMaximo = manaMaximo;


        data.listaCD = new List<string>(listaCD);
        data.habilidades = new List<string>(habilidades);

        data.DebilidadFisica = DebilidadFisica;
        data.DebilidadFuego = DebilidadFuego;
        data.DebilidadHielo = DebilidadHielo;
        data.DebilidadRayo = DebilidadRayo;
        data.DebilidadTierra = DebilidadTierra;
        data.DebilidadAgua = DebilidadAgua;
        data.DebilidadViento = DebilidadViento;
        data.DebilidadBalance = DebilidadBalance;
        data.DebilidadCaos = DebilidadCaos;

        data.EXP = EXP;
        data.EXPNecesaria = EXPNecesaria;
        data.LV = LV;
        data.FirstTime = false;
    }
    public void CargarDatos(PlayerData data)
    {
        nombre = data.nombre;
        nivelUnidad = data.nivelUnidad;
        danio = data.danio;
        saludMaxima = data.saludMaxima;
        saludActual = data.saludActual;
        Bloqueando = data.Bloqueando;
        podermagico = data.podermagico;
        fuerza = data.fuerza;
        stunRestante = data.stunRestante;
        mana = data.mana;
        manaMaximo = data.manaMaximo;


        listaCD = new List<string>(data.listaCD);
        habilidades = new List<string>(data.habilidades);

        DebilidadFisica = data.DebilidadFisica;
        DebilidadFuego = data.DebilidadFuego;
        DebilidadHielo = data.DebilidadHielo;
        DebilidadRayo = data.DebilidadRayo;
        DebilidadTierra = data.DebilidadTierra;
        DebilidadAgua = data.DebilidadAgua;
        DebilidadViento = data.DebilidadViento;
        DebilidadBalance = data.DebilidadBalance;
        DebilidadCaos = data.DebilidadCaos;

        EXP = data.EXP;
        EXPNecesaria = data.EXPNecesaria;
        LV = data.LV;
    }
}