using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDBatalla : MonoBehaviour
{
    public Text nombreTexto;
    public Text nivelTexto;
    public Slider vidaSlider;

    public void SetHUD(Unidad unidad)
    {
        nombreTexto.text = unidad.nombre;
        nivelTexto.text = "Level" + unidad.nivelUnidad;
        vidaSlider.maxValue = unidad.saludMaxima;
        vidaSlider.value = unidad.saludActual;
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
}
