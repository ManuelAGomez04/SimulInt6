using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum EstadoBatalla { COMIENZO, TURNOJUG, TURNOENEM, VICTORIA, DERROTA, ESPERANDO }

public class SistemaBatalla : MonoBehaviour
{
    public GameObject jugadorPrefab1;
    public GameObject jugadorPrefab2;
    public GameObject jugadorPrefab3;
    public GameObject enemigoPrefab;
    public GameObject selector; 
    public GameManagerScript gameManager;
    public bool isDead;
    public TextMeshProUGUI TextoVida1;
    public TextMeshProUGUI TextoVida2;
    public TextMeshProUGUI TextoVida3;
    public TextMeshProUGUI TextoVidaEnemigo;
    public TMP_Dropdown dropdownAtaques;

    public Transform jugador1PosBatalla;
    public Transform jugador2PosBatalla;
    public Transform jugador3PosBatalla;
    public Transform enemigoPosBatalla;

    Unidad unidadJugador1;
    Unidad unidadJugador2;
    Unidad unidadJugador3;
    Unidad unidadEnemigo;

    public HUDBatalla jugador1HUD;
    public HUDBatalla jugador2HUD;
    public HUDBatalla jugador3HUD;
    public HUDBatalla enemigoHUD;
    public List<GameObject> botones;
    public GameObject SelectorA;
    public GameObject Prefavboton;
    public GameObject menu;
    public void crearbotones(Unidad jugador)
    {
        int index = 0;
        foreach (string opcion in jugador.habilidades)
        { 
            GameObject boton = Instantiate(Prefavboton, SelectorA.transform);
           // boton.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(AtaqueJugador(jugador)));
           boton.transform.SetParent(menu.transform, true);
            boton.GetComponent<RectTransform>().anchoredPosition = new Vector2( 295, 210 - 60 * index);
            index++;
            botones.Add(boton);
            boton.GetComponentInChildren<TextMeshProUGUI>().text = opcion;
        }
    }
    public void destruirbotones()
    {
        foreach (GameObject boton in botones)
        {
            Destroy(boton);
        }
        botones.Clear();
    }

    public void ActivarBotones()
    {
         SelectorA.SetActive(true);
        foreach (GameObject boton in botones)
        {

            boton.SetActive(true);
        }
    }

    public void DesactivarBotones()
    {
        SelectorA.SetActive(false);
        foreach (GameObject boton in botones)
        {
            boton.SetActive(false);
        }
    }

    public Text dialogoTexto;

    public int indexJugador = 0;

    public EstadoBatalla state;
    public bool muerto;
    public List <string> patronAtaquesEnemigo;
    public int indexPatron;
    public float TiempoDeEspera = 1f;
    void Start()
    {
        state = EstadoBatalla.COMIENZO;
        StartCoroutine(ComenzarBatalla());
        TextoVida1.text = unidadJugador1.saludActual.ToString() + "/" + unidadJugador1.saludMaxima.ToString();
        TextoVida2.text = unidadJugador2.saludActual.ToString() + "/" + unidadJugador2.saludMaxima.ToString();
        TextoVida3.text = unidadJugador3.saludActual.ToString() + "/" + unidadJugador3.saludMaxima.ToString();
        TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
        DesactivarBotones();
        destruirbotones();
    }
    //tiene que dejar de defender
    IEnumerator AtaqueJugador(Unidad Jugador)
    {
       
        state = EstadoBatalla.ESPERANDO;
        dropdownAtaques.ClearOptions();
        dropdownAtaques.options.Add(new TMP_Dropdown.OptionData() { text = "Elegir una opción" });
        dropdownAtaques.gameObject.SetActive(true);
        crearbotones(Jugador);
        ActivarBotones();


        for (int i = 0; i < Jugador.habilidades.Count; i++)
        {
            dropdownAtaques.options.Add(new TMP_Dropdown.OptionData() { text = Jugador.habilidades[i] });
        }
        bool muerto = false;    
 
        dropdownAtaques.onValueChanged.AddListener((index) =>
        {
            if (dropdownAtaques.options[index].text == "Ataque básico")
            {

                dialogoTexto.text = Jugador.nombre + " ataca a " + unidadEnemigo.nombre;
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio, "Fisico");
                StartCoroutine(ChequearResistencias(unidadEnemigo, "Fisico"));
                enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();

            }
            //Inicia Aumento
            if (dropdownAtaques.options[index].text == "Aumento" && Jugador.mana >= 5 && Jugador.chequearCD("Aumento") == false) 
            {

                dialogoTexto.text = Jugador.nombre + " ´¡La fuerza de " + Jugador.nombre + " aumenta!";
                Jugador.fuerza += 2;
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 5;  
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Aumento", 2);
            }
            else if (dropdownAtaques.options[index].text == "Aumento" && Jugador.mana < 5)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return; 
            }
            else if (dropdownAtaques.options[index].text == "Aumento" && Jugador.chequearCD("Aumento") == true)
            {
                dialogoTexto.text = "Recuperando Habilidad.";
                return;
            }
            //Fin Aumento
            //Inicia Ataque
            if (dropdownAtaques.options[index].text == "Ataque Aturdidor" && Jugador.mana >= 7 && Jugador.chequearCD("Ataque Aturdidor") == false)
            {

                dialogoTexto.text = Jugador.nombre + " ataca a " + unidadEnemigo.nombre;
                unidadEnemigo.stunRestante += 2;
               enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 7;
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Ataque Aturdidor", 2);
            }
            else if (dropdownAtaques.options[index].text == "Ataque Aturdidor" && Jugador.mana < 7)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return;
            }
            else if (dropdownAtaques.options[index].text == "Ataque Aturdidor" && Jugador.chequearCD("Ataque Aturdidor") == true)
            {
                dialogoTexto.text = "Recuperando Habilidad.";
                return;
            }
            //Fin Ataque 
            //Magia 
            if (dropdownAtaques.options[index].text == "Bru" && Jugador.mana >= 5 && Jugador.chequearCD("Bru") == false)
            {

                dialogoTexto.text = Jugador.nombre + " Lanza llamas a " + unidadEnemigo.nombre;
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio, "Fuego");
                StartCoroutine(ChequearResistencias(unidadEnemigo, "Fuego"));
                enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 5;
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Bru", 2);
            }
            else if (dropdownAtaques.options[index].text == "Bru" && Jugador.mana < 7)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return;
            }

            if (dropdownAtaques.options[index].text == "Chio" && Jugador.mana >= 5 && Jugador.chequearCD("Chio") == false)
            {

                dialogoTexto.text = Jugador.nombre + " Congela a  " + unidadEnemigo.nombre;
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio, "Hielo");
                StartCoroutine(ChequearResistencias(unidadEnemigo, "Hielo"));
                enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 5;
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Chio", 2);
            }
            else if (dropdownAtaques.options[index].text == "Chio" && Jugador.mana < 7)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return;
            }
            if (dropdownAtaques.options[index].text == "Ful" && Jugador.mana >= 5 && Jugador.chequearCD("Ful") == false)
            {

                dialogoTexto.text = Jugador.nombre + " Truena a  " + unidadEnemigo.nombre;
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio, "Rayo");
                StartCoroutine(ChequearResistencias(unidadEnemigo, "Rayo"));
               enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 5;
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Ful", 2);
            }
            else if (dropdownAtaques.options[index].text == "Ful" && Jugador.mana < 7)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return;
            }
            if (dropdownAtaques.options[index].text == "Rea" && Jugador.mana >= 5 && Jugador.chequearCD("Rea") == false)
            {

                dialogoTexto.text = Jugador.nombre + " Entierra a  " + unidadEnemigo.nombre;
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio, "Tierra");
                StartCoroutine(ChequearResistencias(unidadEnemigo, "Tierra"));
               enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 5;
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Rea", 2);
            }
            else if (dropdownAtaques.options[index].text == "Rea" && Jugador.mana < 7)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return;
            }
            if (dropdownAtaques.options[index].text == "Nero" && Jugador.mana >= 5 && Jugador.chequearCD("Nero") == false)
            {

                dialogoTexto.text = Jugador.nombre + " Inunda a  " + unidadEnemigo.nombre;
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio, "Agua");
                StartCoroutine(ChequearResistencias(unidadEnemigo, "Agua"));
               enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 5;
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Nero", 2);
            }
            else if (dropdownAtaques.options[index].text == "Nero" && Jugador.mana < 7)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return;
            }
            if (dropdownAtaques.options[index].text == "Aze" && Jugador.mana >= 5 && Jugador.chequearCD("Aze") == false)
            {

                dialogoTexto.text = Jugador.nombre + " Vuela a  " + unidadEnemigo.nombre;
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio, "Viento");
                StartCoroutine(ChequearResistencias(unidadEnemigo, "Viento"));
               enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 5;
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Aze", 2);
            }
            else if (dropdownAtaques.options[index].text == "Aze" && Jugador.mana < 7)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return;
            }
            if (dropdownAtaques.options[index].text == "Dyo" && Jugador.mana >= 5 && Jugador.chequearCD("Dyo") == false)
            {

                dialogoTexto.text = Jugador.nombre + " Ilumina a  " + unidadEnemigo.nombre;
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio, "Balance");
                StartCoroutine(ChequearResistencias(unidadEnemigo, "Balance"));
               enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 5;
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Dyo", 2);
            }
            else if (dropdownAtaques.options[index].text == "Dyo" && Jugador.mana < 7)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return;
            }
            if (dropdownAtaques.options[index].text == "Kata" && Jugador.mana >= 5 && Jugador.chequearCD("Kata") == false)
            {

                dialogoTexto.text = Jugador.nombre + " Oscurece a  " + unidadEnemigo.nombre;
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio, "Caos");
                StartCoroutine(ChequearResistencias(unidadEnemigo, "Caos"));
               enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 5;
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Kata", 2);
            }
            else if (dropdownAtaques.options[index].text == "Kata" && Jugador.mana < 7)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return;
            }
            if (dropdownAtaques.options[index].text == "Agni" && Jugador.mana >= 5 && Jugador.chequearCD("Agni") == false)
            {

                dialogoTexto.text = Jugador.nombre + " Detona a  " + unidadEnemigo.nombre;
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio);
               enemigoHUD.SetSalud(unidadEnemigo.saludActual);
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();
                HUDBatalla hud = seleccionarHud(Jugador);
                Jugador.mana -= 5;
                hud.SetMana(Jugador.mana);
                Jugador.AgregarCD("Agni", 2);
            }
            else if (dropdownAtaques.options[index].text == "Agni" && Jugador.mana < 7)
            {
                dialogoTexto.text = "Maná Insuficiente.";
                return;
            }
            dropdownAtaques.gameObject.SetActive(false);
            HUDBatalla hudJugador = seleccionarHud(Jugador);
            hudJugador.AnimacionAtaque();

            if (muerto)
            {
                state = EstadoBatalla.VICTORIA;
                TerminarBatalla();
            }
            else
            {
                state = EstadoBatalla.TURNOJUG;
                TurnoJugador();

            }
            dropdownAtaques.onValueChanged.RemoveAllListeners(); // Remove the listener after selection
        }
        );


















        
        float danioFinal = Jugador.danio + Jugador.fuerza;
        int suerte = UnityEngine.Random.Range(1, 5);
        int esquivar = UnityEngine.Random.Range(1, 10);
        print("suerte:" + suerte.ToString());
        print("esquivar:" + esquivar.ToString());

        //1 al 6 no pasa nada
        //7 y 8 =0.5
        //9 al 10 esquiva completamente

        if (esquivar <= 6)
        {
            //no hacemos nada

        }
        else if (esquivar <= 8)
        {
            danioFinal = danioFinal / 2;


        }
        else
        {//9 y 10

            danioFinal = 0;
        }

        //print(suerte);

        if (suerte == 6)
        {
            danioFinal = danioFinal * 2;


        }
        else
        {

            //normal

        }



       


        //suerte
        //0 no acertaste
        //1 a 5 normal
        //6 critico
       
        yield return new WaitForSeconds(0.5f);

    }
    IEnumerator ChequearResistencias(Unidad unidad, string tipo)
    {
        if (tipo == "Fisico")
        {
            if (unidad.DebilidadFisica > 1)
            {
                dialogoTexto.text = unidad.nombre + " es débil al daño físico.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadFisica < 1 && unidad.DebilidadFisica > 0)
            {
                dialogoTexto.text = unidad.nombre + " es resistente al daño físico.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadFisica == 0)
            {
                dialogoTexto.text = unidad.nombre + " es inmune al daño físico.";
                yield return new WaitForSeconds(1f);
            }
        }
        else if (tipo == "Fuego")
        {
            if (unidad.DebilidadFuego > 1)
            {
                dialogoTexto.text = unidad.nombre + " es débil al daño de fuego.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadFuego < 1 && unidad.DebilidadFuego > 0)
            {
                dialogoTexto.text = unidad.nombre + " es resistente al daño de fuego.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadFuego == 0)
            {
                dialogoTexto.text = unidad.nombre + " es inmune al daño de fuego.";
                yield return new WaitForSeconds(1f);
            }
        }
        else if (tipo == "Hielo")
        {
            if (unidad.DebilidadHielo > 1)
            {
                dialogoTexto.text = unidad.nombre + " es débil al daño de hielo.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadHielo < 1 && unidad.DebilidadHielo > 0)
            {
                dialogoTexto.text = unidad.nombre + " es resistente al daño de hielo.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadHielo == 0)
            {
                dialogoTexto.text = unidad.nombre + " es inmune al daño de hielo.";
                yield return new WaitForSeconds(1f);
            }
        }
        else if (tipo == "Rayo")
        {
            if (unidad.DebilidadRayo > 1)
            {
                dialogoTexto.text = unidad.nombre + " es débil al daño de rayo.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadRayo < 1 && unidad.DebilidadRayo > 0)
            {
                dialogoTexto.text = unidad.nombre + " es resistente al daño de rayo.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadRayo == 0)
            {
                dialogoTexto.text = unidad.nombre + " es inmune al daño de rayo.";
                yield return new WaitForSeconds(1f);
            }
        }
        else if (tipo == "Tierra")
        {
            if (unidad.DebilidadTierra > 1)
            {
                dialogoTexto.text = unidad.nombre + " es débil al daño de tierra.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadTierra < 1 && unidad.DebilidadTierra > 0)
            {
                dialogoTexto.text = unidad.nombre + " es resistente al daño de tierra.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadTierra == 0)
            {
                dialogoTexto.text = unidad.nombre + " es inmune al daño de tierra.";
                yield return new WaitForSeconds(1f);
            }
        }
        else if (tipo == "Agua")
        {
            if (unidad.DebilidadAgua > 1)
            {
                dialogoTexto.text = unidad.nombre + " es débil al daño de agua.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadAgua < 1 && unidad.DebilidadAgua > 0)
            {
                dialogoTexto.text = unidad.nombre + " es resistente al daño de agua.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadAgua == 0)
            {
                dialogoTexto.text = unidad.nombre + " es inmune al daño de agua.";
                yield return new WaitForSeconds(1f);
            }
        }
        else if (tipo == "Viento")
        {
            if (unidad.DebilidadViento > 1)
            {
                dialogoTexto.text = unidad.nombre + " es débil al daño de viento.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadViento < 1 && unidad.DebilidadViento > 0)
            {
                dialogoTexto.text = unidad.nombre + " es resistente al daño de viento.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadViento == 0)
            {
                dialogoTexto.text = unidad.nombre + " es inmune al daño de viento.";
                yield return new WaitForSeconds(1f);
            }
        }
        else if (tipo == "Balance")
        {
            if (unidad.DebilidadBalance > 1)
            {
                dialogoTexto.text = unidad.nombre + " es débil al daño de balance.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadBalance < 1 && unidad.DebilidadBalance > 0)
            {
                dialogoTexto.text = unidad.nombre + " es resistente al daño de balance.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadBalance == 0)
            {
                dialogoTexto.text = unidad.nombre + " es inmune al daño de balance.";
                yield return new WaitForSeconds(1f);
            }
        }
        else if (tipo == "Caos")
        {
            if (unidad.DebilidadCaos > 1)
            {
                dialogoTexto.text = unidad.nombre + " es débil al daño de caos.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadCaos < 1 && unidad.DebilidadCaos > 0)
            {
                dialogoTexto.text = unidad.nombre + " es resistente al daño de caos.";
                yield return new WaitForSeconds(1f);
            }
            else if (unidad.DebilidadCaos == 0)
            {
                dialogoTexto.text = unidad.nombre + " es inmune al daño de caos.";
                yield return new WaitForSeconds(1f);
            }

        }




        }
    IEnumerator ComenzarBatalla()
    {
        GameObject jugador1Go = Instantiate(jugadorPrefab1, jugador1PosBatalla);
        unidadJugador1 = jugador1Go.GetComponent<Unidad>();

        GameObject jugador2Go = Instantiate(jugadorPrefab2, jugador2PosBatalla);
        unidadJugador2 = jugador2Go.GetComponent<Unidad>();

        GameObject jugador3Go = Instantiate(jugadorPrefab3, jugador3PosBatalla);
        unidadJugador3 = jugador3Go.GetComponent<Unidad>();

        GameObject enemigoGo = Instantiate(enemigoPrefab, enemigoPosBatalla);
        unidadEnemigo = enemigoGo.GetComponent<Unidad>();

        jugador1HUD.SetHUD(unidadJugador1);
        jugador2HUD.SetHUD(unidadJugador2);
        jugador3HUD.SetHUD(unidadJugador3);
        enemigoHUD.SetHUD(unidadEnemigo);

        yield return new WaitForSeconds(2f);

        state = EstadoBatalla.TURNOJUG;
        TurnoJugador();
    }

    public Unidad seleccionarJugadorAzar()
    {
        int decision = UnityEngine.Random.Range(1, 4);
        print("decision: " + decision.ToString());

        if (decision == 1)
        {
            return unidadJugador1;
        }

        else if (decision == 2)
        {
            return unidadJugador2;
        }
        else
        {
            return unidadJugador3;

        }
    }
    public HUDBatalla seleccionarHud(Unidad unidad)
        {
        if (unidad == unidadJugador1)
        {
            return jugador1HUD;
        }
        else if (unidad == unidadJugador2)
        {
            return jugador2HUD;
        }
        else 
        {
            return jugador3HUD;
        }
    }
    public TextMeshProUGUI seleccionarTexto(Unidad unidad)
    {
        if (unidad == unidadJugador1)
        {
            return TextoVida1;
        }
        else if (unidad == unidadJugador2)
        {
            return TextoVida2;
        }
        else
        {
            return TextoVida3;
        }
    }
    IEnumerator TurnoEnemigo()
    { 


        bool muerto = false;
        Unidad pj = seleccionarJugadorAzar();
        HUDBatalla hud = seleccionarHud(pj);
        TextMeshProUGUI texto = seleccionarTexto(pj);
        if (patronAtaquesEnemigo[indexPatron] == "Ataque básico")
        {
            
            dialogoTexto.text = unidadEnemigo.nombre + " ataca a " + pj.nombre;
            yield return new WaitForSeconds(0.5f);
            muerto = pj.RecibirDanio(unidadEnemigo.danio, "Fisico");
            StartCoroutine(ChequearResistencias(pj, "Fisico"));
           hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

        } 
        if (patronAtaquesEnemigo[indexPatron] == "Ataque grande")
        {
            
            dialogoTexto.text = unidadEnemigo.nombre + " ataca a " + pj.nombre;
            yield return new WaitForSeconds(0.5f);
            muerto = pj.RecibirDanio(unidadEnemigo.danio * 2, "Fisico");
            StartCoroutine(ChequearResistencias(pj, "Fisico"));
           hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();
        }
        if (patronAtaquesEnemigo[indexPatron] == "Ataque aturdidor")
        {
            
            dialogoTexto.text = unidadEnemigo.nombre + " aturde a " + pj.nombre;
            yield return new WaitForSeconds(0.5f);
            pj.stunRestante += 3;
           hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();
        }
        if (patronAtaquesEnemigo[indexPatron] == "Lluvia de Flechas")
        {

            dialogoTexto.text = unidadEnemigo.nombre + " LLueven flechas sobre el grupo ";
            yield return new WaitForSeconds(0.5f);
            bool muerto1 = unidadJugador1.RecibirDanio(unidadEnemigo.danio, "Fisico");
            bool muerto2 = unidadJugador2.RecibirDanio(unidadEnemigo.danio, "Fisico");
            bool muerto3 = unidadJugador3.RecibirDanio(unidadEnemigo.danio, "Fisico");
            StartCoroutine(ChequearResistencias(unidadJugador1, "Fisico"));
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(ChequearResistencias(unidadJugador2, "Fisico"));
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(ChequearResistencias(unidadJugador3, "Fisico"));
            yield return new WaitForSeconds(0.5f);
            jugador1HUD.SetSalud(unidadJugador1.saludActual);
            jugador2HUD.SetSalud(unidadJugador2.saludActual);
            jugador3HUD.SetSalud(unidadJugador3.saludActual);
            TextoVida1.text = unidadJugador1.saludActual.ToString() + "/" + unidadJugador1.saludMaxima.ToString();
            TextoVida2.text = unidadJugador2.saludActual.ToString() + "/" + unidadJugador2.saludMaxima.ToString();
            TextoVida3.text = unidadJugador3.saludActual.ToString() + "/" + unidadJugador3.saludMaxima.ToString();
            if (muerto1 || muerto2 || muerto3)
            {
                muerto = true;
            }
        }
        if (patronAtaquesEnemigo[indexPatron] == "Bru" && unidadEnemigo.mana >= 5 && unidadEnemigo.chequearCD("Bru") == false)
        {

            dialogoTexto.text = unidadEnemigo.nombre + " Lanza llamas a " + pj.nombre;
            muerto = pj.RecibirDanio(unidadEnemigo.danio, "Fuego");
            StartCoroutine(ChequearResistencias(pj, "Fuego"));
            hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

            unidadEnemigo.mana -= 5;
            enemigoHUD.SetMana(unidadEnemigo.mana);
            unidadEnemigo.AgregarCD("Bru", 2);
        }
        else if (patronAtaquesEnemigo[indexPatron] == "Bru" && unidadEnemigo.mana < 7)
        {
            dialogoTexto.text = "Maná Insuficiente.";

        }

        if (patronAtaquesEnemigo[indexPatron] == "Chio" && unidadEnemigo.mana >= 5 && unidadEnemigo.chequearCD("Chio") == false)
        {

            dialogoTexto.text = unidadEnemigo.nombre + " Congela a  " + pj.nombre;
            muerto = pj.RecibirDanio(unidadEnemigo.danio, "Hielo");
            StartCoroutine(ChequearResistencias(pj, "Hielo"));
            hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

            unidadEnemigo.mana -= 5;
            enemigoHUD.SetMana(unidadEnemigo.mana);
            unidadEnemigo.AgregarCD("Chio", 2);
        }
        else if (patronAtaquesEnemigo[indexPatron] == "Chio" && unidadEnemigo.mana < 7)
        {
            dialogoTexto.text = "Maná Insuficiente.";

        }
        if (patronAtaquesEnemigo[indexPatron] == "Ful" && unidadEnemigo.mana >= 5 && unidadEnemigo.chequearCD("Ful") == false)
        {

            dialogoTexto.text = unidadEnemigo.nombre + " Truena a  " + pj.nombre;
            muerto = pj.RecibirDanio(unidadEnemigo.danio, "Rayo");
            StartCoroutine(ChequearResistencias(pj, "Rayo"));
            hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

            unidadEnemigo.mana -= 5;
            enemigoHUD.SetMana(unidadEnemigo.mana);
            unidadEnemigo.AgregarCD("Ful", 2);
        }
        else if (patronAtaquesEnemigo[indexPatron] == "Ful" && unidadEnemigo.mana < 7)
        {
            dialogoTexto.text = "Maná Insuficiente.";

        }
        if (patronAtaquesEnemigo[indexPatron] == "Rea" && unidadEnemigo.mana >= 5 && unidadEnemigo.chequearCD("Rea") == false)
        {

            dialogoTexto.text = unidadEnemigo.nombre + " Entierra a  " + pj.nombre;
            muerto = pj.RecibirDanio(unidadEnemigo.danio, "Tierra");
            StartCoroutine(ChequearResistencias(pj, "Tierra"));
            hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

            unidadEnemigo.mana -= 5;
            enemigoHUD.SetMana(unidadEnemigo.mana);
            unidadEnemigo.AgregarCD("Rea", 2);
        }
        else if (patronAtaquesEnemigo[indexPatron] == "Rea" && unidadEnemigo.mana < 7)
        {
            dialogoTexto.text = "Maná Insuficiente.";

        }
        if (patronAtaquesEnemigo[indexPatron] == "Nero" && unidadEnemigo.mana >= 5 && unidadEnemigo.chequearCD("Nero") == false)
        {

            dialogoTexto.text = unidadEnemigo.nombre + " Inunda a  " + pj.nombre;
            muerto = pj.RecibirDanio(unidadEnemigo.danio, "Agua");
            StartCoroutine(ChequearResistencias(pj, "Agua"));
            hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

            unidadEnemigo.mana -= 5;
            enemigoHUD.SetMana(unidadEnemigo.mana);
            unidadEnemigo.AgregarCD("Nero", 2);
        }
        else if (patronAtaquesEnemigo[indexPatron] == "Nero" && unidadEnemigo.mana < 7)
        {
            dialogoTexto.text = "Maná Insuficiente.";

        }
        if (patronAtaquesEnemigo[indexPatron] == "Aze" && unidadEnemigo.mana >= 5 && unidadEnemigo.chequearCD("Aze") == false)
        {

            dialogoTexto.text = unidadEnemigo.nombre + " Vuela a  " + pj.nombre;
            muerto = pj.RecibirDanio(unidadEnemigo.danio, "Viento");
            StartCoroutine(ChequearResistencias(pj, "Viento"));
            hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

            unidadEnemigo.mana -= 5;
            enemigoHUD.SetMana(unidadEnemigo.mana);
            unidadEnemigo.AgregarCD("Aze", 2);
        }
        else if (patronAtaquesEnemigo[indexPatron] == "Aze" && unidadEnemigo.mana < 7)
        {
            dialogoTexto.text = "Maná Insuficiente.";

        }
        if (patronAtaquesEnemigo[indexPatron] == "Dyo" && unidadEnemigo.mana >= 5 && unidadEnemigo.chequearCD("Dyo") == false)
        {

            dialogoTexto.text = unidadEnemigo.nombre + " Ilumina a  " + pj.nombre;
            muerto = pj.RecibirDanio(unidadEnemigo.danio, "Balance");
            StartCoroutine(ChequearResistencias(pj, "Balance"));
            hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

            unidadEnemigo.mana -= 5;
            enemigoHUD.SetMana(unidadEnemigo.mana);
            unidadEnemigo.AgregarCD("Dyo", 2);
        }
        else if (patronAtaquesEnemigo[indexPatron] == "Dyo" && unidadEnemigo.mana < 7)
        {
            dialogoTexto.text = "Maná Insuficiente.";

        }
        if (patronAtaquesEnemigo[indexPatron] == "Kata" && unidadEnemigo.mana >= 5 && unidadEnemigo.chequearCD("Kata") == false)
        {

            dialogoTexto.text = unidadEnemigo.nombre + " Oscurece a  " + pj.nombre;
            muerto = pj.RecibirDanio(unidadEnemigo.danio, "Caos");
            StartCoroutine(ChequearResistencias(pj, "Caos"));
            hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

            unidadEnemigo.mana -= 5;
            enemigoHUD.SetMana(unidadEnemigo.mana);
            unidadEnemigo.AgregarCD("Kata", 2);
        }
        else if (patronAtaquesEnemigo[indexPatron] == "Kata" && unidadEnemigo.mana < 7)
        {
            dialogoTexto.text = "Maná Insuficiente.";

        }
        if (patronAtaquesEnemigo[indexPatron] == "Agni" && unidadEnemigo.mana >= 5 && unidadEnemigo.chequearCD("Agni") == false)
        {

            dialogoTexto.text = unidadEnemigo.nombre + " Detona a  " + pj.nombre;
            muerto = pj.RecibirDanio(unidadEnemigo.danio);
            hud.SetSalud(pj.saludActual);
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

            unidadEnemigo.mana -= 5;
            enemigoHUD.SetMana(unidadEnemigo.mana);
            unidadEnemigo.AgregarCD("Agni", 2);
        }
        else if (patronAtaquesEnemigo[indexPatron] == "Agni" && unidadEnemigo.mana < 7)
        {
            dialogoTexto.text = "Maná Insuficiente.";

        }



        indexPatron++; 
        if (indexPatron >= patronAtaquesEnemigo.Count)
        {
            indexPatron = 0; 
        }


        if (muerto)
        {
            state = EstadoBatalla.DERROTA;
            TerminarBatalla();
        }
        else
        {
            state = EstadoBatalla.TURNOJUG;
            indexJugador = 0;
            TurnoJugador();
        }
    }
    void TerminarBatalla()
    {
        if (state == EstadoBatalla.VICTORIA)
        {
            gameManager.Ganador();
            if (RecompenzaManager.Instance)
            {
                RecompenzaManager rm = RecompenzaManager.Instance;
                rm.dinero += 27;

            }
            dialogoTexto.text = "Ganaste 27 de Oro!";
        }
        else if (state == EstadoBatalla.DERROTA && !isDead)
        {
            isDead = true;
            gameManager.gameOver();
            dialogoTexto.text = "Derrota";
        }
    }
IEnumerator Esperar(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        indexJugador++;

        if (indexJugador == 1)
        {
            unidadJugador1.PasoTurno();
            dialogoTexto.text = "¡Le toca a " + unidadJugador1.nombre + "!";
            selector.SetActive(true);
            selector.transform.position = unidadJugador1.transform.parent.position + new Vector3(0, 2.20f, 0);

        }
        if (indexJugador == 2)
        {
            unidadJugador2.PasoTurno();
            dialogoTexto.text = "¡Le toca a " + unidadJugador2.nombre + "!";
            selector.SetActive(true);
            selector.transform.position = unidadJugador2.transform.parent.position + new Vector3(0, 2.20f, 0);

        }
        if (indexJugador == 3)
        {
            unidadJugador3.PasoTurno();
            dialogoTexto.text = "¡Le toca a " + unidadJugador3.nombre + "!";
            selector.SetActive(true);
            selector.transform.position = unidadJugador3.transform.parent.position + new Vector3(0, 2.20f, 0);
        }

        if (indexJugador == 1 && unidadJugador1.stunRestante > 0)
        {
            dialogoTexto.text = "Sofía está inmovilizada.";
            unidadJugador1.stunRestante--;
            indexJugador++;
        }
        if (indexJugador == 2 && unidadJugador2.stunRestante > 0)
        {
            dialogoTexto.text = "Mastropiero está inmovilizado.";
            unidadJugador2.stunRestante--;
            indexJugador++;
        }
        if (indexJugador == 3 && unidadJugador3.stunRestante > 0)
        {
            dialogoTexto.text = "Alfredo está inmovilizado.";
            unidadJugador3.stunRestante--;
            indexJugador++;
        }

        if (indexJugador == 4)
        {
            selector.SetActive(false);
            if (unidadEnemigo.stunRestante > 0)
            {
                dialogoTexto.text = unidadEnemigo.nombre + " está inmovilizado.";
                unidadEnemigo.stunRestante--;
                state = EstadoBatalla.TURNOJUG;
                indexJugador = 0;
                TurnoJugador();

            }
            else
            {
                StartCoroutine(TurnoEnemigo());

            }


        }


    }
    void TurnoJugador()
    {
        StartCoroutine(Esperar(TiempoDeEspera));
       
    }
    public void AtaqueBoton()
    {
        if (state != EstadoBatalla.TURNOJUG)
            return;

        if (indexJugador == 1)
        {


            StartCoroutine(AtaqueJugador(unidadJugador1));

        }
        if (indexJugador == 2)
        {


            StartCoroutine(AtaqueJugador(unidadJugador2));

        }

        if (indexJugador == 3)
        {


            StartCoroutine(AtaqueJugador(unidadJugador3));

        }




    }
    public void Defenderboton()

    {
        dropdownAtaques.gameObject.SetActive(false);
        if (state != EstadoBatalla.TURNOJUG && state != EstadoBatalla.ESPERANDO)
            return;

        if (indexJugador == 1)
        {


            StartCoroutine(DefenderJugador(unidadJugador1));

        }
        if (indexJugador == 2)
        {


            StartCoroutine(DefenderJugador(unidadJugador2));

        }

        if (indexJugador == 3)
        {


            StartCoroutine(DefenderJugador(unidadJugador3));

        }
    }
    IEnumerator DefenderJugador(Unidad Jugador)
    {
        dialogoTexto.text = Jugador.nombre + " se defiende!";
        state = EstadoBatalla.ESPERANDO;
        yield return new WaitForSeconds(0.5f);
        Jugador.Bloqueando = true;



        
        if (muerto)
        {
            state = EstadoBatalla.VICTORIA;
            TerminarBatalla();
        }
        else
        {
            state = EstadoBatalla.TURNOJUG;
            TurnoJugador();

        }


   
    
    }
    public void Curarboton()
    {
        dropdownAtaques.gameObject.SetActive(false);
        if (state != EstadoBatalla.TURNOJUG && state != EstadoBatalla.ESPERANDO)
            return;

        if (indexJugador == 1)
        {


            StartCoroutine(CurarJugador(unidadJugador1));

        }
        if (indexJugador == 2)
        {


            StartCoroutine(CurarJugador(unidadJugador2));

        }

        if (indexJugador == 3)
        {


            StartCoroutine(CurarJugador(unidadJugador3));

        }

   
    }

     IEnumerator CurarJugador(Unidad Jugador)
    {
        print("Curando a " + Jugador.nombre);
        dialogoTexto.text = Jugador.nombre + " ha curado!";
        state = EstadoBatalla.ESPERANDO;
        yield return new WaitForSeconds(0.5f);
        if (unidadJugador1.saludActual < unidadJugador2.saludActual && unidadJugador1.saludActual < unidadJugador3.saludActual)
        {
            unidadJugador1.saludActual += Jugador.podermagico;
            jugador1HUD.SetSalud(unidadJugador1.saludActual);
            TextoVida1.text = unidadJugador1.saludActual.ToString() + "/" + unidadJugador1.saludMaxima.ToString();
        }
        else if (unidadJugador2.saludActual < unidadJugador1.saludActual && unidadJugador2.saludActual < unidadJugador3.saludActual)
        {
            unidadJugador2.saludActual += Jugador.podermagico;
            jugador2HUD.SetSalud(unidadJugador2.saludActual);
            TextoVida2.text = unidadJugador2.saludActual.ToString() + "/" + unidadJugador2.saludMaxima.ToString();
        }
        else
        {
            unidadJugador3.saludActual += Jugador.podermagico;
            jugador3HUD.SetSalud(unidadJugador3.saludActual);
            TextoVida3.text = unidadJugador3.saludActual.ToString() + "/" + unidadJugador3.saludMaxima.ToString();
        }




       state = EstadoBatalla.TURNOJUG;
        TurnoJugador();




    }
    //1)Enemigos detectan debilidad de jugador,
    //enemigo tiene mas probabilidad de atacar a jugador que sea debil a
    //tipo de ataque del enemigo

    //Golpe aturdidor o stun, tanto enemigo como jjugador lo pueden hacer, el objetivo no puede atacar por varios turnos
    //el jugador tiiene la habilidad de salir del stun con un boton.

    //Postura: mecanica, 
    IEnumerator Enemigo1() 
    {
        yield return null;
    }
}