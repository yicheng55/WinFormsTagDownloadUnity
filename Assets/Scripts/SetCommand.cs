using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetCommand : MonoBehaviour
{

    private TCPTestClient tCPTestClient;

    // Start is called before the first frame update
    void Start()
    {
        tCPTestClient = new TCPTestClient();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ConnectButton()
    {
        Debug.Log("ConnectButton()!!!!!!");
        tCPTestClient.ConnectButton();
    }

    public void SendmagButton()
    {
        Debug.Log("SendmagButton()!!!!!!");
        tCPTestClient.SendMessageButton();
    }

    public void ReturnButton()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene(0);

    }

}
