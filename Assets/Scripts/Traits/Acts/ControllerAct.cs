using System.Collections.Generic;
using DungeonRush.Traits;

public class ControllerAct : IAct
{
    private bool canMove;
    private bool canAttack;
    private bool anger;

    public bool CanMove { get => canMove; }
    public bool CanAttack { get => canAttack; }
    public bool Anger { get => anger; }

    public ControllerAct()
    {
        Reset();
    }

    public void Reset()
    {
        canMove = true;
        canAttack = true;
        anger = false;
    }

    public void ActControl(List<StatusData> list)
    {
        Status s;
        for (int i = 0; i < list.Count; i++)
        {
            s = list[i].status;

            if (s.StatusType == StatusType.DISARMED)
                canAttack = false;
            else if (s.StatusType == StatusType.ENTANGLED)
                canMove = false;
            else if (s.StatusType == StatusType.STUNNED)
            {
                canMove = false;
                canAttack = false;
            }
            else if (s.StatusType == StatusType.ANGER)
                anger = true;
        }
    }

}