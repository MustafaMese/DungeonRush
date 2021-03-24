using System.Collections.Generic;
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

        public Stack<int> difficultyStack = new Stack<int>();

        private static LoadManager instance = null;
        // Game Instance Singleton
        public static LoadManager Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void Fill()
        {
            difficultyStack.Push(1);
            print(1);
            for (var i = 5; i > 1; i--)
                for (var y = 0; y < Random.Range(3, 6); y++)
                {
                    print(i);
                    difficultyStack.Push(i);
                }            
            print(1);
            difficultyStack.Push(1);
            

        }

        public void LoadNextScene()
        {
            var scene = SceneManager.GetActiveScene();
            var levelIndex = scene.buildIndex;
            if(levelIndex == 0)
                SceneManager.LoadScene(levelIndex + 1);
            else
            {
                int diffuculty = difficultyStack.Pop();
                switch (diffuculty)
                {
                    case (int)Difficulty.STARTING_POINT:
                        SceneManager.LoadScene(1);
                        break;
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
                }
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

public enum Difficulty
{
    STARTING_POINT,
    VERY_EASY,
    EASY,
    MODERATE,
    HARD,
    VERY_HARD,
    BOSS
}