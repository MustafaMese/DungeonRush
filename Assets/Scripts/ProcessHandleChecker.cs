using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush
{
    namespace DataPackages
    {
        public struct ProcessHandleChecker
        {
            public bool startProcess;
            public bool endProcess;

            public void Init(bool startingProcess)
            {
                if (startingProcess)
                    startProcess = true;
                else
                    startProcess = false;
                endProcess = false;
            }

            public void Reset()
            {
                startProcess = false;
                endProcess = false;
            }
        }
    }
}
