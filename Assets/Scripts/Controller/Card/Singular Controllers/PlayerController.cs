using DungeonRush.Cards;
using DungeonRush.Customization;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Items;
using DungeonRush.Managers;
using DungeonRush.Property;
using DungeonRush.Saving;
using DungeonRush.Skills;
using DungeonRush.Traits;
using System.Collections;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class PlayerController : MonoBehaviour, ICardController, IMoveController
    {
        private bool isRunning = false;
        private PlayerCard player;
        private ProcessHandleChecker preparingProcess;
        private ProcessHandleChecker attackProcess;
        private ProcessHandleChecker moveProcess;
        private Attacker attacker;
        private ICustomization customization;
        private StatusController statusController;

        private void Start()
        {
            InitProcessHandlers();
            player = GetComponent<PlayerCard>();
            attacker = player.GetComponent<Attacker>();
            customization = player.GetComponent<ICustomization>();
            statusController = player.GetComponent<StatusController>();
            MoveSchedular.Instance.playerController = this;
        }

        private void Update()
        {
            if (!isRunning) return;

            MakeMove();
        }

        public void MakeMove()
        {
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

        #region MOVE METHODS
        public void PrepareMoveProcess()
        {
            if (!Board.touched && SwipeManager.swipeDirection != Swipe.NONE)
            {
                var canMove = DoMove(SwipeManager.swipeDirection);
                if (canMove)
                {
                    preparingProcess.Finish();
                    Board.touched = true;

                    var move = player.GetMove().GetCanMove();

                    if (move)
                        moveProcess.StartProcess();
                    else
                        attackProcess.StartProcess();
                }
                else
                {
                    player.GetMove().Reset();
                }
            }
        }

        private bool DoMove(Swipe swipe)
        {
            bool b = player.GetShift().Define(player, swipe);

            return b;
        }

        public void AttackProcess()
        {
            if (attackProcess.start)
            {
                player.ExecuteAttack();
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
                Stop();
                Board.touched = false;
            }
        }

        public void MoveProcess()
        {
            if (moveProcess.start)
            {
                player.ExecuteMove();
                moveProcess.ContinuingProcess(false);
            }
            else if (moveProcess.continuing)
            {
                if (player.IsMoveFinished())
                {
                    player.SetIsMoveFinished(false);
                    moveProcess.EndProcess();
                }
            }
            else if (moveProcess.end)
            {
                Stop();
                Board.touched = false;
            }
        }

        #endregion

        #region MOVE CONTROLLER METHODS
        public void InitProcessHandlers()
        {
            preparingProcess.Init(false);
            attackProcess.Init(false);
            moveProcess.Init(false);
        }
        public bool IsRunning()
        {
            return isRunning;
        }
        public void Run()
        {
            isRunning = true;
        }
        public Card GetCard()
        {
            return player;
        }
        public void Stop() 
        {
            isRunning = false;
            preparingProcess.Finish();
            attackProcess.Finish();
            moveProcess.Finish();
            customization.Change(transform.position.y);
            Notify();
        }
        public void Begin() 
        {
            Run();
            preparingProcess.StartProcess();
        }

        public void ActivateStatuses()
        {
            statusController.ActivateStatuses();
        }

        public void StatusControl()
        {
            statusController.StatusControl();
        }

        private void Notify()
        {
            MoveSchedular.Instance.enemyController.ConfigureSurroundingCardsSkinStates();
            if (player.InstantMoveCount > 0)
            {
                Begin();
                ActivateStatuses();
                player.InstantMoveCount--;
            }
            else
            {
                StatusControl();
                player.InstantMoveCount = player.TotalMoveCount;
                MoveSchedular.Instance.OnNotify();
            }
        }
        #endregion

        #region SAVING METHODS
        public void SavePlayer()
        {
            SavingSystem.SavePlayerInstantProgress(player);
        }

        public void LoadPlayer()
        {
            PlayerData data = SavingSystem.LoadPlayerInstantProgress();

            if (data == null) return;

            player.SetMaxHealth(data.maxHealth);
            player.SetCurrentHealth(data.currentHealth);
            player.Coins = data.gold;

            // Statları da kaydet buraya.

            for (int i = 0; i < data.uniqueItemIDs.Length; i++)
            {
                Item item = ItemDB.Instance.GetItem(data.uniqueItemIDs[i]);
                player.GetComponent<ItemUser>().TakeItem(item);
            }

            for (int i = 0; i < data.uniqueSkillIDs.Length; i++)
            {
                Skill skill = ItemDB.Instance.GetSkill(data.uniqueSkillIDs[i]);
                player.GetComponent<SkillUser>().AddSkill(skill);
            }
        }

        #endregion
    }
}
