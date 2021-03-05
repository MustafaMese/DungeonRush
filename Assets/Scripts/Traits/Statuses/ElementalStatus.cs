using System.Collections;
using DungeonRush.Cards;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Traits
{
    public class ElementalStatus : Status
    {
        [SerializeField] ElementType elementType;

        public override void Execute(Card card, Tile tile)
        {
            if(card != null)
            {
                if (Power > 0)
                    card.GetDamagable().IncreaseHealth(Power);
                else
                    card.GetDamagable().DecreaseHealth(Power);
            }
            
            StartCoroutine(KillStatus());
        }

        public void Initialize(Vector3 pos)
        {
            transform.position = pos;
            FillPool();
            effectUsed = false;

            Animate();
        }

        private void FillPool()
        {
            effectPool = new Data.ObjectPool<GameObject>();
            effectPool.SetObject(effect);
            effectPool.Fill(1, transform);

            for (var i = 0; i < 1; i++)
            {   
                GameObject obj = effectPool.Pull(transform);
                obj.SetActive(false);
                effectPool.Push(obj);
            }
        }

        protected override IEnumerator KillStatus()
        {
            yield return new WaitForSeconds(effectTime);
            Destroy(this.gameObject);
        }

        public ElementType GetElementType()
        {
            return elementType;
        }
     }
}