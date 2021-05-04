using System.Collections;
using System.Collections.Generic;
using DungeonRush.Controller;
using DungeonRush.Items;
using DungeonRush.Managers;
using DungeonRush.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.UI
{
    public class RewardCanvas : MonoBehaviour, ICanvasController
    {
        [System.Serializable]
        public class RewardButton
        {
            public Button button;
            public SkillObject skill;

            public RewardButton(Button button)
            {
                this.button = button;
                skill = null;
            }

        }
        public List<RewardButton> rewardButtons = new List<RewardButton>();

        [SerializeField] GameObject panel;
        [SerializeField] Button[] buttons;
        [SerializeField] Sprite foreshadow;

        public bool isChosen;

        private void Start() 
        {
            isChosen = false;

            for (var i = 0; i < buttons.Length; i++)
            {
                rewardButtons.Add(new RewardButton(buttons[i]));
                buttons[i].image.sprite = foreshadow;
            }    
        }

        public void PanelControl(bool activate)
        {
            panel.SetActive(activate);
            if(activate)
                SetButtons();
            else
                ResetButtons();
            
        }

        private void SetButtons()
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                SkillObject skill = ItemDB.Instance.GetRandomSkill();
                rewardButtons[i].button = buttons[i];
                rewardButtons[i].skill = skill;
                rewardButtons[i].button.image.sprite = rewardButtons[i].skill.skillPrefab.IconSmall;
            }
                
        }

        private void ResetButtons()
        {
            for (var i = 0; i < rewardButtons.Count; i++)
            {
                rewardButtons[i].button.image.sprite = null;
                rewardButtons[i].skill = null;
            }
        }

        // Button method.
        public void ChooseSkill(Button button)
        {
            if(isChosen) return;
            
            isChosen = true;
            SkillObject skill = FindSkill(button);
            MoveSchedular.Instance.playerController.GetComponent<SkillUser>().AddSkillToList(skill);
            StartCoroutine(LevelTransition());
        }

        private IEnumerator LevelTransition()
        {
            MoveSchedular.Instance.playerController.SavePlayer();
            yield return new WaitForSeconds(1f);
            GameManager.Instance.SetGameState(GameState.LEVEL_TRANSITION);
        }

        private SkillObject FindSkill(Button button)
        {
            SkillObject skill = null;
            for (var i = 0; i < rewardButtons.Count; i++)
            {
                if (rewardButtons[i].button == button)
                    skill = rewardButtons[i].skill;
                else
                    rewardButtons[i].button.interactable = false;
            }

            return skill;
        }
    }
}


