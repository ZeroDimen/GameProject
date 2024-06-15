using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public enum GameStatus
{
    Title,
    Ingame,
    EscapeMenuOpen
}
public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public GameObject EscapeMenu;
    public GameObject SlotMenu;
    public GameObject Login;
    public GameObject Signup;
    public GameObject Player;
    private bool IsEscapeMenuOpen;
    public GameStatus gameStatus;
    public TMP_InputField InputIdField_Signin;
    public TMP_InputField InputPwField_Signin;
    public TMP_InputField InputIdField_Login;
    public TMP_InputField InputPwField_Login;
    public Button Continue;
    public Button Setting;
    public Button Exit;
    public Button LoginBtn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        EscapeMenu.SetActive(false);
        SlotMenu.SetActive(false);
        Signup.SetActive(false);
        Login.SetActive(true);
        
    }

    private void Start()
    {
        gameStatus = GameStatus.Title;
        Player = GameObject.Find("Player");
    }

    private void Update()
    {
        if(gameStatus == GameStatus.Ingame)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (EscapeMenu.activeSelf)
                {
                    EscapeMenu.SetActive(false);
                }
                else
                {
                    EscapeMenu.SetActive(true);
                    Continue.Select();
                }
            }
        }

        if (InputIdField_Login.isFocused||InputPwField_Login.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                InputPwField_Login.Select();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("click enter");
                LoginBtn.onClick.Invoke();
            }
            
            
        }

        if (InputPwField_Signin.isFocused || InputPwField_Signin.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                InputPwField_Signin.Select();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                string s1 = InputIdField_Login.text;
                string s2 = InputPwField_Login.text;
                GetComponent<Database>().CreateAccount(s1, s2);
            }
        }
    }

    public void LoginButton()
    {
        GameManager.instance.id = InputIdField_Login.text;
        GameManager.instance.pw = InputPwField_Login.text;
        
        //서버에서 불러오기?
        GetComponent<Database>().LoadData();
        
        GetComponent<SaveData>().LoadData();
        SlotMenu.SetActive(true);
        GetComponent<SaveData>().SlotLoad();
        Login.SetActive(false);
        
        //this.gameObject.SetActive(false);
    }

    public void LoginToSignUp()
    {
        Login.SetActive(false);
        Signup.SetActive(true);
    }

    public void SignupToLogin()
    {
        Signup.SetActive(false);
        Login.SetActive(true);
    }

    public void ExitButtonInGame()
    {
        //현재 세이브 파일 저장 불가능함 일단은 슬롯 메뉴 창으로 돌아가는 걸로 or 타이틀화면 나오면 타이틀 화면으로 ㄱㄱ
        
    }

    public void ContinueButton()
    {
        
    }
}
