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
    public float tiempoCarga = 1f;
    private AsyncOperation preloadedSceneOperation;
    // Start is called before the first frame update
    void Start()
    {

        //StartCoroutine(PreloadRoutine());
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
       
        tiempoCarga -= Time.deltaTime;
        if (tiempoCarga <= 0f)
        {
            CambiarEscena();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (text.gameObject)
        {
            text.gameObject.SetActive(false);
        }
    }
}
