using UnityEngine;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Property;

namespace DungeonRush.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/Weapon")]
    public class Weapon : Item
    {
        [SerializeField] AttackStyle attackStyle;
        [SerializeField] BoneType bone;
        [Space]
        [Header("If this is a dual item, then fill the following varible.")]
        [SerializeField] Sprite secondarySprite;

        public override void Execute(Card card)
        {
            SetWeapon(card);
        }

        private void SetWeapon(Card card)
        {
            ItemUser itemUser = card.GetComponent<ItemUser>();
            Attacker attacker = card.GetComponent<Attacker>();
            if (itemUser != null && attacker != null && itemUser.isWeaponUser)
            {
                itemUser.weapon = this;
                itemUser.weaponBone.sprite = GetPrimarySprite();
                attacker.SetAttackStyle(attackStyle);
            }
        }

        public override int GetPower()
        {
            return attackStyle.GetPower();
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