using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarOro : MonoBehaviour
{
    public Text texto;
    // Start is called before the first frame update
    void Start()
    {
        if (RecompenzaManager.Instance)
        {
            RecompenzaManager rm = RecompenzaManager.Instance;
           texto.text = "Oro: " + rm.dinero.ToString();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
