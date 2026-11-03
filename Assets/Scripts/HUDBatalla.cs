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
    public Image vidaSlider;
    public Image manaSlider;
    public float saludMaxima;
    public float manaMaximo;
    public TextMeshProUGUI TextoMana;
    public Animator animatorataque;


    public void SetHUD(Unidad unidad)
    {
        nombreTexto.text = unidad.nombre;
        nivelTexto.text = "Level" + unidad.nivelUnidad;
        saludMaxima = unidad.saludMaxima;
        manaMaximo = unidad.manaMaximo;
        vidaSlider.fillAmount = unidad.saludActual / unidad.saludMaxima;
        manaSlider.fillAmount = unidad.mana / unidad.manaMaximo;
        SetMana(unidad.mana);

    }

    public void SetSalud(float salud) 
    {
        vidaSlider.fillAmount = salud/saludMaxima;

    }

    public void AnimacionAtaque()
    {
        animatorataque.gameObject.SetActive(true);
        StartCoroutine(DesactivarAnimacion());

    }
    IEnumerator DesactivarAnimacion()
    {
        yield return new WaitForSeconds(1f);
        animatorataque.gameObject.SetActive(false);
    }

    internal void SetHUD(Unit unidadEnemigo)
    {
        throw new NotImplementedException();
    }

    internal void SetSalud(object saludActual)
    {
        throw new NotImplementedException();
    }

    public void SetMana(float Mana)
    {
      manaSlider.fillAmount = Mana/manaMaximo;
        if (TextoMana != null)
        {
            TextoMana.text = Mana.ToString() + "/" + manaMaximo.ToString();
        }
    }

}
