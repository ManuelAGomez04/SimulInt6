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

    public Text dialogoTexto;

    public int indexJugador = 0;

    public EstadoBatalla state;
    public bool muerto;
    public List <string> patronAtaquesEnemigo;
    public int indexPatron;
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
        dropdownAtaques.ClearOptions();
        dropdownAtaques.options.Add(new TMP_Dropdown.OptionData() { text = "Elegir una opción" });
        dropdownAtaques.gameObject.SetActive(true);
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
                muerto = unidadEnemigo.RecibirDanio(Jugador.danio);
                enemigoHUD.vidaSlider.value = unidadEnemigo.saludActual;
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();

            }

            if (dropdownAtaques.options[index].text == "Ataque Aturdidor")
            {

                dialogoTexto.text = Jugador.nombre + " ataca a " + unidadEnemigo.nombre;
                unidadEnemigo.stunRestante += 2;
                enemigoHUD.vidaSlider.value = unidadEnemigo.saludActual;
                TextoVidaEnemigo.text = unidadEnemigo.saludActual.ToString() + "/" + unidadEnemigo.saludMaxima.ToString();

            }

            dropdownAtaques.gameObject.SetActive(false);
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


















        //
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



       


        //suerte
        //0 no acertaste
        //1 a 5 normal
        //6 critico
       
        yield return new WaitForSeconds(0.5f);

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
        int decision = Random.Range(1, 4);
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

        if (patronAtaquesEnemigo[indexPatron] == "Ataque básico")
        {
            Unidad pj = seleccionarJugadorAzar();
            HUDBatalla hud = seleccionarHud(pj);
            TextMeshProUGUI texto = seleccionarTexto(pj);
            dialogoTexto.text = unidadEnemigo.nombre + " ataca a " + pj.nombre;
            yield return new WaitForSeconds(0.5f);
            muerto = pj.RecibirDanio(unidadEnemigo.danio);
            hud.vidaSlider.value = pj.saludActual;
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();

        } 
        if (patronAtaquesEnemigo[indexPatron] == "Ataque grande")
        {
            Unidad pj = seleccionarJugadorAzar();
            HUDBatalla hud = seleccionarHud(pj);
            TextMeshProUGUI texto = seleccionarTexto(pj);
            dialogoTexto.text = unidadEnemigo.nombre + " ataca a " + pj.nombre;
            yield return new WaitForSeconds(0.5f);
            muerto = pj.RecibirDanio(unidadEnemigo.danio * 2);
            hud.vidaSlider.value = pj.saludActual;
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();
        }
        if (patronAtaquesEnemigo[indexPatron] == "Ataque aturdidor")
        {
            Unidad pj = seleccionarJugadorAzar();
            HUDBatalla hud = seleccionarHud(pj);
            TextMeshProUGUI texto = seleccionarTexto(pj);
            dialogoTexto.text = unidadEnemigo.nombre + " aturde a " + pj.nombre;
            yield return new WaitForSeconds(0.5f);
            pj.stunRestante += 3;
            hud.vidaSlider.value = pj.saludActual;
            texto.text = pj.saludActual.ToString() + "/" + pj.saludMaxima.ToString();
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
    void TurnoJugador()
    {
        indexJugador++;
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
        dropdownAtaques.gameObject.SetActive(false);
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
        dropdownAtaques.gameObject.SetActive(false);
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
        print("Curando a " + Jugador.nombre);
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