using DungeonRush.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    private static LoadManager _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        if (GameManager.gameState == GameState.LEVEL_TRANSITION)
           LoadNextScene();

        if (GameManager.gameState == GameState.START)
            StartCoroutine(LoadNewScene());
    }

    public void LoadNextScene()
    {
        var scene = SceneManager.GetActiveScene();
        var n = scene.buildIndex + 1;

        if(n != SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(scene.buildIndex + 1);
        else
            SceneManager.LoadScene(0);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public static void LoadLoseScene()
    {
        SceneManager.LoadScene("LoseScreen");
    }
    public void LoadLoadingScreen()
    {
        SceneManager.LoadScene("LoadingScreen");
    }

    public void RestartScene()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public IEnumerator LoadNewScene()
    {
        var scene = SceneManager.GetActiveScene();
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene.buildIndex + 1);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // Loading completed
            if (ao.progress == 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
        print("bum");
    }
}
