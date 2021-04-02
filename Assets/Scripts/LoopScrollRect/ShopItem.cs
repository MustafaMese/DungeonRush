using DungeonRush.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : LoopScrollRectItem
{
    public TextMeshProUGUI priceText;
	public Button buton;
	public Image icon;
	public Image shadow;

	[HideInInspector] public bool itemYeniOlusturuldu = true;
    [HideInInspector] public Item item;
}