using System.Collections;
using System.Collections.Generic;
using DungeonRush.Traits;

public interface IAct
{
    void Reset();
    void ActControl(List<StatusData> list);
}
