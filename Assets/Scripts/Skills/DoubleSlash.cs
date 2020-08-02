using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using UnityEngine;

namespace DungeonRush.Skills
{
    [CreateAssetMenu(menuName = "Skill/DoubleSlash", order = 1)]
    public class DoubleSlash : Skill
    {
        [SerializeField] int slashPower = 2;
        [SerializeField] GameObject doubleSlashPrefab = null;
        private GameObject doubleSlashInstance = null;

        private Transform cardTransform;

        public override void Execute(Move move)
        {
            Tile targetTile = move.GetTargetTile();
            Card targetCard = targetTile.GetCard();
            cardTransform = move.GetCard().transform;
            
            if(targetCard != null)
                targetCard.DecreaseHealth(slashPower);

            AnimateObject(targetTile.GetCoordinate(), targetTile.transform);
        }

        private void AnimateObject(Vector3 target, Transform cardT)
        {
            if (doubleSlashInstance == null)
                InitializeObject(target, cardT);
            else
                EnableObject(target, cardT);
        }

        private void InitializeObject(Vector3 pos, Transform parent)
        {
            doubleSlashInstance = Instantiate(doubleSlashPrefab, pos, Quaternion.identity, parent);
        }

        private void EnableObject(Vector3 pos, Transform parent)
        {
            doubleSlashInstance.SetActive(true);
            doubleSlashInstance.transform.SetParent(parent);
            doubleSlashInstance.transform.position = pos;
        }
        public override void DisableObject()
        {
            if (doubleSlashInstance != null)
            {
                doubleSlashInstance.SetActive(false);
                doubleSlashInstance.transform.SetParent(cardTransform);
            }
        }
    }
}
