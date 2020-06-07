using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class MoveSchedular : MonoBehaviour
    {
        public bool notify = false;

        /// <summary>
        /// -1 for Nothing, 0 for Player, 1 for NonPlayers, 2 for Adding
        /// </summary>
        public int turn;
        public int oldTurn;

        public PlayerController pc;
        public NonPlayerController npc;

        private void Start()
        {
            pc = FindObjectOfType<PlayerController>();
            npc = FindObjectOfType<NonPlayerController>();

            turn = 0;
            oldTurn = -1;
        }

        private void Update()
        {
            if (pc == null) 
            { 
                pc = FindObjectOfType<PlayerController>();
                pc.Begin();
            }

            if (notify) 
            {
                OnNotify();
                notify = false;
            }
        }

        public void OnNotify() 
        {
            if(turn != 2) 
            {
                oldTurn = turn;
                turn = 2;
            }
            else
            {
                if (oldTurn == 0)
                    turn = 1;
                else
                    turn = 0;

                oldTurn = -1;
            }

            CardManager.Instance.ReshuffleCards();
            if (turn == -1) 
            {
                // Nothing
            }
            else if (turn == 0)
            {
                pc.Begin();
            }
            else if (turn == 1)
            {
                npc.Begin();
            }
            else if (turn == 2)
            {
                
            }
        }
    }
}
