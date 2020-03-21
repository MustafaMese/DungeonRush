using DungeonRush.Cards;
using DungeonRush.Element;
using DungeonRush.Managers;
using DungeonRush.Property;
using DungeonRush.Settings;
using UnityEngine;

namespace DungeonRush 
{
    namespace Moves
    {
        public class Mover : MonoBehaviour
        {
            public bool startMoving;

            private Move move;
            private Vector3 direction;

            private void Update()
            {
                if (startMoving)
                {
                    if(move.type == MoveType.None && move.GetCard() == null)
                        direction = GetDirection();
                    move.GetCard().transform.Translate(direction * MoveMaker.speed * Time.deltaTime);
                    if (GetComponent<Card>().isMoving)
                    {
                        if ((direction == Vector3.down || direction == Vector3.right)
                                    && Geometry.GridFromPoint(move.GetCard().transform.position) == move.GetTargetTile().GetCoordinate())
                            TerminateMove();
                        else if ((direction == Vector3.left) && (Geometry.GridFromPointFixedLeft(move.GetCard().transform.position) == move.GetTargetTile().GetCoordinate()
                                    || Geometry.GridFromPointFixedLeft(move.GetCard().transform.position).x <= move.GetTargetTile().GetCoordinate().x))
                            TerminateMove();
                        else if ((direction == Vector3.up) && (Geometry.GridFromPointFixedTop(move.GetCard().transform.position) == move.GetTargetTile().GetCoordinate()
                                    || Geometry.GridFromPointFixedTop(move.GetCard().transform.position).y <= move.GetTargetTile().GetCoordinate().y))
                            TerminateMove();
                    }
                }
            }

            private Vector3 GetDirection()
            {
                move = GetComponent<Card>().GetMove().TranferMoveInfoToAnotherMove(move);
                Vector3 pos = Geometry.PointFromGrid(move.GetTargetTile().GetCoordinate());
                Vector3 cardPos = Geometry.PointFromGrid(move.GetCardTile().GetCoordinate());
                var heading = pos - cardPos;
                var distance = heading.magnitude;
                var direction = heading / distance;
                return direction;
            }

            // TODO Burasıyla oyna......
            public void TerminateMove()
            {
                move.GetCard().isMoving = false;
                startMoving = false;
                move.GetCard().transform.position = move.GetTargetTile().transform.position;
                MoveType moveType = move.GetMoveType();
                if (move.GetCard().GetCardType() == CardType.PLAYER)
                {
                    PlayerCard card = GameManager.GetCardManager().GetPlayerCard();
                    if(moveType != MoveType.Empty)
                    {
                        if(moveType == MoveType.Attack)
                        {
                            Card enemy = move.GetTargetTile().GetCard();
                            card.GetComponent<Attacker>().Attack(enemy);
                        }
                        else if(moveType == MoveType.Item)
                        {
                            Card item = move.GetTargetTile().GetCard();
                            if (item.GetItemType() == ItemType.POTION)
                                card.GetComponent<ItemUser>().TakePotion(item);
                            else if (item.GetItemType() == ItemType.WEAPON)
                                card.GetComponent<ItemUser>().TakeWeapon(item);

                        }
                        else if(moveType == MoveType.Coin)
                        {
                            Card coin = move.GetTargetTile().GetCard();
                            FindObjectOfType<CoinCounter>().IncreaseCoin(coin.GetHealth());
                        }
                        Tile.ChangeTile(move, false, true);
                    }
                    else
                    {
                        Tile.ChangeTile(move, false, true);
                    }
                }
                else
                {
                    Card moverCard = move.GetCard();
                    Card targetCard = move.GetTargetTile().GetCard();
                    if(moveType == MoveType.Attack && moverCard.GetComponent<Attacker>())
                    {
                        // TODO Player'a zırh eklemede burası kullanılabilinir.
                        if(targetCard.GetCardType() == CardType.PLAYER)
                        {
                            moverCard.GetComponent<Attacker>().Attack(targetCard);
                        }
                        else
                        {
                            moverCard.GetComponent<Attacker>().Attack(targetCard);
                        }
                    }
                    else if (moveType == MoveType.Item && moverCard.GetComponent<ItemUser>())
                    {
                        if (targetCard.GetItemType() == ItemType.POTION)
                            moverCard.GetComponent<ItemUser>().TakePotion(targetCard);
                        else if (targetCard.GetItemType() == ItemType.WEAPON)
                            moverCard.GetComponent<ItemUser>().TakeWeapon(targetCard);
                    }

                    if (move.GetMoveType() != MoveType.Empty)
                        Tile.ChangeTile(move, false, false);
                    else
                        Tile.ChangeTile(move, true, false);
                }

                if (move.GetLastMove())
                {
                    Board.touched = false;
                    MoveMaker.movesFinished = true;
                }
                move.Reset();
            }
        }
    }
}
