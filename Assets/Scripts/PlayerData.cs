using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Información General")]
    public string nombre;
    public int nivelUnidad;
    public float LV;
    public float EXP;
    public float EXPNecesaria;

    [Header("Estadísticas de Combate")]
    public float saludMaxima;
    public float saludActual;
    public int manaMaximo;
    public int mana;
    public float danio;
    public float fuerza;
    public float podermagico;

    [Header("Estados")]
    public bool Bloqueando;
    public int stunRestante;

    [Header("Habilidades y Cooldowns")]
    public List<string> habilidades = new List<string>();
    public List<string> listaCD = new List<string>();

    [Header("Debilidades Elementales")]
    public float DebilidadFisica;
    public float DebilidadFuego;
    public float DebilidadHielo;
    public float DebilidadRayo;
    public float DebilidadTierra;
    public float DebilidadAgua;
    public float DebilidadViento;
    public float DebilidadBalance;
    public float DebilidadCaos;

    public bool FirstTime = true;
    public void CopyFrom(PlayerData other)
    {
        if (other == null) return;

        nombre = other.nombre;
        nivelUnidad = other.nivelUnidad;
        LV = other.LV;
        EXP = other.EXP;
        EXPNecesaria = other.EXPNecesaria;

        saludMaxima = other.saludMaxima;
        saludActual = other.saludActual;
        manaMaximo = other.manaMaximo;
        mana = other.mana;
        danio = other.danio;
        fuerza = other.fuerza;
        podermagico = other.podermagico;

        Bloqueando = other.Bloqueando;
        stunRestante = other.stunRestante;


        habilidades = new List<string>(other.habilidades);
        listaCD = new List<string>(other.listaCD);

        DebilidadFisica = other.DebilidadFisica;
        DebilidadFuego = other.DebilidadFuego;
        DebilidadHielo = other.DebilidadHielo;
        DebilidadRayo = other.DebilidadRayo;
        DebilidadTierra = other.DebilidadTierra;
        DebilidadAgua = other.DebilidadAgua;
        DebilidadViento = other.DebilidadViento;
        DebilidadBalance = other.DebilidadBalance;
        DebilidadCaos = other.DebilidadCaos;
        FirstTime = other.FirstTime;
    }
}