using DungeonRush.Items;
using DungeonRush.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.UI
{
    public class PickItemCanvas : MonoBehaviour, ICanvasController
    {
        [SerializeField] string text = "You took the ";

        [SerializeField] TextMeshProUGUI textMesh;
        [SerializeField] GameObject panel;
        [SerializeField] Image itemSprite;

        private string itemName = "";

        public void EnablePanel(Item i)
        {
            panel.SetActive(true);
            SetItemName(i.GetItemName());
            SetItemSprite(i.GetBigSprite());
            SetText();
        }

        private void SetText()
        {
            string s = text + itemName;
            textMesh.text = s;
        }

        public void DisablePanel()
        {
            itemName = "";
            itemSprite.sprite = null;
            panel.SetActive(false);
            GameManager.Instance.SetGameState(GameState.PLAY);
        }

        private void SetItemName(string n)
        {
            itemName = n;
        }

        private void SetItemSprite(Sprite i)
        {
            itemSprite.sprite = i;
        }

        public void PanelControl(bool activate)
        {
            panel.SetActive(activate);
        }
    }
}