using DungeonRush.Element;
using DungeonRush.Moves;
using UnityEngine;

namespace DungeonRush {

    namespace Managers {
        public class AnimationHandler : MonoBehaviour
        {
            const int attackIndex = 0;
            const int healthIndex = 1;
            const int coinIndex = 2;

            private Animator anim;

            private void Start()
            {
                anim = GetComponentInChildren<Animator>();
            }

            public void DoAnim(MoveType moveType, Tile targetTile, int listNumber)
            {
                switch (moveType)
                {   
                    case MoveType.Attack:
                        PlaceAnimationGameObject(targetTile);
                        anim.SetTrigger("attack");
                        break;
                    case MoveType.Item:
                        PlaceAnimationGameObject(targetTile);
                        if(targetTile.GetCard().GetItemType() == ItemType.POTION)
                            anim.SetTrigger("health");
                        break;
                    case MoveType.Coin:
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

            // TODO V4
            //private void PlaceAnimationGameObject(Tile t, MoveType type, int listNumber)
            //{
            //    anim.transform.position = t.transform.position;
            //    if (type == MoveType.Attack)
            //    {
            //        int tListNumber = t.GetListNumber();
            //        if (tListNumber + 1 == listNumber)
            //            anim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            //        else if (tListNumber + 4 == listNumber)
            //            anim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270f));
            //        else if (tListNumber - 4 == listNumber)
            //            anim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
            //    }
            //}
        }
    } 
}