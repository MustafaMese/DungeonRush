using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Attack/LongRangeAttack")]
public class LongRangeAttacking : AttackStyle
{
    private Vector2[] directions = { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1),
                                            new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1) };

    [SerializeField] int range = 0;

    public override void Attack(Move move, int damage)
    {
        Card targetCard = move.GetTargetTile().GetCard();
        if (targetCard != null && targetCard.GetCardType() != CardType.WALL)
            targetCard.DecreaseHealth(damage);
    }

    public override void SetEffectPosition(GameObject effect, Vector3 tPos, Transform card = null)
    {
        effect.transform.position = tPos;
        effect.transform.SetParent(card);
    }

    public override bool Define(Card card, Swipe swipe)
    {
        Vector2 direction = Vector2.zero;

        switch (swipe)
        {
            case Swipe.NONE:
                return false;
            case Swipe.UP:
                direction = new Vector2(0, 1);
                break;
            case Swipe.DOWN:
                direction = new Vector2(0, -1);
                break;
            case Swipe.LEFT:
                direction = new Vector2(-1, 0);
                break;
            case Swipe.RIGHT:
                direction = new Vector2(1, 0);
                break;
            case Swipe.UP_RIGHT:
                direction = new Vector2(1, 1);
                break;
            case Swipe.UP_LEFT:
                direction = new Vector2(-1, 1);
                break;
            case Swipe.DOWN_RIGHT:
                direction = new Vector2(1, -1);
                break;
            case Swipe.DOWN_LEFT:
                direction = new Vector2(-1, -1);
                break;
            default:
                break;
        }

        int rL = Board.RowLength;
        Vector2 coordinate = card.GetTile().GetCoordinate();
        Vector2 targetCoordinate;
        for (int i = 0; i < range; i++)
        {
            targetCoordinate = coordinate + direction * (i + 1);
            if (targetCoordinate.y < rL && targetCoordinate.y >= 0 && targetCoordinate.x < rL && targetCoordinate.x >= 0)
            {
                Tile targetTile = Board.tilesByCoordinates[targetCoordinate];
                if (targetTile.GetCard() != null)
                {
                    if (targetTile.GetCard().GetCardType() == CardType.WALL)
                        continue;
                    ConfigureCardMove(card, targetTile);
                    Debug.Log("5");
                    return true;
                }
            }
        }

        return false;
    }

    public override Dictionary<Tile, Swipe> GetAvaibleTiles(Card card)
    {
        if (card == null) return null;

        int rL = Board.RowLength;
        Vector2 coordinate = card.GetTile().GetCoordinate();
        Dictionary<Tile, Swipe> availableTiles = new Dictionary<Tile, Swipe>();

        foreach (var direction in directions)
        {
            Vector2 targetCoordinate;
            for (int i = 0; i < range; i++)
            {
                targetCoordinate = coordinate + direction * (i + 1);
                
                if(targetCoordinate.y < rL && targetCoordinate.y >= 0 && targetCoordinate.x < rL && targetCoordinate.x >= 0)
                {
                    var tile = Board.tilesByCoordinates[targetCoordinate];
                    if (tile != null && tile.GetCard() != null && 
                            (tile.GetCard().GetCardType() == CardType.ENEMY || tile.GetCard().GetCardType() == CardType.PLAYER))
                    {
                        availableTiles.Add(tile, FindSwipeByDirection(direction));
                        break;
                    }
                }
            }
        }

        return availableTiles;
    }

    private Swipe FindSwipeByDirection(Vector2 direction)
    {
        if (direction == new Vector2(1, 0))
            return Swipe.RIGHT;
        else if (direction == new Vector2(-1, 0))
            return Swipe.LEFT;
        else if (direction == new Vector2(0, 1))
            return Swipe.UP;
        else if (direction == new Vector2(0, -1))
            return Swipe.DOWN;
        else if (direction == new Vector2(1, 1))
            return Swipe.UP_RIGHT;
        else if (direction == new Vector2(1, -1))
            return Swipe.DOWN_RIGHT;
        else if (direction == new Vector2(-1, 1))
            return Swipe.UP_LEFT;
        else if (direction == new Vector2(-1, -1))
            return Swipe.DOWN_LEFT;
        else
            return Swipe.NONE;
    }
}
