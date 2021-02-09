using System;
using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class TrapAIController : MonoBehaviour, IMoveController
    {
        private Card card;
        private Attacker attacker;
        private ProcessHandleChecker preparingProcess;
        private ProcessHandleChecker attackProcess;

        private bool isRunning = false;

        private void Start()
        {
            Initialize();
            InitProcessHandlers();

            TrapManager.subscribedTraps.Add(this);
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
            card = GetComponent<Card>();
            attacker = GetComponent<Attacker>();
        }

        public Card GetCard()
        {
            return card;
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
            throw new NotImplementedException();
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
            bool canMove = DoMove();
            if (canMove)
                AttackProcess();
            else
            {
                preparingProcess.Finish();
                Stop();
                Notify();
            }
        }

        public void Notify()
        {
            MoveSchedular.Instance.trapController.OnNotify();
        }

        public void Stop()
        {
            isRunning = false;
            card.GetMove().Reset();
        }

        public void Run()
        {
            isRunning = true;
            preparingProcess.StartProcess();
        }
    }
}

