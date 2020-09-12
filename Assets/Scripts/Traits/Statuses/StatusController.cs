using DungeonRush.Cards;
using DungeonRush.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StatusController : MonoBehaviour
{
    [Serializable]
    public class StatusData
    {
        public Status status;
        public ObjectPool poolForStatusEffect;
        public ObjectPool poolForTextPopup;

        public int turnCount;

        public StatusData(Status status, GameObject statusEffect, GameObject textPopup)
        {
            this.status = status;

            poolForStatusEffect = new ObjectPool();
            poolForTextPopup = new ObjectPool();

            poolForStatusEffect.SetObject(statusEffect);
            poolForStatusEffect.FillPool(1);
            poolForTextPopup.SetObject(textPopup);
            poolForTextPopup.FillPool(1);
            turnCount = status.TurnCount;
        }
    }

    public List<StatusData> activeStatuses = new List<StatusData>();
    private Card card;

    private void Start()
    {
        card = GetComponent<Card>();
    }

    public void AddStatus(Status status)
    {
        GameObject effect = Instantiate(status.Effect, transform);
        GameObject textPopup = Instantiate(status.TextPopUp, transform);
        StatusData sd = new StatusData(status, effect, textPopup);

        effect.SetActive(false);
        textPopup.gameObject.SetActive(false);

        activeStatuses.Add(sd);
    }

    public void ActivateStatuses()
    {
        for (int i = 0; i < activeStatuses.Count; i++)
        {
            ApplyStatus(activeStatuses[i]);
        }
    }

    private void StatusControl(StatusData statusData)
    {
        statusData.turnCount--;
        if (statusData.turnCount <= 0)
        {
            activeStatuses.Remove(statusData);
            StartCoroutine(KillStatus(statusData));
        }
    }

    private void ApplyStatus(StatusData statusData)
    {
        statusData.status.Execute(card);
        StartCoroutine(TextPopup(statusData));
        StartCoroutine(Animate(statusData));
    }

    private IEnumerator Animate(StatusData statusData)
    {
        GameObject obj = statusData.poolForStatusEffect.PullObjectFromPool();
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        yield return new WaitForSeconds(statusData.status.EffectLifeTime);
        statusData.poolForStatusEffect.AddObjectToPool(obj);
        StatusControl(statusData);
    }

    private IEnumerator TextPopup(StatusData statusData)
    {
        GameObject obj = statusData.poolForTextPopup.PullObjectFromPool();
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        TextPopup objTxt = obj.GetComponent<TextPopup>();
        string power = statusData.status.Power.ToString();
        Vector3 pos = transform.position;
        objTxt.Setup(power, pos);
        yield return new WaitForSeconds(statusData.status.EffectLifeTime);
        statusData.poolForTextPopup.AddObjectToPool(obj);
    }

    private IEnumerator KillStatus(StatusData statusData)
    {
        yield return new WaitForSeconds(0.6f);
        statusData.poolForStatusEffect.DeleteObjectsInPool();
        statusData.poolForTextPopup.DeleteObjectsInPool();
    }
}
