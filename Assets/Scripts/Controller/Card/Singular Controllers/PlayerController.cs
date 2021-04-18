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
using TMPro;
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
        private SkillUser skillUser;
        
        [SerializeField] PlayerData data;
        [SerializeField] FieldOfView fieldOfView;
        
        private void Start()
        {
            InitProcessHandlers();
            player = GetComponent<PlayerCard>();
            attacker = player.GetComponent<Attacker>();
            customization = player.GetComponent<ICustomization>();
            statusController = player.GetComponent<StatusController>();
            skillUser = player.GetComponent<SkillUser>();

            if(transform.position.y > 0)
                customization.ChangeLayer(false, (int)transform.position.y);
            else if (transform.position.y < 0)
                customization.ChangeLayer(true, (int)transform.position.y);
           
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
                    Stop();
                    Board.touched = false;
                }
            }
        }

        public void MoveProcess()
        {
            if (moveProcess.start)
            {
                player.ExecuteMove();

                float y = player.GetMove().GetTargetTile().GetCoordinate().y;

                if(player.GetMove().GetMoveType() != MoveType.EVENT)
                {
                    if (y < player.transform.position.y)
                        customization.ChangeLayer(true);
                    else if (y > player.transform.position.y)
                        customization.ChangeLayer(false);
                }
                
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
        public Sprite GetCardIcon()
        {
            return player.GetCharacterIcon();
        }
        public void Stop() 
        {
            isRunning = false;
            preparingProcess.Finish();
            attackProcess.Finish();
            moveProcess.Finish();
            //customization.ChangeLayer(transform.position.y);
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

        public void Notify()
        {
            MoveSchedular.Instance.enemyController.ConfigureSurroundingCardsSkinStates();
            PlayerCamera.Instance.MoveCamera(transform.position);
            if (player.GetStats().InstantMoveCount > 0)
            {
                Begin();
                ActivateStatuses();
                player.GetStats().InstantMoveCount--;
            }
            else
            {
                player.GetStats().InstantMoveCount = player.GetStats().TotalMoveCount;
                MoveSchedular.Instance.OnNotify();
            }
        }
        #endregion

        #region SAVING METHODS
        public void SavePlayer()
        {
            //SavingSystem.SavePlayerInstantProgress(player);
            data.Save(player);
        }

        public void LoadPlayer()
        {
            data.Load(player);
            // PlayerData data = SavingSystem.LoadPlayerInstantProgress();

            // if (data == null) 
            // {
            //     text += true;
            //     return;
            // }

            // player.SetMaxHealth(data.maxHealth);
            // player.SetCurrentHealth(data.currentHealth);
            // player.GetComponent<Health>().InitializeBar();
            // player.Gold = data.gold;
            // player.Experience = data.xp;

            // player.GetStats().CriticChance = data.criticChance;
            // player.GetStats().DodgeChance = data.dodgeChance;
            // player.GetStats().LifeCount = data.lifeCount;
            // player.GetStats().TotalMoveCount = data.moveCount;
            // player.GetStats().LootChance = data.lootChance;

            // for (int i = 0; i < data.uniqueItemIDs.Length; i++)
            // {
            //     Item item = ItemDB.Instance.GetItem(data.uniqueItemIDs[i]);
            //     player.GetComponent<ItemUser>().TakeItem(item, false);
            // }

            // for (int i = 0; i < data.uniqueSkillIDs.Length; i++)
            // {
            //     SkillObject skillObject = ItemDB.Instance.GetSkill(data.uniqueSkillIDs[i]);
            //     player.GetComponent<SkillUser>().AddSkill(skillObject, false);
            // }
        }

        #endregion
    }
}
