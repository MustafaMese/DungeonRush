using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DungeonRush
{
    namespace Data
    {
        [Serializable]
        public struct ProcessHandleChecker
        {
            public bool run;
            public bool start;
            public bool continuing;
            public bool end;

            public void Init(bool startingProcess)
            {
                run = startingProcess;
                start = false;
                continuing = false;
                end = false;
            }

            public void StartProcess()
            {
                run = true;
                start = true;
                continuing = true;
                end = false;
            }

            public void ContinuingProcess(bool isFinishing)
            {
                start = false;
                continuing = true;
                end = isFinishing;
            }

            public void EndProcess()
            {
                start = false;
                continuing = false;
                end = true;
            }

            public void Finish()
            {
                run = false;
                start = false;
                continuing = false;
                end = false;
            }

            public bool IsRunning() 
            {
                return run;
            }
        }
    }
}
