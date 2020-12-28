using DungeonRush.Attacking;
using DungeonRush.Data;
using DungeonRush.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Operate(ObjectPool<GameObject> pool, Vector3 pos, float time)
    {
        StartCoroutine(OperateEffect(pool, pos, time));
    }

    public void Operate(SkillData skillData, Move move)
    {
        StartCoroutine(OperateEffect(skillData, move));
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

    private IEnumerator OperateEffect(ObjectPool<GameObject> pool, Vector3 pos, float time)
    {
        GameObject obj = pool.Pull(transform);
        obj.SetActive(true);
        obj.transform.position = pos;
        obj.transform.SetParent(null);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        pool.AddObjectToPool(obj);
    }

    private IEnumerator OperateEffect(SkillData skillData, Move move)
    {
        GameObject obj;
        List<GameObject> objects = new List<GameObject>();
        int count = skillData.skill.GetGameobjectCount();
        for (int i = 0; i < count; i++)
        {
            obj = skillData.poolForEffect.Pull(transform);
            obj.SetActive(true);

            skillData.skill.PositionEffect(obj, move);
            objects.Add(obj);
        }

        yield return new WaitForSeconds(skillData.skill.EffectTime);

        for (int i = 0; i < count; i++)
        {
            obj = objects[i];
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            skillData.poolForEffect.AddObjectToPool(obj);
        }
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
