using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketItem : LoopScrollRectItem
{
    public string ID;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;

    public Button button;
    public Image image;
}
