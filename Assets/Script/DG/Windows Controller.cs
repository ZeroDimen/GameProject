using System;
using UnityEngine;

public class WindowsController : MonoBehaviour
{
    public GameObject Setting_Window;
    public GameObject Save_Window;

    private void Awake()
    {
        Save_Window.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Setting_Window.activeSelf)
            {
                Setting_Window.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Setting_Window.SetActive(false);
                Save_Window.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void Open_Save_Window()
    {
        Save_Window.SetActive(true);
    }
}
