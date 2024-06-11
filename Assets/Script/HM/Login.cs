using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_Text Idtext;
    public TMP_Text Pwtext;
    
    public void LoginButton()
    {
        Data_Manager.instance.id = Idtext.text;
        Data_Manager.instance.pw = Pwtext.text;
        Destroy(this.gameObject);
    }
}
