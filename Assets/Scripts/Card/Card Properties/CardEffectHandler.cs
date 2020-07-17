using DungeonRush.Field;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Property
{
    public class CardEffectHandler : MonoBehaviour
    {
        enum Directions { Top = 270, Down = 90, Right = 180, Left = 0 }
        const float circleAngle = 360;
        private Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void DoAnim(MoveType moveType, Tile targetTile, int listNumber)
        {
            switch (moveType)
            {
                case MoveType.ATTACK:
                    PlaceAnimationGameObject(targetTile, moveType, listNumber);
                    anim.SetTrigger("attack");
                    break;
                case MoveType.ITEM:
                    PlaceAnimationGameObject(targetTile);
                    //if (targetTile.GetCard().GetItemType() == ItemType.POTION)
                    //    anim.SetTrigger("health");
                    break;
                case MoveType.COIN:
                    PlaceAnimationGameObject(targetTile);
                    anim.SetTrigger("coin");
                    break;
                default:
                    break;
            }
        }

        private void PlaceAnimationGameObject(Tile t)
        {
            anim.transform.position = t.transform.position;
            anim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        private void PlaceAnimationGameObject(Tile t, MoveType type, int listNumber)
        {
            anim.transform.position = t.transform.position;
            float rot = anim.transform.rotation.z;
            if (type == MoveType.ATTACK)
            {
                int tListNumber = t.GetListNumber();
                if (tListNumber + 1 == listNumber)
                    rot = RotateAngle(rot, Directions.Right);
                else if (tListNumber + 4 == listNumber)
                    rot = RotateAngle(rot, Directions.Down);
                else if (tListNumber - 4 == listNumber)
                    rot = RotateAngle(rot, Directions.Top);
                else
                    rot = RotateAngle(rot, Directions.Left);
            }
            anim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
        }

        private float RotateAngle(float z, Directions d)
        {
            return Mathf.MoveTowardsAngle(z, (int)d, circleAngle);
        }

        public void AnimationFinished()
        {
            print("Animation Finished");
        }
    }
}
