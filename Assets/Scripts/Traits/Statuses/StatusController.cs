using DungeonRush.Cards;
using DungeonRush.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.Traits
{
    [Serializable]
    public class StatusData
    {
        public Status status;
        public ObjectPool<GameObject> poolForStatusEffect;
        public GameObject image;
        public int turnCount;

        public StatusData(Status status, GameObject statusEffect, Transform t, GameObject image = null, int turnCount = -1)
        {
            this.status = status;

            poolForStatusEffect = new ObjectPool<GameObject>();

            if (statusEffect != null)
            {
                poolForStatusEffect.SetObject(statusEffect);
                poolForStatusEffect.FillPool(1, t);
            }
            if (turnCount == -1)
                this.turnCount = status.TurnCount;
            else
                this.turnCount = turnCount;

            if(image != null)
                this.image = image;
        }
    }

    public class StatusController : MonoBehaviour
    {
        
        [SerializeField] CharacterCanvas characterCanvas;

        public List<StatusData> activeStatuses = new List<StatusData>();
        public List<Status> STATUS = new List<Status>();

        private Card card;
        private void Start()
        {
            card = GetComponent<Card>();
            for (int i = 0; i < STATUS.Count; i++)
            {
                AddStatus(STATUS[i]);
            }
        }

        public void AddStatus(Status status)
        {
            GameObject effect = null;
            GameObject textPopup = null;

            if (status.Effect != null)
                effect = InstatiateObject(status.Effect);
            if (status.TextPopUp != null)
                textPopup = InstatiateObject(status.TextPopUp);

            StatusData sd;

            if (status.StatusType != StatusType.INEFFECTIVE)
            {
                GameObject img = characterCanvas.AddImageToPanel(status.Icon).gameObject;
                sd = new StatusData(status, effect, transform, img);
                print("1");
            }
            else
                sd = new StatusData(status, effect, transform);

            activeStatuses.Add(sd);
        }

        private GameObject InstatiateObject(GameObject obj)
        {
            GameObject effect = Instantiate(obj, transform);
            effect.SetActive(false);
            return effect;
        }

        public void AddStatus(StatusData statusData)
        {
            Status status = statusData.status;

            GameObject effect = null;
            GameObject textPopup = null;

            if (status.Effect != null)
                effect = InstatiateObject(status.Effect);
            if (status.TextPopUp != null)
                textPopup = InstatiateObject(status.TextPopUp);

            GameObject img = characterCanvas.AddImageToPanel(status.Icon).gameObject;

            statusData = new StatusData(status, effect, transform, img, statusData.turnCount);
            activeStatuses.Add(statusData);
        }

        public void ActivateStatuses()
        {
            for (int i = 0; i < activeStatuses.Count; i++)
            {
                ApplyStatus(activeStatuses[i]);
            }
        }

        public void StatusControl()
        {
            for (int i = 0; i < activeStatuses.Count; i++)
            {
                StatusData statusData = activeStatuses[i];

                statusData.turnCount--;
                if (statusData.turnCount <= 0)
                {
                    activeStatuses.Remove(statusData);
                    StartCoroutine(KillStatus(statusData));
                }
            }
        }

        private void ApplyStatus(StatusData statusData)
        {
            if (statusData.turnCount > 0)
                statusData.status.Execute(card);
            else
                statusData.status.Execute(card, true);    

            if (statusData.status.TextPopUp != null)
                TextPopup(statusData);
            if (statusData.status.Effect != null)
                StartCoroutine(Animate(statusData));
        }

        private IEnumerator Animate(StatusData statusData)
        {
            GameObject obj = statusData.poolForStatusEffect.PullObjectFromPool(transform);
            obj.transform.position = transform.position;
            yield return new WaitForSeconds(statusData.status.EffectLifeTime);
            statusData.poolForStatusEffect.AddObjectToPool(obj);
        }

        private void TextPopup(StatusData statusData)
        {
            string power = statusData.status.Power.ToString();
            Vector3 pos = transform.position;
            TextPopupManager.Instance.TextPopup(pos, power);
        }

        private IEnumerator KillStatus(StatusData statusData)
        {
            if (statusData.image != null)
            {
                Destroy(statusData.image.gameObject);
                statusData.image = null;
            }

            yield return new WaitForSeconds(0.6f);
            TextPopupManager.Instance.DeleteObjectsInPool(statusData.poolForStatusEffect);
        }
    }
}
