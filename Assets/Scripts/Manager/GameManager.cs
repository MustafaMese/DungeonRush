using DungeonRush.Controller;
using UnityEngine;

namespace DungeonRush
{
    namespace Managers
    {
        public class GameManager : MonoBehaviour
        {
            public MoveSchedular moveSchedular;

            public Vector3 v1;
            public Vector3 v2;

            private void Start()
            {
                Application.targetFrameRate = 45;
                moveSchedular = FindObjectOfType<MoveSchedular>();
            }

            private void Update()
            {
                var d = (v1 - v2).sqrMagnitude;
                print(d);
            }
        }
    }
}
