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
            public bool moveFinished = false;

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
                Vector3 cardPos = Geometry.PointFromGrid(move.GetCardTile().GetCoordinate());
                var heading = pos - cardPos;
                var distance = heading.magnitude;
                var direction = heading / distance;
                return direction;
            }
            public void TerminateMove()
            {
                move.GetCard().isMoving = false;
                move.GetCard().transform.position = move.GetTargetTile().transform.position;
                MoveType moveType = move.GetMoveType();
                if (move.GetCard().GetCardType() == CardType.PLAYER)
                {
                    PlayerCard card = FindObjectOfType<PlayerCard>();
                    PlayerMoveTypes(moveType, card);

                    // TODO Board.touched sadece controllerlar da kullanılacak.
                    Board.touched = false;
                }
                else
                {
                    Card moverCard = move.GetCard();
                    Card targetCard = move.GetTargetTile().GetCard();
                    NonPlayerMoveTypes(moveType, moverCard, targetCard);

                    Board.touched = false;
                    MoveMaker.movesFinished = true;
                }

                moveFinished = true;
                move.Reset();
            }

            private void PlayerMoveTypes(MoveType moveType, PlayerCard card)
            {
                switch (moveType)
                {
                    case MoveType.NONE:
                        break;
                    case MoveType.ATTACK:
                        Card enemy = move.GetTargetTile().GetCard();
                        card.GetComponent<Attacker>().Attack(enemy);
                        Tile.ChangeTile(move, false, true);
                        break;
                    case MoveType.ITEM:
                        Card item = move.GetTargetTile().GetCard();
                        if (item.GetItemType() == ItemType.POTION)
                            card.GetComponent<ItemUser>().TakePotion(item);
                        else if (item.GetItemType() == ItemType.WEAPON)
                            card.GetComponent<ItemUser>().TakeWeapon(item);
                        Tile.ChangeTile(move, false, true);
                        break;
                    case MoveType.COIN:
                        Card coin = move.GetTargetTile().GetCard();
                        FindObjectOfType<CoinCounter>().IncreaseCoin(coin.GetHealth());
                        Tile.ChangeTile(move, false, true);
                        break;
                    case MoveType.EMPTY:
                        Tile.ChangeTile(move, true, true);
                        break;
                    default:
                        break;
                }
            }

            private void NonPlayerMoveTypes(MoveType moveType, Card moverCard, Card targetCard)
            {
                switch (moveType)
                {
                    case MoveType.NONE:
                        break;
                    case MoveType.ATTACK:
                        if (moverCard.GetComponent<Attacker>())
                            moverCard.GetComponent<Attacker>().Attack(targetCard);
                        Tile.ChangeTile(move, false, false);
                        break;
                    case MoveType.ITEM:
                        if (moverCard.GetComponent<ItemUser>())
                        {
                            if (targetCard.GetItemType() == ItemType.POTION)
                                moverCard.GetComponent<ItemUser>().TakePotion(targetCard);
                            else if (targetCard.GetItemType() == ItemType.WEAPON)
                                moverCard.GetComponent<ItemUser>().TakeWeapon(targetCard);
                        }
                        Tile.ChangeTile(move, false, false);
                        break;
                    case MoveType.COIN:
                        break;
                    case MoveType.EMPTY:
                        Tile.ChangeTile(move, true, false);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

