using System.Collections;
using UnityEngine;

public class LayerFadeInOut : MonoBehaviour
{
    public GameObject[] gameObjects_FadeIn; // GameObj Array
    public GameObject[] gameObjects_FadeOut; // GameObj Array
    
    public float fadeDuration = 1.0f; // Time to change alpha value

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(FadeIn());
            StartCoroutine(FadeOut());
        }
    }
    
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        float currentAlpha = 1.0f;
        float targetAlpha = 0.0f;
        float fadeTimer = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            
            float t = elapsedTime / fadeDuration;
            t = Mathf.Clamp01(t);
            
            currentAlpha = 1 - Mathf.Lerp(1.0f, 0.0f, t);
            
            foreach (GameObject obj in gameObjects_FadeIn)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
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
        float targetAlpha = 0.0f;
        float fadeTimer = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            
            float t = elapsedTime / fadeDuration;
            t = Mathf.Clamp01(t);
            
            currentAlpha = Mathf.Lerp(1.0f, 0.0f, t);
            
            foreach (GameObject obj in gameObjects_FadeOut)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
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
}