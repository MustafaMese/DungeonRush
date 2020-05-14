using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class NonPlayerController : MonoBehaviour, ICardController
    {
        public ProcessHandleChecker determineProcess;
        public ProcessHandleChecker decideProcess;
        public ProcessHandleChecker assigningProcess;

        public List<AIController> aiControllers;
        public List<Card> highLevelCards;
        public List<Card> attackers;

        bool isRunning = false;
        bool finishTurn = false;
        public bool FinishTurn { get => finishTurn; set => finishTurn = value; }

        public int attackerIndex = 0;
        PlayerController playerController;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        // TODO Buraları düzelt en son assign de kalıyodu.
        private void Update()
        {
            if (IsRunning())
            {
                if (determineProcess.IsRunning())
                {
                    highLevelCards = GetHighLevelCards();
                    if (highLevelCards != null && highLevelCards.Count > 0)
                    {
                        decideProcess.StartProcess();
                    }
                    else
                    {
                        MigrateTurn();
                    }
                    determineProcess.Finish();
                }
                else if (decideProcess.IsRunning())
                {
                    attackers = DecideAttackerEnemies();
                    if (attackers != null && attackers.Count > 0)
                    {
                        assigningProcess.StartProcess();
                    }
                    else
                    {
                        MigrateTurn();
                    }
                    decideProcess.Finish();
                }
                else if (assigningProcess.IsRunning())
                {
                    print("1");
                    if (assigningProcess.start)
                    {
                        print("2");
                        // TODO iki yüksek seviyeli birbirine saldırırsa hata alıyo.
                        if (attackers[attackerIndex] == null)
                        {
                            print("3");
                            assigningProcess.EndProcess();
                            return;
                        }

                        attackers[attackerIndex].GetComponent<AIController>().Run();
                        assigningProcess.ContinuingProcess(false);
                    }
                    else if (assigningProcess.continuing)
                    {
                        print("4");
                        if (finishTurn)
                        {
                            print("5");
                            assigningProcess.EndProcess();
                            finishTurn = false;
                        }
                    }
                    else if (assigningProcess.end)
                    {
                        print("6");
                        attackerIndex++;
                        if (attackers.Count > attackerIndex)
                        {
                            print("7");
                            assigningProcess.StartProcess();
                        }
                        else
                        {
                            print("8");
                            assigningProcess.EndProcess();
                            MigrateTurn();
                            print("bitti");
                        }
                    }
                }
            }
        }

        private void MigrateTurn()
        {
            isRunning = false;
            highLevelCards.Clear();
            attackers.Clear();
            assigningProcess.Finish();
            playerController.Run();
            attackerIndex = 0;
        }

        private IEnumerator Islem(Card card)
        {
            print(card.GetTile() + "'in İlerlemesi başladı.");
            assigningProcess.ContinuingProcess(false);
            yield return new WaitForSeconds(1f);
            print(card.GetTile() + "'in İlerlemesi bitti");
            assigningProcess.EndProcess();
        }

        private List<Card> DecideAttackerEnemies()
        {
            List<Card> cards = new List<Card>();
            var attackerCount = highLevelCards.Count % 4;
            for (int i = 0; i < attackerCount; i++)
            {
                var card = highLevelCards[i];
                if (!cards.Contains(card))
                    cards.Add(card);
            }
            return cards;
        }

        private List<Card> GetHighLevelCards()
        {
            return CardManager.Instance.GetHighLevelCards();
        }

        public void Run()
        {
            isRunning = true;
            determineProcess.StartProcess();
        }

        public void InitProcessHandlers()
        {
            throw new System.NotImplementedException();
        }

        public bool IsRunning()
        {
            return isRunning;
        }
    }
}
