using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

enum GameStatus
{
    Title,
    Ingame,
    EscapeMenuOpen
}
public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public GameObject EscapeMenu;
    public GameObject LoginPanel;
    public GameObject SlotMenu;
    public TMP_Text Idtext;
    public TMP_Text Pwtext;
    private bool IsEscapeMenuOpen;
    private GameStatus _gameStatus;

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
        LoginPanel.SetActive(true);
        _gameStatus = GameStatus.Title;
    }

    private void Update()
    {
        //그거 뭐야 이스케이프 키 눌렀을때 반응
    }

    public void LoginButton()
    {
        GameManager.instance.id = Idtext.text;
        GameManager.instance.pw = Pwtext.text;
        SlotMenu.SetActive(true);
        //서버에서 불러오기?
        LoginPanel.SetActive(false);
        //this.gameObject.SetActive(false);
    }

    public void ExitButton()
    {
        
    }
}
