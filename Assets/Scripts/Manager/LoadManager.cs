using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    private static LoadManager _instance;
    private static bool isInLoadingScreen = false;

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
        if (!isInLoadingScreen && SceneManager.GetActiveScene().name == "LoadingScreen")
        {
            StartCoroutine(LoadNewScene("GameScreen"));
        }
    }

    public void InstantLoadScene()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
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

    public IEnumerator LoadNewScene(string scene)
    {
        isInLoadingScreen = true;
        yield return new WaitForSeconds(2f);

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
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
                isInLoadingScreen = false;
            }

            yield return null;
        }
    }
}
