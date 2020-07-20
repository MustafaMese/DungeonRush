using DungeonRush.Controller;
using UnityEngine;

namespace DungeonRush
{
    namespace Managers
    {
        public class GameManager : MonoBehaviour
        {
            public MoveSchedular moveSchedular;

            private void Start()
            {
                Application.targetFrameRate = 60;
                moveSchedular = FindObjectOfType<MoveSchedular>();
            }
        }
    }
}
