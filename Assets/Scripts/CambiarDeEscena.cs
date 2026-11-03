using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class CambiarDeEscena : MonoBehaviour
    
{
    public int escenaIndex;
    public TextMeshPro text;
    private AsyncOperation preloadedSceneOperation;
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(PreloadRoutine());
    }

    private IEnumerator PreloadRoutine()
    {
        preloadedSceneOperation = SceneManager.LoadSceneAsync(escenaIndex);


        preloadedSceneOperation.allowSceneActivation = false;


        while (preloadedSceneOperation.progress < 0.9f)
        {
            yield return null;
        }

        Debug.Log("cargo!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CambiarEscena()
    {
        if (preloadedSceneOperation != null)
        {

            preloadedSceneOperation.allowSceneActivation = true;
        }
        else
        {

            SceneManager.LoadScene(escenaIndex);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        text.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E)) 
        { 
        CambiarEscena();  
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        text.gameObject.SetActive(false);
    }
}
