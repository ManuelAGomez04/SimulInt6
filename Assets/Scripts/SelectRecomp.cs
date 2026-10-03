using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectRecomp : MonoBehaviour
{
   
    public int indice;
    void Start()
    {
        if (RecompenzaManager.Instance)
        {
            RecompenzaManager rm = RecompenzaManager.Instance;
            if (indice == 1 && rm.recompenza1)
            {
                gameObject.SetActive(false);
            }   
            else if (indice == 2 && rm.recompenza2)
                {
                    gameObject.SetActive(false);
                }
                else if (indice == 3 && rm.recompenza3)
                {
                    gameObject.SetActive(false);
            }
        }
    }

   
    void Update()
    {
        
    }
    public void SeleccionarRecompensa()
    {
        print(" Elija un Boton " + indice);
        if (RecompenzaManager.Instance)
        {
            print("Seleccionaste la recompensa " + indice);
            RecompenzaManager rm = RecompenzaManager.Instance;
            if (indice == 1)
            {
                rm.recompenza1 = true;
            }
            else if (indice == 2)
            {
                rm.recompenza2 = true;
            }
            else if (indice == 3)
            {
                rm.recompenza3 = true;
            }
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
