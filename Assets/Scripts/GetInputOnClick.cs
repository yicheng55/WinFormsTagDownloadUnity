using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class GetInputOnClick : MonoBehaviour
{
    public Button btnClick;
    public InputField InputUser;
    
    // Start is called before the first frame update
    void Start()
    {
        Button btn = btnClick.GetComponent< Button>();
        btn.onClick.AddListener(GetInputOnClickHandler);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GetInputOnClickHandler()
    {
        Debug.Log("Log input" + InputUser.text);

    }
}
