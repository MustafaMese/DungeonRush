using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonRush.Managers
{
    public class LoadManager : MonoBehaviour 
    {
        private const int START_POINT = 4;
        private const int SKIP_COUNT = 5;

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
            var levelIndex = scene.buildIndex;

            if((levelIndex + 1) != SceneManager.sceneCountInBuildSettings)
            {
                if(levelIndex == 0)
                    SceneManager.LoadScene(levelIndex + 1);
                else if(levelIndex == 1)
                    LoadRandomLevel(2, 4);
                else if(levelIndex > 1 && levelIndex < 4)
                    LoadRandomLevel(4, 9);
                else if(levelIndex < 9)
                    LoadRandomLevel(9, 14);
                else if(levelIndex < 14)
                    LoadRandomLevel(14, 19);
                else if(levelIndex < 19)
                    LoadRandomLevel(19, 24);
                else if(levelIndex < 24)
                    LoadRandomLevel(24, 29);
            }
            else
                SceneManager.LoadScene(1);
        }

        private void LoadRandomLevel(int min, int max)
        {
            int index = Random.Range(min, max);
            SceneManager.LoadScene(index);
        }

        public void LoadStartScene()
        {
            SceneManager.LoadScene("StartScreen");
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