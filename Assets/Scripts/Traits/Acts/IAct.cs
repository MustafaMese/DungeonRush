﻿using System.Collections;
using System.Collections.Generic;

namespace DungeonRush.Traits
{
    public interface IAct
    {
        void Reset();
        void ActControl(List<Status> list);
    }
}