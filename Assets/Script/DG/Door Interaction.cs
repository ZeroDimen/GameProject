using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject obj;
    [SerializeField] private SceneField[] _scenesToLoad;
    [SerializeField] private SceneField[] _scenesToUnload;
    private GameObject Player;
    
    
    public Vector3 to_Position;
    private void Start()
    {
        Player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && obj.activeSelf)
        {
            LoadScenes();
            UnloadScenes();
            Player.transform.position = to_Position;
        }
    }

    private void LoadScenes()
    {
        for (int i = 0; i < _scenesToLoad.Length; i++)
        {
            bool isSceneLoaded = false;
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if (loadedScene.name == _scenesToLoad[i].SceneName)
                {
                    isSceneLoaded = true;
                    break;
                }
            }

            if (!isSceneLoaded)
            {
                SceneManager.LoadSceneAsync(_scenesToLoad[i], LoadSceneMode.Additive);
            }
        }
    }
    private void UnloadScenes()
    {
        for (int i = 0; i < _scenesToUnload.Length; i++)
        {
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if (loadedScene.name == _scenesToUnload[i].SceneName)
                {
                    SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                }
            }    
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        obj.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        obj.SetActive(false);
    }
}
