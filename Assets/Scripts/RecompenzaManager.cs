using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RecompenzaManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static RecompenzaManager Instance { get; private set; }
    public bool recompenza1 = false;
    public bool recompenza2 = false;
    public bool recompenza3 = false;
    public int dinero = 0;
    public List<Unidad> habilidadesDesbloqueadas;
    public List<PlayerData> ListaUnidades;
    public List<PlayerData> ListaUnidades2;
    public List<bool> JefesDerrotados;
    public Vector2 posicionJugador;
    public List<bool> ItemsTienda;
    private void Awake()
    {
        print (ListaUnidades[0].FirstTime);
        DontDestroyOnLoad(this.gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            for (int i = 0; i < 3; i++)
            {
                if (ListaUnidades[i] != null && ListaUnidades2[i] != null)
                {

                    ListaUnidades[i].CopyFrom(ListaUnidades2[i]);
                }
            }
        }

    }
}
