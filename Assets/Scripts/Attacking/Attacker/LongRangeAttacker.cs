using DungeonRush.Data;
using DungeonRush.Property;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LongRangeAttacker : Attacker
{
    [SerializeField] float damageTime = 0.3f;

    public override void Attack()
    {
        attackFinished = false;
        Move move = card.GetMove();
        StartCoroutine(FinishAttack(move));
    }

    private IEnumerator FinishAttack(Move move)
    {
        UpdateAnimation(true, true);
        Damage(move);
        yield return new WaitForSeconds(damageTime);
        attackFinished = true;
    }

    private void Damage(Move move)
    {
        AttackAction(move);
    }

    protected override IEnumerator StartAttackAnimation(ObjectPool pool, Move move, float time)
    {
        Vector2 pos = move.GetTargetTile().GetCoordinate();
        Transform cardTransform = move.GetCardTile().transform;

        if(move.GetTargetTile().GetCard() != null)
        {
            GameObject obj = pool.PullObjectFromPool();
            attackStyle.SetEffectPosition(obj, cardTransform.position, cardTransform);
            obj.transform.DOMove(pos, time).OnComplete(() => FinishAnimation(obj, pool, move));
        }
        yield return null;
    }

    private void FinishAnimation(GameObject obj, ObjectPool pool, Move move)
    {
        Transform cardTransform = move.GetCardTile().transform;

        attackStyle.SetEffectPosition(obj, cardTransform.position, cardTransform);
        pool.AddObjectToPool(obj);
    }
}
