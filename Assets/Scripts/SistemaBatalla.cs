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

    public EstadoBatalla state;
    void Start()
    {
        state = EstadoBatalla.COMIENZO;
        ComenzarBatalla();
    }

    void ComenzarBatalla()
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
    }
}
