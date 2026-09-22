using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum EstadoBatalla{ COMIENZO, TURNOJUG, TURNOENEM, VICTORIA, DERROTA }

public class SistemaBatalla : MonoBehaviour
{
    public GameObject jugadorPrefab1;
    public GameObject jugadorPrefab2;
    public GameObject jugadorPrefab3;
    public GameObject enemigoPrefab;

    public Transform jugador1PosBatalla;
    public Transform jugador2PosBatalla;
    public Transform jugador3PosBatalla;
    public Transform enemigoPosBatalla;

    Unit unidadJugador1;
    Unit unidadJugador2;
    Unit unidadJugador3;
    Unit unidadEnemigo;

    public HUDBatalla jugadorHUD;
    public HUDBatalla enemigoHUD;

    public Text dialogoTexto;

    public EstadoBatalla state;
    void Start()
    {
        state = EstadoBatalla.COMIENZO;
        StartCoroutine(ComenzarBatalla());
    }

    IEnumerator ComenzarBatalla()
    {
        GameObject jugador1Go = Instantiate(jugadorPrefab1, jugador1PosBatalla);
        unidadJugador1 = jugador1Go.GetComponent<Unit>();

        GameObject jugador2Go = Instantiate(jugadorPrefab2, jugador2PosBatalla);
        unidadJugador2 = jugador2Go.GetComponent<Unit>();

        GameObject jugador3Go = Instantiate(jugadorPrefab3, jugador3PosBatalla);
        unidadJugador3 = jugador3Go.GetComponent<Unit>();

        GameObject enemigoGo = Instantiate(enemigoPrefab, enemigoPosBatalla);
        unidadEnemigo = enemigoGo.GetComponent<Unit>();

        jugadorHUD.SetHUD(unidadJugador1);
        enemigoHUD.SetHUD(unidadEnemigo);

        yield return new WaitForSeconds(2f);

        state = EstadoBatalla.TURNOJUG;
        TurnoJugador();
    }
    IEnumerator AtaqueJugador()
    {
        bool muerto = unidadEnemigo.RecibirDanio(unidadJugador1.danio);

        enemigoHUD.SetSalud(unidadEnemigo.saludActual);
        dialogoTexto.text = "Ataque exitoso";

        yield return new WaitForSeconds(2f);

        if (muerto)
        {
            state = EstadoBatalla.VICTORIA;
            TerminarBatalla();
        }
        else
        {
            state = EstadoBatalla.TURNOENEM;
            StartCoroutine(TurnoEnemigo());
        }
    }
    IEnumerator TurnoEnemigo()
    {
        dialogoTexto.text = unidadEnemigo.nombre + " ataca!";

        yield return new WaitForSeconds(2f);

        bool muerto = unidadJugador1.RecibirDanio(unidadEnemigo.danio);
        
        jugadorHUD.SetSalud(unidadJugador1.saludActual);

        yield return new WaitForSeconds(1f);

        if(muerto)
        {
            state = EstadoBatalla.DERROTA;
            TerminarBatalla();
        }else
        {
            state = EstadoBatalla.TURNOJUG;
            TurnoJugador();
        }
    }
    void TerminarBatalla()
    {
        if(state == EstadoBatalla.VICTORIA)
        {
            dialogoTexto.text = "¡Victoria!";
        }else if (state == EstadoBatalla.DERROTA)
        {
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
        StartCoroutine(AtaqueJugador());
    }
}
