using DungeonRush.Items;
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

        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI explanationText;
        [SerializeField] GameObject panel;
        [SerializeField] Image sprite;

        public void EnablePanel(Item i)
        {
            panel.SetActive(true);
            SetSprite(i.GetUISprite());
            SetText(i.GetName(), i.GetExplanation());
        }

        public void EnablePanel(Skill s)
        {
            panel.SetActive(true);
            SetSprite(s.IconBig);
            SetText(s.SkillName, s.Explanation);
        }

        private void SetText(string objName, string objExp)
        {
            nameText.text = objName;
            explanationText.text = objExp;
        }

        public void DisablePanel()
        {
            sprite.sprite = null;
            panel.SetActive(false);

            GameManager.Instance.SetGameState(GameState.PLAY);
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