using System.Collections.Generic;
using DungeonRush.Traits;

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

    public void ActControl(List<StatusData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].status.StatusType == StatusType.ACID)
                isAcid = true;
        }
    }
}