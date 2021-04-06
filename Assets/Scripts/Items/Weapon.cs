using UnityEngine;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Property;
using System.Collections.Generic;

namespace DungeonRush.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/Weapon")]
    public class Weapon : Item
    {
        [Multiline(2), SerializeField] string explanation;
        
        [Header("Weapon Properties")]
        [Space(25f)]
        [SerializeField] AttackStyle attackStyle;
        [SerializeField] BoneType bone;
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

        public override string GetExplanation()
        {
            return explanation;
        }

        public List<Impact> GetImpacts()
        {
            return attackStyle.GetImpacts();
        }
    }
}