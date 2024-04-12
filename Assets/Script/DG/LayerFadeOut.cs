using UnityEngine;

public class LayerFadeOut : MonoBehaviour
{
    public GameObject[] gameObjects; // GameObj Array
    public float fadeDuration = 1.0f; // Time to change alpha value

    private bool isFading = false;
    private float currentAlpha = 1.0f;
    private float targetAlpha = 0.0f;
    private float fadeTimer = 0.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isFading)
        {
            isFading = true;
            currentAlpha = 1.0f;
            targetAlpha = 0.0f;
            fadeTimer = 0.0f;
        }

        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float t = fadeTimer / fadeDuration;
            t = Mathf.Clamp01(t);
            currentAlpha = Mathf.Lerp(1.0f, 0.0f, t);

            foreach (GameObject obj in gameObjects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Color color = renderer.material.color;
                    color.a = currentAlpha;
                    renderer.material.color = color;
                }
            }

            if (t >= 1.0f)
            {
                isFading = false;
            }
        }
    }
}