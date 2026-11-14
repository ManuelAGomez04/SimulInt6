using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Comprar : MonoBehaviour
{
    public int precio = 15;
    public int indexCompra;
    public TMPro.TextMeshProUGUI DineroTotal;
    public TMPro.TextMeshProUGUI Dialogo;
    // Start is called before the first frame update
    void Start()
    {
        
        
       

        if (RecompenzaManager.Instance != null)
        {
            RecompenzaManager rm = RecompenzaManager.Instance;
            DineroTotal.text = "Pesoros: " + rm.dinero.ToString();


            if (rm.ItemsTienda != null )
            {



                for (int i = 0; i < rm.ItemsTienda.Count; i++) 
                {
                    if (rm.ItemsTienda[indexCompra] == true)
                    {
                       gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void comprar()
    {
        if (RecompenzaManager.Instance != null)
        {
            RecompenzaManager rm = RecompenzaManager.Instance;
            if (rm.dinero >= precio)
            {
                rm.dinero -= precio;
                logicaCompra(indexCompra);
                DineroTotal.text = "Pesoros: " + rm.dinero.ToString();
                Debug.Log("Compra realizada. Dinero restante: " + rm.dinero);
            }
            else
            {
                Debug.Log("No tienes suficiente dinero para comprar.");
            }
        }
    }
    public void logicaCompra(int compra)
    {
        if (1 == compra)
        {
            if (RecompenzaManager.Instance != null)
            {
                RecompenzaManager rm = RecompenzaManager.Instance;
               rm.ListaUnidades[0].saludActual += rm.ListaUnidades[0].saludMaxima;
               rm.ListaUnidades[1].saludActual += rm.ListaUnidades[1].saludMaxima;
               rm.ListaUnidades[2].saludActual += rm.ListaUnidades[2].saludMaxima;
                Dialogo.text = "Tu Grupo se ha curado";

            }
        }
        if (2 == compra)
        {
            if (RecompenzaManager.Instance != null)
            {
                RecompenzaManager rm = RecompenzaManager.Instance;
                rm.ListaUnidades[0].DebilidadAgua = 0.50f;
               rm.ItemsTienda[compra] = true;
                Dialogo.text = "Sofia ahora es resistente al Agua";
                gameObject.SetActive(false);

            }
        }
    }
}
