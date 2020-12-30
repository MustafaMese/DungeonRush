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
            public Skill skill;
            public Button button;

            public SkillButton(Skill s, Button b)
            {
                skill = s;
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

        public void AddSkill(Skill skillData)
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
            if (button == null) return;

            Skill skill = null;
            if (skillButton.skill != null)
                skill = skillButton.skill;

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

        public Button FindButton(Skill skill)
        {
            for (int i = 0; i < skillButtons.Count; i++)
            {
                if (skillButtons[i].skill != null && skillButtons[i].skill == skill)
                    return skillButtons[i].button;
            }
            return null;
        }

        private void FindEmptyButton(List<SkillButton> skillButtons, Skill skill)
        {
            foreach (var skillButton in skillButtons)
            {
                if (skillButton.skill == null)
                {
                    skillButton.skill = skill;
                    SetButton(skillButton);
                    return;
                }
            }
        }

        public void Execute(Button button)
        {
            if (!card.Controller.IsRunning()) return;

            SkillButton skillButton = FindSkill(button);
            skillUser.ExecuteActiveSkill(skillButton.skill);
        }

        public void EnableDisableButton(Skill skillData, bool enable)
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
