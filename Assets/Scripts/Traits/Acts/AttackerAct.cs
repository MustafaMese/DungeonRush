using System.Collections.Generic;

namespace DungeonRush.Traits
{
    public class AttackerAct : IAct
    {
        private int extraDodgeChance;
        private int extraCriticChance;
        private bool canLifeSteal;

        public int ExtraDodgeChance { get => extraDodgeChance; }
        public int ExtraCriticChance { get => extraCriticChance; }
        public bool CanLifeSteal { get => canLifeSteal; }

        public AttackerAct()
        {
            Reset();
        }

        public void Reset()
        {
            extraCriticChance = 0;
            extraDodgeChance = 0;
            canLifeSteal = false;
        }

        public void ActControl(List<Status> list)
        {
            Status s;
            for (int i = 0; i < list.Count; i++)
            {
                s = list[i];
                if (s.StatusType == StatusType.SLOWED)
                {
                    extraDodgeChance -= s.Power;
                    extraCriticChance -= s.Power;
                }
                else if (s.StatusType == StatusType.HASTE)
                {
                    extraCriticChance += s.Power;
                    extraDodgeChance += s.Power;
                }
                else if (s.StatusType == StatusType.LIFE_STEAL)
                    canLifeSteal = true;
            }
        }
    }
}