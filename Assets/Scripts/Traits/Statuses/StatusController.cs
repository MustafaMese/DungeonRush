using DungeonRush.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    [Serializable]
    public class StatusData
    {
        public Status status;
        public GameObject statusEffect;

        public StatusData(Status status, GameObject statusEffect)
        {
            this.status = status;

            this.statusEffect = Instantiate(statusEffect);
            this.statusEffect.SetActive(false);
        }
    }

    public List<StatusData> activeStatuses = new List<StatusData>();

    private Card card;

    private void Start()
    {
        card = GetComponent<Card>();
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
        statusData.status.TurnCount--;
        if (statusData.status.TurnCount <= 0)
        {
            Destroy(statusData.statusEffect);
            activeStatuses.Remove(statusData);
        }
    }

    private void ApplyStatus(StatusData statusData)
    {
        statusData.status.Execute(card);
        Animate(statusData);
    }

    private IEnumerator Animate(StatusData statusData)
    {
        statusData.statusEffect.SetActive(true);
        statusData.statusEffect.transform.SetParent(card.transform);
        statusData.statusEffect.transform.position = card.transform.position;
        yield return new WaitForSeconds(statusData.status.EffectLifeTime);
        statusData.statusEffect.SetActive(false);
        StatusControl(statusData);
    }
}
