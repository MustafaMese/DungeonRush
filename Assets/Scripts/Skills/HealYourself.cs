using DungeonRush.Cards;
using DungeonRush.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "Skill/Healing")]
    public class HealYourself : Skill
    {
        [SerializeField] int healPower = 2;

        [SerializeField] GameObject healPrefab = null;
        private GameObject healPrefabInstance = null;

        public override void Execute(Move move)
        {
            Card card = move.GetCard();
            card.IncreaseHealth(healPower);
            AnimateObject(card.transform);
        }

        private void AnimateObject(Transform t)
        {
            if (healPrefabInstance == null)
                InitializeObject(t.position, t);
            else
                EnableObject(t.position);
        }

        public override void DisableObject()
        {
            if (healPrefabInstance != null)
                healPrefabInstance.SetActive(false);
        }

        private void InitializeObject(Vector3 pos, Transform parent)
        {
            healPrefabInstance = Instantiate(healPrefab, pos, Quaternion.identity, parent);
        }

        private void EnableObject(Vector3 pos)
        {
            healPrefabInstance.SetActive(true);
            healPrefabInstance.transform.position = pos;
        }

    }
}
