﻿using DungeonRush.Cards;
using DungeonRush.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.Traits
{
    public class StatusController : MonoBehaviour
    {
        [SerializeField] CharacterCanvas characterCanvas;

        public List<Status> activeStatuses = new List<Status>();
        public List<StatusObject> STATUS = new List<StatusObject>();

        private Card card;
        private void Start()
        {
            card = GetComponent<Card>();
            for(int i = 0; i < STATUS.Count; i++)
            {
                AddStatus(STATUS[i]);
            }
        }

        public void AddStatus(StatusObject statusObject)
        {
            Status status = statusObject.Create(transform);
            status.Initialize(characterCanvas, this);
            activeStatuses.Add(status);
        }

        private GameObject InstatiateObject(GameObject obj)
        {
            GameObject effect = Instantiate(obj, transform);
            effect.SetActive(false);
            return effect;
        }

        public void ActivateStatuses()
        {
            for (int i = 0; i < activeStatuses.Count; i++)
            {
                ApplyStatus(activeStatuses[i]);
            }
        }

        private void ApplyStatus(Status status)
        {
            status.Execute(card);
            status.Adjust();
        }

        public void Notify(Status status)
        {
            activeStatuses.Remove(status);
        }
    }
}
