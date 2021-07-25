using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void DownLoadButton()
    {
        Debug.Log("DownLoadButton!!!");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }


}
