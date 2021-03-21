using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonRush.Managers
{
    public class LoadManager : MonoBehaviour 
    {
        [SerializeField] int veryEasyFirst;
        [SerializeField] int easyFirst;
        [SerializeField] int moderateFirst;
        [SerializeField] int hardFirst;
        [SerializeField] int veryHardFirst;
        [SerializeField] int bossFirst;

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

            int diffuculty = GameManager.Instance.levelCount / 5;

            if((levelIndex + 1) != SceneManager.sceneCountInBuildSettings)
            {
                switch (diffuculty)
                {
                    case (int)Difficulty.VERY_EASY:
                        LoadRandomLevel(veryEasyFirst, easyFirst);
                        break;
                    case (int)Difficulty.EASY:
                        LoadRandomLevel(easyFirst, moderateFirst);
                        break;
                    case (int)Difficulty.MODERATE:
                        LoadRandomLevel(moderateFirst, hardFirst);
                        break;
                    case (int)Difficulty.HARD:
                        LoadRandomLevel(hardFirst, veryHardFirst);
                        break;
                    case (int)Difficulty.VERY_HARD:
                        LoadRandomLevel(veryHardFirst, bossFirst);
                        break;
                    case (int)Difficulty.BOSS:
                        LoadRandomLevel(bossFirst, SceneManager.sceneCountInBuildSettings - 1);
                        break;
                    default:
                        SceneManager.LoadScene(1);
                        GameManager.Instance.levelCount = 0;
                        break;
                }
            }
            else
            {
                GameManager.Instance.levelCount = 0;
                SceneManager.LoadScene(1);
            }
            
            
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
    }
}