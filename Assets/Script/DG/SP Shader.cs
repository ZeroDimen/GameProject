using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SPShader : MonoBehaviour
{
    public static SPShader instance;
    private Material cameraMaterial;
    private float SPScale = 0f;
    
    public float appliedTimeToShader = 0.01f;
    public float appliedTimeToColor = 1.0f;
    private bool _onShader;
    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
        if (ApplyShader())
        {
            SPScale = 1f;
        }
        
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
    

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        cameraMaterial.SetFloat("_SPscale",SPScale);
        Graphics.Blit(src,dest,cameraMaterial);
    }

    public void ShaderChange()
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

    public bool ApplyShader(bool On = false)
    {
        if (On)
        {
            SPScale = 1f;
            return false;
        }
  
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "CutScene_1")
            {
                return true;
            }
        }
        
        return false;
    }
}