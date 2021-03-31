using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class EnvironmentManager : MonoBehaviour, ICardController
    {
        private static EnvironmentManager instance;
        public static EnvironmentManager Instance 
        {
            get 
            {
                return instance; 
            }
        }
        
        [SerializeField] int activeTrapDistance = 4;

        private PlayerController playerController;
        private ProcessHandleChecker determineProcess;
        private ProcessHandleChecker assigningProcess;
        private bool moveFinished = false;
        private bool isRunning = false;
        private int trapIndex;
        private List<EnvironmentAIController> trapCards;

        public List<EnvironmentElement> elementPrefabs = new List<EnvironmentElement>();
        public List<ImpactList> impacts = new List<ImpactList>();
        public static List<EnvironmentAIController> subscribedEnvironmentCards = new List<EnvironmentAIController>();
        
        private void Awake() 
        {
            instance = this;
        }

        private void Start()
        {
            playerController = MoveSchedular.Instance.playerController;
            InitProcessHandlers();
        }

        private void Update()
        {

            if (IsRunning())
            {
                if (determineProcess.IsRunning())
                {
                    Board.touched = true;
                    DetermineActiveTraps();
                    moveFinished = true;
                }
                else if (assigningProcess.IsRunning())
                {
                    if (trapIndex < trapCards.Count)
                        MoveControllers();
                    else
                        FinishMovement();
                }
            }
        }

        #region ENVIRONMENT MENTHODS

        public EnvironmentCard GetEnvironmentCard(ElementType element)
        {
            for (var i = 0; i < elementPrefabs.Count; i++)
            {
                if(element == elementPrefabs[i].element)
                    return elementPrefabs[i].card;
            }
            return null;
        }
        
        private Impact GetImpact(ElementType element)
        {
            for (var i = 0; i < impacts.Count; i++)
            {
                if (element == impacts[i].element)
                    return impacts[i].impact;
            }
            return null;
        }

        public Impact GetImpactPrefab(ElementType changed, ElementType changer)
        {
            if ((changed == ElementType.FIRE && changer == ElementType.POISON) || (changed == ElementType.POISON && changer == ElementType.FIRE))
                return GetImpact(ElementType.FIRE);
            
            return null;
        }

        public EnvironmentCard GetElementPrefab(ElementType changed, ElementType changer)
        {
            if(changed == ElementType.GRASS && changer == ElementType.FIRE)
                return GetEnvironmentCard(ElementType.FIRE);
            else if(changer == ElementType.ELECTRICITY && changed == ElementType.WATER)
                return GetEnvironmentCard(ElementType.ELECTRICITY);

            return null;
        }

        public EnvironmentCard GetElementPrefab(Impact impact, ElementType changed, out bool delete)
        {
            ElementType changer = impact.elementType;
            delete = false;

            if ((changed == ElementType.FIRE && changer == ElementType.POISON) || (changed == ElementType.POISON && changer == ElementType.FIRE))
                delete = true;
            else if ((changed == ElementType.FIRE && changer == ElementType.WATER) || (changed == ElementType.WATER && changer == ElementType.FIRE))
                delete = true;
            else if (changed == ElementType.GRASS && changer == ElementType.FIRE)
                return GetEnvironmentCard(ElementType.FIRE);
            else if (changer == ElementType.ELECTRICITY && changed == ElementType.WATER)
                return GetEnvironmentCard(ElementType.ELECTRICITY);   

            return null;
        }

        #endregion

        #region DETERMINIG METHODS
        private void DetermineActiveTraps()
        {
            trapCards = GetActiveTraps();
            determineProcess.Finish();
            assigningProcess.StartProcess();
            if(trapCards == null || trapCards.Count <= 0)
            {
                Stop();
            }
        }
        private List<EnvironmentAIController> GetActiveTraps()
        {
            List<EnvironmentAIController> l = new List<EnvironmentAIController>();

            for (int i = 0; i < subscribedEnvironmentCards.Count; i++)
            {
                if(subscribedEnvironmentCards[i] == null)
                {
                    subscribedEnvironmentCards.Remove(subscribedEnvironmentCards[i]);
                    continue;
                }

                if ((subscribedEnvironmentCards[i].transform.position - playerController.transform.position).sqrMagnitude <= activeTrapDistance)
                    l.Add(subscribedEnvironmentCards[i]);
            }

            return l;
        }

        #endregion

        #region ASSIGNING PROCESS
        private void MoveControllers()
        {
            if (moveFinished && trapCards[trapIndex] != null)
            {
                trapCards[trapIndex].Run();
                moveFinished = false;
            }
            else if (trapCards[trapIndex] == null)
            {
                trapIndex++;
                moveFinished = true;
            }
        }

        private void FinishMovement()
        {
            Stop();
            assigningProcess.Finish();
        }

        #endregion

        #region CARD CONTROLLER METHODS

        public void Begin()
        {
            Run();
            determineProcess.StartProcess();
        }
        public void InitProcessHandlers()
        {
            determineProcess.Init(false);
            assigningProcess.Init(false);
        }
        public bool IsRunning()
        {
            return isRunning;
        }
        public void Run()
        {
            isRunning = true;
        }
        public void Stop()
        {
            isRunning = false;
            trapCards.Clear();
            trapIndex = 0;
            determineProcess.Finish();
            assigningProcess.Finish();
            Notify();
            Board.touched = false;
        }
        public void Notify()
        {
            MoveSchedular.Instance.OnNotify();
        }
        public void OnNotify()
        {
            moveFinished = true;
            trapIndex++;
        }

        #endregion

        public static void UnsubscribeCard(EnvironmentAIController controller)
        {
            subscribedEnvironmentCards.Remove(controller);
        }

        private void OnDestroy()
        {
            subscribedEnvironmentCards.Clear();
        }
    }
}
