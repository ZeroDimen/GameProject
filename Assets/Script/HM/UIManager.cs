using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public GameObject EscapeMenu;
    public GameObject LoginPanel;
    public GameObject SlotMenu;
    
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
    }

    private void Update()
    {
        
    }
}
