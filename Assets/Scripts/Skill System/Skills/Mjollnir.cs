using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Property;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skill/Mjollnir")]
    public class Mjollnir : Skill
    {
        private Vector2[] directions = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0),
                                        new Vector2(-1, 1), new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1),
                                        new Vector2(2, 0), new Vector2(-2, 0), new Vector2(0, 2), new Vector2(0, -2)};

        private List<Card> targets = new List<Card>();
        private List<Vector3> targetPositions = new List<Vector3>();

        public override void Execute(Move move)
        {
            FindTargets(move);
            for (int i = 0; i < targets.Count; i++)
            {
                Card card = targets[i];
                if (card != null)
                    card.DecreaseHealth(Power);
            }
        }

        public override void PositionEffect(GameObject effect, Move move)
        {
            effect.transform.SetParent(null);

            MjollnirPositioning mPos = effect.GetComponent<MjollnirPositioning>();
            mPos.Execute(targetPositions, EffectTime);
        }

        public override Vector3 PositionTextPopup(GameObject textPopup, Move move)
        {
            throw new System.NotImplementedException();
        }

        private void FindTargets(Move move)
        {
            targets.Clear();
            targetPositions.Clear();
            int rL = Board.RowLength;
            Vector2 currentCoordinate = move.GetCardTile().GetCoordinate();
            bool isFinished = false;
            while (!isFinished)
            {

                for (int i = 0; i < directions.Length; i++)
                {
                    Vector2 direction = directions[i];

                    Vector2 target = currentCoordinate + direction;
                    if (target.y < rL && target.y >= 0 && target.x < rL && target.x >= 0)
                    {
                        Tile targetTile = Board.tilesByCoordinates[target];
                        Card targetCard = targetTile.GetCard();
                        if (targetCard != null && (targetCard.GetCardType() == CardType.ENEMY || targetCard.GetCardType() == CardType.PLAYER) &&
                                targetCard != move.GetCard() && !targets.Contains(targetCard))
                        {
                            targets.Add(targetCard);
                            targetPositions.Add(target);
                            currentCoordinate = targetTile.GetCoordinate();
                            break;
                        }
                    }

                    if (i == directions.Length - 1)
                        isFinished = true;
                }

            }
        }
    }
}