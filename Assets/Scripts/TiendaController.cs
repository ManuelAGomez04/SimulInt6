using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiendaController : MonoBehaviour
{
    GameObject tiendaUI;
    // Start is called before the first frame update
    void Start()
    {
        tiendaUI = GameObject.Find("tiendaUI");
        tiendaUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Jugador")
        {
            ToggleTienda(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ToggleTienda(2);
    }
    private void ToggleTienda(int a = 0)
    { 
        if (a == 1)
        {
            tiendaUI.SetActive(true);
        }
        else
        {
            tiendaUI.SetActive(false);
        }
    }
}
