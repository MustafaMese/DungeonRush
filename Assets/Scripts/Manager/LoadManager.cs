using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonRush.Managers
{
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
                SceneManager.LoadScene(1);
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

        public IEnumerator LoadScene()
        {
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            Debug.Log("Pro :" + asyncOperation.progress);
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                //Output the current progress
                //text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    //Change the Text to show the Scene is ready
                    //text.text = "Press the space bar to continue";
                    //Wait to you press the space key to activate the Scene
                    if (Input.GetKeyDown(KeyCode.Space))
                        //Activate the Scene
                        asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}