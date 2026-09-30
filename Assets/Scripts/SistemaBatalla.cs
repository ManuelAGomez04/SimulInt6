using System.Collections;
using System.Collections.Generic;
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
    public GameManagerScript gameManager;
    public bool isDead;

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

    public Text dialogoTexto;

    public EstadoBatalla state;
    void Start()
    {
        state = EstadoBatalla.COMIENZO;
        StartCoroutine(ComenzarBatalla());
    }
    IEnumerator AtaqueJugador(Unidad Jugador)
    {
        dialogoTexto.text = " !Le toca a " + Jugador.nombre;
        state = EstadoBatalla.ESPERANDO;
        float danioFinal = Jugador.danio;
        int suerte = Random.Range(1, 5);
        int esquivar = Random.Range(1, 10);
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



        bool muerto = unidadEnemigo.RecibirDanio(danioFinal);


        //suerte
        //0 no acertaste
        //1 a 5 normal
        //6 critico









        enemigoHUD.SetSalud(unidadEnemigo.saludActual);
        dialogoTexto.text = "Ataque exitoso";

        yield return new WaitForSeconds(0.5f);

        if (muerto)
        {
            state = EstadoBatalla.VICTORIA;
            TerminarBatalla();
        }
        else
        {
            if (Jugador.nombre == "Sofia")
            {
                StartCoroutine(AtaqueJugador(unidadJugador2));
            }
            if (Jugador.nombre == "Mastropiero")

            {
                StartCoroutine(AtaqueJugador(unidadJugador3));
            }
            if (Jugador.nombre == "Alfredo")

            {
                StartCoroutine(TurnoEnemigo());
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

    IEnumerator TurnoEnemigo()
    {
        dialogoTexto.text = unidadEnemigo.nombre + " ataca!";

        yield return new WaitForSeconds(0.5f);

        int decision = Random.Range(1, 4);

        bool muerto;

        if (decision == 1)
        {
            dialogoTexto.text = unidadEnemigo.nombre + " ataca a " + unidadJugador1.nombre;
            yield return new WaitForSeconds(0.5f);
            muerto = unidadJugador1.RecibirDanio(unidadEnemigo.danio);
            jugador1HUD.SetSalud(unidadJugador1.saludActual);
        }

        else if (decision == 2)
        {
            dialogoTexto.text = unidadEnemigo.nombre + " ataca a " + unidadJugador2.nombre;
            yield return new WaitForSeconds(0.5f);
            muerto = unidadJugador2.RecibirDanio(unidadEnemigo.danio);
            jugador2HUD.SetSalud(unidadJugador2.saludActual);
        }
        else
        {
            dialogoTexto.text = unidadEnemigo.nombre + " ataca a " + unidadJugador3.nombre;
            yield return new WaitForSeconds(0.5f);
            muerto = unidadJugador3.RecibirDanio(unidadEnemigo.danio);
            jugador3HUD.SetSalud(unidadJugador3.saludActual);
        }



        if (muerto)
        {
            state = EstadoBatalla.DERROTA;
            TerminarBatalla();
        }
        else
        {
            state = EstadoBatalla.TURNOJUG;
            TurnoJugador();
        }
    }
    void TerminarBatalla()
    {
        if (state == EstadoBatalla.VICTORIA)
        {
            dialogoTexto.text = "¡Victoria!";
        }
        else if (state == EstadoBatalla.DERROTA && !isDead)
        {
            isDead = true;
            gameManager.gameOver();
            dialogoTexto.text = "Derrota";
        }
    }
    void TurnoJugador()
    {
        dialogoTexto.text = "Tu turno";
    }
    public void AtaqueBoton()
    {
        if (state != EstadoBatalla.TURNOJUG)
            return;
        StartCoroutine(AtaqueJugador(unidadJugador1));
    }
    public void Defenderboton(Unidad Jugador)
    {
        if (state != EstadoBatalla.TURNOJUG)
            return;
        Jugador.Bloqueando = true;
        StartCoroutine(DefenderJugador());
    }
    IEnumerator DefenderJugador()
    {

    }
}