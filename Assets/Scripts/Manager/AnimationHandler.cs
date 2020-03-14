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

            public void DoAnim(MoveType moveType, Tile targetTile)
            {
                switch (moveType)
                {   
                    case MoveType.Attack:
                        PlaceAnimationGameObject(targetTile);
                        anim.SetTrigger("attack");
                        break;
                    case MoveType.Item:
                        break;
                    case MoveType.Coin:
                        break;
                    default:
                        break;
                }
            }

            private void PlaceAnimationGameObject(Tile t)
            {
                anim.transform.position = t.transform.position;
            }
        }
    } 
}