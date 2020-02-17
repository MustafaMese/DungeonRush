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

            public void StartProcess()
            {
                endProcess = false;
                startProcess = true;                
            }

            public void FinishProcess()
            {
                endProcess = true;
                startProcess = false;
            }

            public void Reset()
            {
                startProcess = false;
                endProcess = false;
            }
        }
    }
}
