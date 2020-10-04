using DungeonRush.Cards;
using DungeonRush.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.UI
{
    public class ActiveSkillCanvas : MonoBehaviour, ICanvasController
    {
        [System.Serializable]
        public class SkillButton
        {
            public SkillData skillData;
            public Button button;

            public SkillButton(SkillData s, Button b)
            {
                skillData = s;
                button = b;
            }
        }

        [SerializeField] GameObject panel;
        [SerializeField] Sprite emptySprite;

        [SerializeField] List<Button> buttons = new List<Button>();
        [SerializeField] List<SkillButton> skillButtons = new List<SkillButton>();

        private SkillUser skillUser;
        private PlayerCard card;

        public void Start()
        {
            card = FindObjectOfType<PlayerCard>();
            skillUser = card.GetComponent<SkillUser>();
            if(skillButtons.Count <= 0)
                InitilizeButtons();
        }

        public void PanelControl(bool activate)
        {
            panel.SetActive(activate);
        }

        public void AddSkill(SkillData skillData)
        {
            if (skillButtons.Count <= 0)
            {
                print("1111");
                InitilizeButtons();
            }
            FindEmptyButton(skillButtons, skillData);
        }

        private void InitilizeButtons()
        {
            print("tatata");
            skillButtons.Clear();

            for (int i = 0; i < buttons.Count; i++)
            {
                SkillButton skillButton = new SkillButton(null, buttons[i]);
                SetButton(skillButton);
                skillButtons.Add(skillButton);
            }
        }

        private void SetButton(SkillButton skillButton)
        {
            Button button = skillButton.button;
            print("1:" + button);
            print("2:" + skillButton.skillData);
            Skill skill = null;
            if (skillButton.skillData != null)
                skill = skillButton.skillData.skill;

            if (skill != null)
                button.image.sprite = skill.IconBig;
            else
                button.image.sprite = emptySprite;
        }

        public SkillButton FindSkill(Button button)
        {
            for (int i = 0; i < skillButtons.Count; i++)
            {
                if (skillButtons[i].button == button)
                    return skillButtons[i];
            }
            return null;
        }

        public Button FindButton(SkillData skillData)
        {
            for (int i = 0; i < skillButtons.Count; i++)
            {
                if (skillButtons[i].skillData != null && skillButtons[i].skillData.listnumber == skillData.listnumber)
                    return skillButtons[i].button;
            }
            return null;
        }

        private void FindEmptyButton(List<SkillButton> skillButtons, SkillData skillData)
        {
            foreach (var skillButton in skillButtons)
            {
                if (skillButton.skillData == null || skillButton.skillData.skill == null)
                {
                    skillButton.skillData = skillData;
                    SetButton(skillButton);
                    return;
                }
            }
        }

        public void Execute(Button button)
        {
            if (!card.Controller.IsRunning()) return;


            SkillButton skillButton = FindSkill(button);
            skillUser.ExecuteActiveSkill(skillButton.skillData);
        }

        public void EnableDisableButton(SkillData skillData, bool enable)
        {
            Button b = FindButton(skillData);

            if (b == null) return;

            if (enable)
                b.interactable = true;
            else
                b.interactable = false;
        }
    }
}
