using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Effects;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "Skill/DoubleSlash", order = 1)]
    public class DoubleSlash : Skill
    {
        public int slashPower = 2;
        public float effectTime = 0.3f;
        public EffectObject slashAnimationPrefab;

        public override void Execute(Move move)
        {
            Card targetCard = move.GetTargetTile().GetCard();
            Card card = move.GetCard();
            
            if(targetCard != null)
                targetCard.DecreaseHealth(slashPower);

            var dir = GetDirection(move);
            Vector3 pos = Vector3.zero;

            if (slashAnimationPrefab.prefab == null)
            {
                pos = SetPosition(move.GetTargetTile(), dir, pos);
                slashAnimationPrefab.InitializeObject(effectTime, pos, card.transform, true);
            }
            else
            {
                pos = SetPosition(move.GetTargetTile(), dir, pos);
                slashAnimationPrefab.EnableObject(effectTime, pos);
            }
        }

        private Vector3 SetPosition(Tile targetCard, Vector3 dir, Vector3 pos)
        {
            if (dir.x != 0)
            {
                if (dir.x < 0)
                {
                    Vector3 coordinate = targetCard.transform.position;
                    pos = new Vector3(coordinate.x + 1, coordinate.y, coordinate.z);
                }
                else if (dir.x > 0)
                {
                    Vector3 coordinate = targetCard.transform.position;
                    pos = new Vector3(coordinate.x - 1, coordinate.y, coordinate.z);
                }
            }
            else if (dir.y != 0)
            {
                if (dir.y < 0)
                {
                    Vector3 coordinate = targetCard.transform.position;
                    pos = new Vector3(coordinate.x, coordinate.y + 1, coordinate.z);
                }
                else if (dir.y > 0)
                {
                    Vector3 coordinate = targetCard.transform.position;
                    pos = new Vector3(coordinate.x, coordinate.y - 1, coordinate.z);
                }
            }

            return pos;
        }

        private Vector3 GetDirection(Move move)
        {
            var heading = move.GetTargetTile().transform.position - move.GetCardTile().transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }

        public override void Initialize(Move move)
        {
            throw new System.NotImplementedException();
        }
    }
}
