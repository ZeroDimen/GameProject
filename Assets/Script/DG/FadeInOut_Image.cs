using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut_Image : MonoBehaviour
{
    public static FadeInOut_Image instance;
        
    public GameObject img;
    public float fadeInDuration = 1.0f;
    public float waitDuration = 1.0f;
    public float fadeOutDuration = 1.0f;
    
    private Image FadeImage;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }
    
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        float currentAlpha = 1.0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            
            float t = elapsedTime / fadeInDuration;
            t = Mathf.Clamp01(t);
            
            currentAlpha = 1 - Mathf.Lerp(1.0f, 0.0f, t);
            
            Image image = img.GetComponent<Image>();
            Color color = image.color;
            color.a = currentAlpha;
            image.color = color;
                
            
            yield return null;
        }

        yield return new WaitForSeconds(waitDuration);
        StartCoroutine(FadeOut());
    }
    
    IEnumerator FadeOut()
    {
        float elapsedTime = 0.0f;
        float currentAlpha = 1.0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            
            float t = elapsedTime / fadeInDuration;
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

    public void FadeInOut()
    {
        StartCoroutine(FadeIn());
    }
}
