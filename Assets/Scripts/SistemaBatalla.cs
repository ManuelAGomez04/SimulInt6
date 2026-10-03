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
    public GameManagerScript gameManager;
    public bool isDead;
    public TextMeshProUGUI TextoVida1;
    public TextMeshProUGUI TextoVida2;
    public TextMeshProUGUI TextoVida3;
    public TextMeshProUGUI TextoVidaEnemigo;

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

    public int indexJugador = 0;

    public EstadoBatalla state;
    public bool muerto;
    void Start()
    {
        state = EstadoBatalla.COMIENZO;
        StartCoroutine(ComenzarBatalla());
        TextoVida1.text = unidadJugador1.saludActual.ToString() + "/" + unidadJugador1.saludMaxima.ToString();
        TextoVida2.text = unidadJugador2.saludActual.ToString() + "/" + unidadJugador2.saludMaxima.ToString();
        TextoVida3.text = unidadJugador3.saludActual.ToString() + "/" + unidadJugador3.saludMaxima.ToString();
        TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();

    }
    //tiene que dejar de defender
    IEnumerator AtaqueJugador(Unidad Jugador)
    {
        dialogoTexto.text = " !Le toca a " + Jugador.nombre;
        state = EstadoBatalla.ESPERANDO;
        float danioFinal = Jugador.danio + Jugador.fuerza;
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









        enemigoHUD.vidaSlider.value = unidadEnemigo.saludActual;
        dialogoTexto.text = "Ataque exitoso";
        TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();


        yield return new WaitForSeconds(0.5f);

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
            jugador1HUD.vidaSlider.value = unidadJugador1.saludActual;
            TextoVida1.text = unidadJugador1.saludActual.ToString() + "/" + unidadJugador1.saludMaxima.ToString();
        }

        else if (decision == 2)
        {
            dialogoTexto.text = unidadEnemigo.nombre + " ataca a " + unidadJugador2.nombre;
            yield return new WaitForSeconds(0.5f);
            muerto = unidadJugador2.RecibirDanio(unidadEnemigo.danio);
            jugador2HUD.vidaSlider.value = unidadJugador2.saludActual;
            TextoVida2.text = unidadJugador2.saludActual.ToString() + "/" + unidadJugador2.saludMaxima.ToString();
        }
        else
        {
            dialogoTexto.text = unidadEnemigo.nombre + " ataca a " + unidadJugador3.nombre;
            yield return new WaitForSeconds(0.5f);
            muerto = unidadJugador3.RecibirDanio(unidadEnemigo.danio);
            jugador3HUD.vidaSlider.value = unidadJugador3.saludActual;
            TextoVida3.text = unidadJugador3.saludActual.ToString() + "/" + unidadJugador3.saludMaxima.ToString();

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
    void TurnoJugador()
    {
        indexJugador++;


        if (indexJugador == 4)
        {

            StartCoroutine(TurnoEnemigo());

        }
        else
        {

            dialogoTexto.text = "Tu turno";
        }

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
        if (state != EstadoBatalla.TURNOJUG)
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
        if (state != EstadoBatalla.TURNOJUG)
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
        dialogoTexto.text = Jugador.nombre + " ha curado!";
        state = EstadoBatalla.ESPERANDO;
        yield return new WaitForSeconds(0.5f);
        if (unidadJugador1.saludActual < unidadJugador2.saludActual && unidadJugador1.saludActual < unidadJugador3.saludActual)
        {
            unidadJugador1.saludActual += Jugador.podermagico;
            jugador1HUD.vidaSlider.value = unidadJugador1.saludActual;
            TextoVida1.text = unidadJugador1.saludActual.ToString() + "/" + unidadJugador1.saludMaxima.ToString();
        }
        else if (unidadJugador2.saludActual < unidadJugador1.saludActual && unidadJugador2.saludActual < unidadJugador3.saludActual)
        {
            unidadJugador2.saludActual += Jugador.podermagico;
            jugador2HUD.vidaSlider.value = unidadJugador2.saludActual;
            TextoVida2.text = unidadJugador2.saludActual.ToString() + "/" + unidadJugador2.saludMaxima.ToString();
        }
        else
        {
            unidadJugador3.saludActual += Jugador.podermagico;
            jugador3HUD.vidaSlider.value = unidadJugador3.saludActual;
            TextoVida3.text = unidadJugador3.saludActual.ToString() + "/" + unidadJugador3.saludMaxima.ToString();
        }




       state = EstadoBatalla.TURNOJUG;
        TurnoJugador();




    }
}