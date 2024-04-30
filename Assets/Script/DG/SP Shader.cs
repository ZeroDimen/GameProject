using System.Collections;
using TMPro;
using UnityEngine;

public class SPShader : MonoBehaviour
{
    
    private Material cameraMaterial;
    public float SPScale = 0.0f;
    
    public float appliedTimeToShader = 0.01f;
    public float appliedTimeToColor = 1.0f;
    private bool _onShader;
    private void Start()
    {
        if (SPScale <= 1.0f)
        {
            _onShader = true;
        }
        else
        {
            _onShader = false;
        }
        
        cameraMaterial = new Material(Shader.Find("Custom/SPscale"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameCameraEffect();
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        cameraMaterial.SetFloat("_SPscale",SPScale);
        Graphics.Blit(src,dest,cameraMaterial);
    }

    public void gameCameraEffect()
    {
        if (_onShader == false)
        {
            StartCoroutine(gameOnShaderEffect());
        }
        else
        {
            StartCoroutine(gameOffShaderEffect());
        }
        
    }
    
    private IEnumerator gameOffShaderEffect()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < appliedTimeToColor)
        {
            elapsedTime += Time.deltaTime;

            SPScale = 1 - (elapsedTime / appliedTimeToColor);
            yield return null;
        }
        _onShader = false;
    }
    private IEnumerator gameOnShaderEffect()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < appliedTimeToShader)
        {
            elapsedTime += Time.deltaTime;

            SPScale = elapsedTime / appliedTimeToShader;
            yield return null;
        }
        _onShader = true;
    }
    
}