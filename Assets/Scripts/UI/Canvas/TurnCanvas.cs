using DungeonRush.Cards;
using DungeonRush.Controller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.UI
{
    public class TurnCanvas : MonoBehaviour, ICanvasController
    {
        private class UIImage
        {
            public Sprite image;
            public bool lastOne;

            public UIImage(Sprite i, bool l)
            {
                image = i;
                lastOne = l;
            }
        }

        private const string playerTurnText = "Your turn!";
        private const string enemyTurnText = "Enemy turn!";

        [SerializeField] TextMeshProUGUI textMesh;
        [SerializeField] Sprite playerIcon;
        [SerializeField] Sprite nooneIcon;
        [SerializeField] List<Image> images = new List<Image>();
        [SerializeField] List<UIImage> cards = new List<UIImage>();
        [SerializeField] GameObject panel;

        private int activeIndex = 0;

        private void Start()
        {
            Initialize();
        }

        public void ChangeText(bool playerTurn)
        {
            if (playerTurn)
                textMesh.text = playerTurnText;
            else
                textMesh.text = enemyTurnText;
        }

        public void SetCardIcons(List<EnemyAIController> aIs)
        {
            bool lastOne = false;
            activeIndex = 0;
            cards.Clear();
            for (int i = 0; i < aIs.Count; i++)
            {
                if (i + 1 == aIs.Count)
                    lastOne = true;

                UIImage image = new UIImage(aIs[i].GetCard().GetCharacterIcon(), lastOne);
                cards.Add(image);
            }
        }

        public void SetImages(int index)
        {
            if (index > 0 && cards.Count > index - 1 && cards[index - 1].image != null)
                images[0].sprite = cards[index - 1].image;
            else
                images[0].sprite = playerIcon;

            if (cards.Count > index && cards[index].image != null)
                images[1].sprite = cards[index].image;

            if (cards.Count > index + 1 && cards[index + 1].image != null)
                images[2].sprite = cards[index + 1].image;
            else
                images[2].sprite = nooneIcon;

            if (cards.Count > index && cards[index].lastOne)
                images[2].sprite = playerIcon;
        }

        public void Next()
        {
            activeIndex++;
            SetImages(activeIndex);
        }

        public void Initialize()
        {
            if (cards.Count > 0)
                images[0].sprite = cards[cards.Count - 1].image;
            else
                images[0].sprite = nooneIcon;

            images[1].sprite = playerIcon;
            images[2].sprite = nooneIcon;
        }

        public void PanelControl(bool activate)
        {
            panel.SetActive(activate);
        }
    }
}
