using DungeonRush.Cards;
using DungeonRush.Customization;
using DungeonRush.Data;
using DungeonRush.Managers;
using DungeonRush.Property;
using System.Collections;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class AIController : MonoBehaviour, IMoveController
    {
        private Swipe swipe;
        private Card card;
        private bool isRunning = false;
        private CardType cardType;

        private ProcessHandleChecker preparingProcess;
        private ProcessHandleChecker attackProcess;
        private ProcessHandleChecker moveProcess;

        private EnemyController enemyController;
        private TrapController trapController;

        private IMover mover;
        private IAttacker attacker;
        private ICustomization customization;
        public GameObject model;

        private void Start()
        {
            card = GetComponent<Card>();
            mover = card.GetComponent<IMover>();
            attacker = card.GetComponent<IAttacker>();
            customization = card.GetComponent<ICustomization>();
            cardType = card.GetCardType();

            if (cardType == CardType.ENEMY)
            {
                enemyController = FindObjectOfType<EnemyController>();
                EnemyController.subscribedEnemies.Add(this);
            }
            else if (cardType == CardType.TRAP)
            {
                trapController = FindObjectOfType<TrapController>();
                TrapController.subscribedTraps.Add(this);
            }

            
            InitProcessHandlers();
        }

        private void Update()
        {
            if (!isRunning) return;

            if (preparingProcess.IsRunning())
            {
                PrepareMoveProcess();
            }
            else if (attackProcess.IsRunning()) 
            {
                AttackProcess();
            }
            else if (moveProcess.IsRunning())
            {
                MoveProcess();
            }
        }

        // TODO Swipe değişkeninden kurtul..

        #region PREPARE MOVE PROCESS

        public void PrepareMoveProcess()
        {
            var canMove = DoMove(swipe);
            
            if (canMove)
            {
                preparingProcess.Finish();
                var move = card.GetMove().GetCanMove();

                if (move)
                    moveProcess.StartProcess();
                else
                    attackProcess.StartProcess();
            }
            else
            {
                preparingProcess.Finish();
                Stop();
                Notify();
            }
        }

        private bool DoMove(Swipe swipe)
        {
            if (cardType == CardType.ENEMY)
                return card.GetShift().Define(card, swipe);
            else if (cardType == CardType.TRAP)
                return card.GetShift().Define(card, Swipe.NONE);
            else
                return false;
        }

        #endregion

        #region ATTACK PROCESS

        public void AttackProcess()
        {
            if (attackProcess.start)
            {
                card.ExecuteAttack();
                attackProcess.ContinuingProcess(false);
            }
            else if (attackProcess.continuing)
            {
                if (attacker.GetAttackFinished())
                {
                    attacker.SetAttackFinished(false);
                    attackProcess.EndProcess();
                }
            }
            else if (attackProcess.end)
            {
                StartCoroutine(EndTurn());
                Stop();
                attackProcess.Finish();
                Notify();
            }
        }
        #endregion

        #region MOVE PROCESS

        public void MoveProcess()
        {
            if (moveProcess.start)
            {
                card.ExecuteMove();
                moveProcess.ContinuingProcess(false);
            }
            else if (moveProcess.continuing)
            {
                
                if (mover.IsMoveFinished())
                {
                    mover.SetIsMoveFinished(false);
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.end)
            {
                StartCoroutine(EndTurn());
                Stop();
                moveProcess.Finish();
                Notify();
            }
        }

        #endregion

        #region STATE METHODS
        public void ChangeAnimatorState(bool state)
        {
            model.GetComponent<Animator>().enabled = state;
        }

        public void ChangeShadowState(bool shadow)
        {
            if (shadow)
                customization.OverShadow();
            else
                customization.RemoveShadow();
        }
        #endregion

        private IEnumerator EndTurn()
        {
            yield return new WaitForSeconds(0);
        }
        private void Notify()
        {
            if (cardType == CardType.ENEMY)
                enemyController.OnNotify();
            else if (cardType == CardType.TRAP)
                trapController.OnNotify();
        }
        private void Stop()
        {
            isRunning = false;
            swipe = Swipe.NONE;
            card.GetMove().Reset();
        }
        public void InitProcessHandlers()
        {
            preparingProcess.Init(true);
            attackProcess.Init(false);
            moveProcess.Init(false);
        }
        public void SetSwipe(Swipe s)
        {
            swipe = s;
        }
        public void SetMove(Move move)
        {
            card.SetMove(move);
        }
        public Card GetCard()
        {
            return card;
        }
        public void Run()
        {
            if (cardType == CardType.ENEMY)
                swipe = GetCard().GetShift().SelectTileToAttack(GetCard().GetShift().GetAvaibleTiles(GetCard()), GetCard());

            isRunning = true;
            preparingProcess.StartProcess();
        }
        public bool IsRunning()
        {
            return isRunning;
        }
    }
}
