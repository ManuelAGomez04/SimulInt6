using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarDeEscena : MonoBehaviour
    
{
    public int escenaIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CambiarEscena()
    {
        SceneManager.LoadScene(escenaIndex);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        { 
        CambiarEscena();  
        }
    }
}
