using System.Collections.Generic;
using DungeonRush.Data;
using DungeonRush.Items;
using DungeonRush.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketUI : MonoBehaviour
{
	public class ItemInfo
	{
		public Item item;

		public ItemInfo(Item item)
		{
			this.item = item;
		}
	}

	public LoopScrollRect scrollView;
	public TextMeshProUGUI goldText;

	private int gold;
	private List<ItemInfo> itemInfo;
	private int seciliItemIndex = -1;

	private void Start()
    {
		PlayerUtility utilities = SavingSystem.LoadUtilities();
		gold = utilities.gold;
        goldText.text = gold.ToString();

        InitializeMarket(ItemDB.Instance.weapons);
    }

    private void InitializeMarket(List<Item> items)
    {
        int count = items.Count;
        itemInfo = new List<ItemInfo>(count);
        for (int i = 0; i < count; i++)
        {
            Item it = items[i];
			if(!it.Purchased)
				itemInfo.Add(new ItemInfo(it));
        }
        // Scroll View'ı aşağı-yukarı kaydırdıkça, kameranın görüş alanına giren
        // her yeni entry için ItemiGuncelle fonksiyonunu çağır
        scrollView.ItemCallback = UpdateItem;

        // Markette kaç eşya olduğu bilgisini LoopScrollRect'e bildir
        scrollView.totalCount = itemInfo.Count;

        // LoopScrollRect'e ekrandaki tüm TestItem'ları yeniden oluşturmasını söyle
        scrollView.RefillCells();
    }

    // index'teki eşyanın TestItem'ı kameranın görüş alanına girdi; TestItem'ın içeriğini
    // o index'teki eşyanın verisini gösterecek şekilde güncelle
    private void UpdateItem( LoopScrollRectItem i, int index )
	{
        ShopItem item = (ShopItem) i;
		item.item = itemInfo[index].item;

		item.priceText.text = item.item.Price.ToString();
		item.icon.sprite = item.item.GetUISprite();

        Color c = item.shadow.color;
		if(gold < item.item.Price)
			c.a = 1;
		else
            c.a = 0;
        item.shadow.color = c;

		if( item.itemYeniOlusturuldu )
		{
			item.itemYeniOlusturuldu = false;
			item.buton.onClick.AddListener( () => PressItem( item ) );
		}
	}

	// Bir TestItem'a tıklandı
	// TODO Bir de burası uyarlanacak
	private void PressItem( ShopItem item )
	{
		if(!item.item.Purchased && gold >= item.item.Price)
		{
			gold -= item.item.Price;
			goldText.text = gold.ToString();
			item.buton.enabled = false;
			item.item.Purchased = true;

			var utilities = SavingSystem.LoadUtilities();
			utilities.gold = gold;
			SavingSystem.SaveUtilities(utilities.totalXp, utilities.gold);

			var data = SavingSystem.LoadItems();
			data.purchasedIDs.Add(item.item.GetID());
			SavingSystem.SaveItems(data);

            scrollView.ItemCallback = UpdateItem;
            seciliItemIndex = item.CurrentRow;
            scrollView.RefreshCells();

			if(item.item.GetItemType() == ItemType.WEAPON)
            	Refresh(false);
			else if(item.item.GetItemType() == ItemType.ARMOR)
				Refresh(true);
		}

        
	}

	private void Delete()
	{
        itemInfo.Clear();
        scrollView.ClearCells();
	}

	public void Refresh(bool isArmor)
	{
		Delete();
		if(!isArmor)
			InitializeMarket(ItemDB.Instance.weapons);
		else if(isArmor)
			InitializeMarket(ItemDB.Instance.armors);
	}
}