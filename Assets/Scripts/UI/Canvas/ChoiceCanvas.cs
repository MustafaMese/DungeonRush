using System.Collections;
using System.Collections.Generic;
using DungeonRush.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.UI
{
    public class ChoiceCanvas : MonoBehaviour, ICanvasController
    {
        [SerializeField] GameObject panel;

        [SerializeField] Image itemImage;
        [SerializeField] TextMeshProUGUI itemName;
        [SerializeField] TextMeshProUGUI itemPower;
        [SerializeField] TextMeshProUGUI itemImpacts;
        [SerializeField] TextMeshProUGUI itemExplanation;
        [SerializeField] Button itemButton;

        [SerializeField] Image lootImage;
        [SerializeField] TextMeshProUGUI lootName;
        [SerializeField] TextMeshProUGUI lootPower;
        [SerializeField] TextMeshProUGUI lootImpacts;
        [SerializeField] TextMeshProUGUI lootExplanation;
        [SerializeField] Button lootButton;

        [SerializeField] Image powerImage1;
        [SerializeField] Image powerImage2;

        [SerializeField] Sprite weaponSprite;
        [SerializeField] Sprite armorSprite;

        private ItemUser itemUser;
        private bool enableButtons;
        private Item tempItem;

        private void Start() 
        {
            enableButtons = false;
        }

        public void PanelControl(bool activate)
        {
            panel.SetActive(activate);
        }

        public void SetPowerImages(ItemType type)
        {
            switch (type)
            {
                case ItemType.ARMOR:
                    powerImage1.sprite = armorSprite;
                    powerImage2.sprite = armorSprite;
                    break;
                case ItemType.WEAPON:
                    powerImage1.sprite = weaponSprite;
                    powerImage2.sprite = weaponSprite;
                    break;
            }
            
        }

        public void EnablePanel(Item item, Item loot, ItemUser itemUser)
        {
            SetItemPanel(item.GetUISprite(), item.GetName(), item.GetPower(), " ", " Bu kılıç kadim zamanlarda ilk youtuberlar tarafından yapıldı. ");
            SetLootPanel(loot.GetUISprite(), loot.GetName(), loot.GetPower(), " ", " Bu kılıç kadim zamanlarda ilk youtuberlar tarafından yapıldı. ");
            
            this.itemUser = itemUser;
            tempItem = loot;
            enableButtons = true;
            PanelControl(true);
        }

        public void SetItemPanel(Sprite image, string name, int power, string impacts, string explanation)
        {
            itemImage.sprite = image;
            itemName.text = name;
            itemPower.text = power.ToString();
            // for (var i = 0; i < impacts.Count; i++)
            //     itemImpacts.text += impacts[i] + "\n";
            itemExplanation.text = explanation;
        }

        public void SetLootPanel(Sprite image, string name, int power, string impacts, string explanation)
        {
            lootImage.sprite = image;
            lootName.text = name;
            lootPower.text = power.ToString();
            // for (var i = 0; i < impacts.Count; i++)
            //     lootImpacts.text += impacts[i] + "\n";
            lootExplanation.text = explanation;
        }

        public void ChooseLoot()
        {
            itemUser.ExecuteItem(tempItem, tempItem.GetBoneType());
            PanelControl(false);
            print("loot");
            tempItem = null;
            enableButtons = false;
        }

        public void ChooseItem()
        {
            PanelControl(false);
            print("item");
            enableButtons = false;
        }
    }
}
