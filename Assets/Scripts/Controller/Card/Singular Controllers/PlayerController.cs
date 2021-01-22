using DungeonRush.Camera;
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
        [SerializeField] FieldOfView fieldOfView;
        private void Start()
        {
            InitProcessHandlers();
            player = GetComponent<PlayerCard>();
            attacker = player.GetComponent<Attacker>();
            customization = player.GetComponent<ICustomization>();
            statusController = player.GetComponent<StatusController>();
            customization.ChangeLayer(transform.position.y);
            MoveSchedular.Instance.playerController = this;
            fieldOfView = Instantiate(fieldOfView);
            fieldOfView.SetOrigin(transform.position);
        }

        private void Update()
        {
            if (!isRunning) return;
            fieldOfView.SetOrigin(transform.position);
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

                    float y = player.GetMove().GetTargetTile().GetCoordinate().y;
                    if(y < player.transform.position.y)
                        customization.ChangeLayer(y);

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
            customization.ChangeLayer(transform.position.y);
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

        private void Notify()
        {
            MoveSchedular.Instance.enemyController.ConfigureSurroundingCardsSkinStates();
            PlayerCamera.Instance.MoveCamera(transform.position);
            if (player.InstantMoveCount > 0)
            {
                Begin();
                ActivateStatuses();
                player.InstantMoveCount--;
            }
            else
            {
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
            player.GetComponent<Health>().InitializeBar();
            player.Coins = data.gold;
            player.Experience = data.xp;

            player.CriticChance = data.criticChance;
            player.DodgeChance = data.dodgeChance;
            player.LifeCount = data.lifeCount;
            player.TotalMoveCount = data.moveCount;
            player.LootChance = data.lootChance;

            for (int i = 0; i < data.uniqueItemIDs.Length; i++)
            {
                Item item = ItemDB.Instance.GetItem(data.uniqueItemIDs[i]);
                player.GetComponent<ItemUser>().TakeItem(item, false);
            }

            for (int i = 0; i < data.uniqueSkillIDs.Length; i++)
            {
                SkillObject skillObject = ItemDB.Instance.GetSkill(data.uniqueSkillIDs[i]);
                player.GetComponent<SkillUser>().AddSkill(skillObject, false);
            }
        }

        #endregion
    }
}
