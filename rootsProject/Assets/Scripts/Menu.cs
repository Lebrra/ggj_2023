using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
   
    //This method gets called when the object is first created. It will get called even if the object is disabled.
    public void Awake()
    {
    }

    //This method gets called when this object is destroyed.
    public void OnDestroy()
    {
       
    }

  
    //A method that we want to hook up to when the restart button gets pressed
    public void Restart()
    {
        //Add code here to restart the game

        SceneManager.LoadScene("MainGameScene");
    }

    public static void Quit()
    {
        //quit the application
        Application.Quit();
    }
}
