using DungeonRush.Cards;
using UnityEngine;

namespace DungeonRush.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/Armor")]
    public class Armor : Item
    {
        [SerializeField] int power;
        [SerializeField] BoneType bone;
        [Space]
        [Header("If this is a dual item, then fill the following varible.")]
        [SerializeField] Sprite secondarySprite;

        public override void Execute(Card card)
        {
            ItemUser itemUser = card.GetComponent<ItemUser>();
            SetArmor(itemUser);
        }

        private void SetArmor(ItemUser itemUser)
        {
            if (itemUser != null && itemUser.isWeaponUser)
            {
                itemUser.armor = this;
                itemUser.armorBone.sprite = GetPrimarySprite();
            }
        }

        public override int GetPower()
        {
            return power;
        }

        public override BoneType GetBoneType()
        {
            return bone;
        }

        public override Sprite GetSecondarySprite()
        {
            return secondarySprite;
        }
    }
}

