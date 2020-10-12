﻿using DungeonRush.Items;
using DungeonRush.Managers;
using DungeonRush.Skills;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.UI
{
    public class PickCanvas : MonoBehaviour, ICanvasController
    {
        [SerializeField] string text = "You took the ";

        [SerializeField] TextMeshProUGUI textMesh;
        [SerializeField] GameObject panel;
        [SerializeField] Image sprite;

        private string pickedObjectName = "";

        public void EnablePanel(Item i)
        {
            panel.SetActive(true);
            SetName(i.GetItemName());
            SetSprite(i.GetBigSprite());
            SetText();
        }

        public void EnablePanel(Skill s)
        {
            panel.SetActive(true);
            SetName(s.SkillName);
            SetSprite(s.IconBig);
            SetText();
        }

        private void SetText()
        {
            string s = text + pickedObjectName;
            textMesh.text = s;
        }

        public void DisablePanel()
        {
            pickedObjectName = "";
            sprite.sprite = null;
            panel.SetActive(false);
            GameManager.Instance.SetGameState(GameState.PLAY);
        }

        private void SetName(string n)
        {
            pickedObjectName = n;
        }

        private void SetSprite(Sprite i)
        {
            sprite.sprite = i;
        }

        public void PanelControl(bool activate)
        {
            panel.SetActive(activate);
        }
    }
}