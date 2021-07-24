using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Download : MonoBehaviour
{
    public InputField textTest;
    int score;

    public void ExitButton()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene(0);

    }

    public void TestButton()
    {
        Debug.Log(textTest.text);
        score++;
        textTest.text = score.ToString();
        Debug.Log(score);
    }
}
