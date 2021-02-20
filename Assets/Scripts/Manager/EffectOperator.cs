using DungeonRush.Attacking;
using DungeonRush.Data;
using System.Collections;
using UnityEngine;

namespace DungeonRush.Managers
{
    public class EffectOperator : MonoBehaviour
    {
        private static EffectOperator instance = null;
        // Game Instance Singleton
        public static EffectOperator Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        private void Awake()
        {
            Instance = this;
        }

        public void Operate(ObjectPool<GameObject> pool, Transform cardTransform, Transform target, float time, AttackStyle attackStyle)
        {
            StartCoroutine(OperateEffect(pool, cardTransform, target, time, attackStyle));
        }

        public void Operate(ObjectPool<GameObject> pool, Transform cardTransform, Vector3 pos, float time)
        {
            StartCoroutine(OperateEffect(pool, cardTransform, pos, time));
        }
        
        private IEnumerator OperateEffect(ObjectPool<GameObject> pool, Transform cardTransform, Transform target, float time, AttackStyle attackStyle)
        {
            GameObject obj = pool.Pull(cardTransform);
            obj.SetActive(true);
            attackStyle.SetEffectPosition(obj, target.position, target);
            yield return new WaitForSeconds(time);
            obj.SetActive(false);
            attackStyle.SetEffectPosition(obj, target.position, cardTransform);
            pool.AddObjectToPool(obj);
        }

        private IEnumerator OperateEffect(ObjectPool<GameObject> pool, Transform cardTransform, Vector3 pos, float time)
        {
            GameObject obj = pool.Pull(cardTransform);
            obj.SetActive(true);
            obj.transform.position = pos;
            obj.transform.SetParent(cardTransform);
            yield return new WaitForSeconds(time);
            obj.SetActive(false);
            pool.AddObjectToPool(obj);
        }

        public void Operate(ObjectPool<GameObject> pool, Transform cardTransform,Vector3 pos)
        {
            GameObject obj = pool.Pull(cardTransform);
            obj.SetActive(true);
            obj.transform.position = pos;
            obj.transform.SetParent(cardTransform);
            pool.AddObjectToPool(obj);
        }

        public void Delete(ObjectPool<GameObject> pool)
        {
            for(int i = 0; i < pool.GetStackLength(); i++)
            {
                GameObject obj = pool.PullForDestroy();
                Destroy(obj);
            }
        }

    }
}