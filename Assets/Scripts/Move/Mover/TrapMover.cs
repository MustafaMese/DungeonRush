using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Shifting;
using UnityEngine;

namespace DungeonRush.Property
{
    public class TrapMover : MonoBehaviour, IMover
    {
        private bool isMoveFinished = false;

        [Header("Shifting Properties")]
        [SerializeField] Shift shifting = null;

        public Shift GetShift()
        {
            return shifting;
        }

        public bool IsMoveFinished()
        {
            return isMoveFinished;
        }

        public void Move()
        {
            throw new System.NotImplementedException();
        }

        public void SetIsMoveFinished(bool b)
        {
            isMoveFinished = b;
        }
    }
}
