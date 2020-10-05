using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour 
{
    private static LoadManager instance = null;
    // Game Instance Singleton
    public static LoadManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    private void Awake()
    {
        Instance = this;
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

    public void LoadLoseScene()
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

    public static int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
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
    }
}
