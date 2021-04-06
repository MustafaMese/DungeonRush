using UnityEngine.UI;
using TMPro;

namespace DungeonRush.Data
{
    public class MarketItem : LoopScrollRectItem
    {
        public string ID;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI priceText;

        public Button button;
        public Image image;
    }
}

