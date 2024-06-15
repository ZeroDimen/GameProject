using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Resolution : MonoBehaviour
{
    // Start is called before the first frame update
    //public Button Left;
    public string[] arr = {"1920 x 1080", "1600 x 900","1440 x 900","1366 x 768", "1280 x 720"};
    public int[] width = { 1920, 1600, 1440, 1366, 1280 };
    public int[] height = { 1080, 900, 900, 768, 720 };
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(Screen.width);
            Debug.Log(Screen.height);
        }
    }
}
