using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDBatalla : MonoBehaviour
{
    public TextMeshProUGUI nombreTexto;
    public TextMeshProUGUI nivelTexto;
    public Slider vidaSlider;
    public Slider manaSlider;

    public void SetHUD(Unidad unidad)
    {
        nombreTexto.text = unidad.nombre;
        nivelTexto.text = "Level" + unidad.nivelUnidad;
        vidaSlider.maxValue = unidad.saludMaxima;
        vidaSlider.value = unidad.saludActual;
        manaSlider.maxValue = unidad.manaMaximo;    
        manaSlider.value = unidad.mana; 
        
    }

    public void SetSalud(int salud)
    {
        vidaSlider.value = salud;
    }

    internal void SetHUD(Unit unidadEnemigo)
    {
        throw new NotImplementedException();
    }

    internal void SetSalud(object saludActual)
    {
        throw new NotImplementedException();
    }

    public void SetMana(int Mana)
    {
      manaSlider.value = Mana;
    }

}
