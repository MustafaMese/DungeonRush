using System.Collections.Generic;

namespace DungeonRush.Traits
{
    public class HealthAct : IAct
    {
        public bool isAcid;
        public bool IsAcid { get => isAcid; }

        public HealthAct()
        {
            Reset();
        }

        public void Reset()
        {
            isAcid = false;
        }

        public void ActControl(List<Status> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].StatusType == StatusType.ACID)
                    isAcid = true;
            }
        }
    }
}