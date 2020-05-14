using UnityEngine;
using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Settings;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Controller;
using DungeonRush.Data;

namespace DungeonRush 
{
    namespace Property
    {
        public class Mover : MonoBehaviour
        {
            public bool startMoving;

            private Move move;
            private Vector3 direction;

            private void Start()
            {
                DOTween.Init();
                move = new Move();
            }

            public void Move()
            {
                if (move == null || move.GetCard() == null )
                    direction = GetDirection();

                move.GetCard().transform.DOMove(move.GetTargetTile().transform.position, 0.2f).OnComplete(() => TerminateMove());
            }

            private Vector3 GetDirection()
            {
                move = GetComponent<Card>().GetMove();
                Vector3 pos = Geometry.PointFromGrid(move.GetTargetTile().GetCoordinate());
                Vector3 cardPos = Geometry.PointFromGrid(move.GetCard().GetTile().GetCoordinate());
                var heading = pos - cardPos;
                var distance = heading.magnitude;
                var direction = heading / distance;
                return direction;
            }
            public void TerminateMove()
            {
                move.GetCard().isMoving = false;
                //startMoving = false;
                move.GetCard().transform.position = move.GetTargetTile().transform.position;
                MoveType moveType = move.GetMoveType();
                if (move.GetCard().GetCardType() == CardType.PLAYER)
                {
                    PlayerCard card = FindObjectOfType<PlayerCard>();
                    if(moveType != MoveType.EMPTY)
                    {
                        if(moveType == MoveType.ATTACK)
                        {
                            Card enemy = move.GetTargetTile().GetCard();
                            card.GetComponent<Attacker>().Attack(enemy);
                        }
                        else if(moveType == MoveType.ITEM)
                        {
                            Card item = move.GetTargetTile().GetCard();
                            if (item.GetItemType() == ItemType.POTION)
                                card.GetComponent<ItemUser>().TakePotion(item);
                            else if (item.GetItemType() == ItemType.WEAPON)
                                card.GetComponent<ItemUser>().TakeWeapon(item);

                        }
                        else if(moveType == MoveType.COIN)
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

                    Board.touched = false;
                    GetComponent<PlayerController>().moveFinished = true;
                }
                else
                {
                    Card moverCard = move.GetCard();
                    Card targetCard = move.GetTargetTile().GetCard();
                    if(moveType == MoveType.ATTACK && moverCard.GetComponent<Attacker>())
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
                    else if (moveType == MoveType.ITEM && moverCard.GetComponent<ItemUser>())
                    {
                        if (targetCard.GetItemType() == ItemType.POTION)
                            moverCard.GetComponent<ItemUser>().TakePotion(targetCard);
                        else if (targetCard.GetItemType() == ItemType.WEAPON)
                            moverCard.GetComponent<ItemUser>().TakeWeapon(targetCard);
                    }

                    if (move.GetMoveType() != MoveType.EMPTY)
                        Tile.ChangeTile(move, false, false);
                    else
                        Tile.ChangeTile(move, true, false);

                    if (move.GetLastMove())
                    {
                        Board.touched = false;
                        MoveMaker.movesFinished = true;
                    }
                }

                move.Reset();
            }
        }
    }
}

//private void Update()
//{
//    if (startMoving)
//    {
//        if(move.type == MoveType.None && move.GetCard() == null)
//            direction = GetDirection();

//        move.GetCard().transform.DOMove(move.GetTargetTile().transform.position, 1f);
//        move.GetCard().transform.Translate(direction * MoveMaker.speed * Time.deltaTime);
//        if (GetComponent<Card>().isMoving)
//        {
//            if ((direction == Vector3.down || direction == Vector3.right)
//                        && Geometry.GridFromPoint(move.GetCard().transform.position) == move.GetTargetTile().GetCoordinate())
//                TerminateMove();
//            else if ((direction == Vector3.left) && (Geometry.GridFromPointFixedLeft(move.GetCard().transform.position) == move.GetTargetTile().GetCoordinate()
//                        || Geometry.GridFromPointFixedLeft(move.GetCard().transform.position).x <= move.GetTargetTile().GetCoordinate().x))
//                TerminateMove();
//            else if ((direction == Vector3.up) && (Geometry.GridFromPointFixedTop(move.GetCard().transform.position) == move.GetTargetTile().GetCoordinate()
//                        || Geometry.GridFromPointFixedTop(move.GetCard().transform.position).y <= move.GetTargetTile().GetCoordinate().y))
//                TerminateMove();
//        }
//    }
//}
