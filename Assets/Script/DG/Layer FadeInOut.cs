using System.Collections;
using UnityEngine;

public class LayerFadeInOut : MonoBehaviour
{
    public static LayerFadeInOut instance;
    
    public GameObject[] gameObjects_FadeIn; // GameObj Array
    public GameObject[] gameObjects_FadeOut; // GameObj Array
    
    public GameObject InOutCam;
    
    public float fadeDuration = 1.0f; // Time to change alpha value

    private void Awake()
    {
        instance = this;
        if (InOutCam != null)
        {
            InOutCam.SetActive(false);
        }
    }

    private void Start()
    {
        AudioManager.instance.ChangeBgm(AudioManager.Bgm.CutScene_Old);
        foreach (GameObject obj in gameObjects_FadeIn)
        {
            obj.SetActive(true);
            Renderer renderer = obj.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                Color color = renderer.material.color;
                color.a = 0;
                renderer.material.color = color;
            }
        }
    }
    

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        float currentAlpha = 1.0f;
        // float targetAlpha = 0.0f;
        // float fadeTimer = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            
            float t = elapsedTime / fadeDuration;
            t = Mathf.Clamp01(t);
            
            currentAlpha = 1 - Mathf.Lerp(1.0f, 0.0f, t);
            
            foreach (GameObject obj in gameObjects_FadeIn)
            {
                Renderer renderer = obj.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    Color color = renderer.material.color;
                    color.a = currentAlpha;
                    renderer.material.color = color;
                }
            }
            yield return null;
        }
    }
    
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0.0f;
        float currentAlpha = 1.0f;
        // float targetAlpha = 0.0f;
        // float fadeTimer = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            
            float t = elapsedTime / fadeDuration;
            t = Mathf.Clamp01(t);
            
            currentAlpha = Mathf.Lerp(1.0f, 0.0f, t);
            
            foreach (GameObject obj in gameObjects_FadeOut)
            {
                Renderer renderer = obj.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    Color color = renderer.material.color;
                    color.a = currentAlpha;
                    renderer.material.color = color;
                }
            }
            yield return null;
        }
    }

    public void FadeInOut_CamMove()
    {
        FadeInOut_Image.instance.FadeInOut();
        // InOutCam.SetActive(true);
        Invoke("MovingCam",2.5f);
        Invoke("ShaderTime",1.5f);
    }

    private void MovingCam()
    {
        // InOutCam.SetActive(false);
        Talk.instance.flag = false;
    }

    private void ShaderTime()
    {
        SPShader.instance.ShaderChange();
        AudioManager.instance.ChangeBgm(AudioManager.Bgm.CutScene_Now);
        StartCoroutine(FadeIn());
        StartCoroutine(FadeOut());
    }
}