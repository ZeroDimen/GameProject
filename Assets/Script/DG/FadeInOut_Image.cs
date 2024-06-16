using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut_Image : MonoBehaviour
{
    public static FadeInOut_Image instance;
        
    public GameObject img;
    
    private Image FadeImage;

    private void Awake()
    {
        instance = this;
    }
    
    private IEnumerator FadeIn(float In, float Stay, float Out)
    {
        float elapsedTime = 0.0f;
        float currentAlpha = 1.0f;

        while (elapsedTime < In)
        {
            elapsedTime += Time.deltaTime;
            
            float t = elapsedTime / In;
            t = Mathf.Clamp01(t);
            
            currentAlpha = 1 - Mathf.Lerp(1.0f, 0.0f, t);
            
            Image image = img.GetComponent<Image>();
            Color color = image.color;
            color.a = currentAlpha;
            image.color = color;
                
            
            yield return null;
        }

        yield return new WaitForSeconds(Stay);
        StartCoroutine(FadeOut(In, Stay,Out));
    }
    
    IEnumerator FadeOut(float In, float Stay, float Out)
    {
        float elapsedTime = 0.0f;
        float currentAlpha = 1.0f;

        while (elapsedTime < Out)
        {
            elapsedTime += Time.deltaTime;
            
            float t = elapsedTime / Out;
            t = Mathf.Clamp01(t);
            
            currentAlpha = Mathf.Lerp(1.0f, 0.0f, t);
            
            Image image = img.GetComponent<Image>();
            Color color = image.color;
            color.a = currentAlpha;
            image.color = color;
            
            yield return null;
        }

        img.GetComponent<Image>().color = Color.clear;
    }

    public void FadeInOut( float In = 1f, float Stay = 1f, float Out = 0.75f)
    {
        StartCoroutine(FadeIn(In,Stay,Out));
    }
}
