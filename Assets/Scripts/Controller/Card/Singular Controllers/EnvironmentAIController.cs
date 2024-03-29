﻿using System;
using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Managers;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class EnvironmentAIController : MonoBehaviour, IMoveController
    {
        private EnvironmentCard card;
        private Attacker attacker;
        private ProcessHandleChecker preparingProcess;
        private ProcessHandleChecker attackProcess;
        private bool isRunning = false;

        private void Start()
        {
            Initialize();
            InitProcessHandlers();

            EnvironmentManager.subscribedEnvironmentCards.Add(this);
        }

        private void InitProcessHandlers()
        {
            attackProcess.Init(false);
            preparingProcess.Init(false);
        }

        private void Update() 
        {
            if(!IsRunning()) return;    

            MakeMove();
        }

        private void Initialize()
        {
            card = GetComponent<EnvironmentCard>();
            attacker = GetComponent<Attacker>();
        }

        public Sprite GetCardIcon()
        {
            return card.GetCharacterIcon();
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public void MakeMove()
        {
            if(preparingProcess.IsRunning())
                PrepareMoveProcess();
            else if(attackProcess.IsRunning())
                AttackProcess();
        }

        private bool DoMove()
        {
            return card.GetAttackStyle().Define(card, Swipe.NONE);
        }

        private void AttackProcess()
        {
            if(attackProcess.start)
            {
                card.ExecuteAttack();
                attackProcess.ContinuingProcess(false);
            }
            else if(attackProcess.continuing)
            {
                if(attacker.GetAttackFinished())
                {
                    attacker.SetAttackFinished(false);
                    attackProcess.Finish();
                    
                    Stop();
                    Notify();
                }
            }
        }

        private void PrepareMoveProcess()
        {
            if(CheckDefinity()) return;

            if (card.GetElementType() != ElementType.NONE)
                EvolveOthers();

            if(IsAttackerNull()) return;

            Move();
        }

        private bool CheckDefinity()
        {
            bool check = card.CheckTime();
            if (!check)
            {
                Notify();
                card.Remove(card);
                if (card.GetElementType() == ElementType.FIRE)
                {
                    var newCard = EnvironmentManager.Instance.GetEnvironmentCard(ElementType.BURNED);
                    CardManager.Instance.AddCard(newCard, card.GetTile());
                }
                return true;
            }
            return false;
        }

        private void Move()
        {
            bool canMove = DoMove();
            preparingProcess.Finish();

            if (canMove)
            {
                attackProcess.StartProcess();
                AttackProcess();
            }
            else
            {
                Stop();
                Notify();
            }
        }

        private bool IsAttackerNull()
        {
            if (attacker == null)
            {
                Stop();
                Notify();
                return true;
            }
            return false;
        }

        private void EvolveOthers()
        {
            List<EnvironmentCard> cards = card.CheckOtherEnvironmentCards();
            for (var i = 0; i < cards.Count; i++)
                card.EvolveIt(cards[i]);
        }

        public void Notify()
        {
            MoveSchedular.Instance.environmentController.OnNotify();
        }

        public void Stop()
        {
            isRunning = false;
            //card.GetMove().Reset();
        }

        public void Run()
        {
            isRunning = true;
            preparingProcess.StartProcess();
        }
    }
}

