using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
     public Animator anim;
    public GameObject tutorial;


    void Start()
    {
        if(tutorial != null)
        {
            tutorial.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openTutorial()
    {
        tutorial.SetActive(true);
    }

    public void closeTutorial()
    {
        tutorial.SetActive(false);
    }


    public void loadscene(int scenetoload)
    {

       

        anim.Play("FadeOut");
        StartCoroutine(delay(scenetoload));

    }

    IEnumerator delay(int scenetoload)
    {
        yield return new WaitForSeconds(2);
        print("Coroutine ended: " + Time.time + " seconds");
        SceneManager.LoadScene(scenetoload);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
