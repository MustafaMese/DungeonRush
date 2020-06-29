﻿using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class MoveSchedular : MonoBehaviour
    {
        public TextMeshProUGUI tourText;

        [SerializeField] bool notify = false;
        /// <summary>
        /// -1 for Nothing, 0 for Player, 1 for NonPlayers, 2 for Adding
        /// </summary>
        [SerializeField] int turnNumber;
        [SerializeField] int oldTurnNumber;
        [SerializeField] int tourCount;

        public PlayerController pc;
        public NonPlayerController npc;

        private void Start()
        {
            pc = FindObjectOfType<PlayerController>();
            npc = FindObjectOfType<NonPlayerController>();

            tourCount = 0;
            tourText.text = tourCount.ToString();
            turnNumber = 0;
            oldTurnNumber = -1;

        }

        private void Update()
        {
            if(pc == null) 
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

        public void IncreaseTour() 
        {
            tourCount++;
            tourText.text = tourCount.ToString();
        }

        public void OnNotify() 
        {
            if(turnNumber != 2)
            {
                oldTurnNumber = turnNumber;
                turnNumber = 2;

                if (oldTurnNumber == 0)
                    IncreaseTour();
            }
            else
            {
                if (oldTurnNumber == 0)
                    turnNumber = 1;
                else
                    turnNumber = 0;

                oldTurnNumber = -1;
            }

            CardManager.Instance.ReshuffleCards();
            if (turnNumber == -1) 
            {
                // Nothing
            }
            else if (turnNumber == 0)
            {
                pc.Begin();
            }
            else if (turnNumber == 1)
            {
                npc.Begin();
            }
            else if (turnNumber == 2)
            {
                
            }
        }
    }
}