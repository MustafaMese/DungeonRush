using DungeonRush.Field;
using DungeonRush.Data;
using DungeonRush.Property;
using UnityEngine;

namespace DungeonRush
{
    namespace Cards
    {
        public class PlayerCard : Card
        {
            public override int GetHealth()
            {
                return health.Get();
            }

            public override Sprite GetImage()
            {
                return characterSprite.sprite;
            }

            public override string GetCardName()
            {
                return cardName;
            }

            public override Tile GetTile()
            {
                return coordinate;
            }

            public override CardType GetCardType()
            {
                return cardProperties.cardType;
            }

            public override void SetHealth(int health)
            {
                this.health.Set(health);
                UpdateHealthText();
            }

            protected override void UpdateHealthText()
            {
                textMeshHealth.text = health.Get().ToString();
            }

            public override void IncreaseHealth(int health)
            {
                this.health.ChangeHealth(false, health);
                UpdateHealthText();
            }

            public override void DecreaseHealth(int damage)
            {
                this.health.ChangeHealth(true, damage);
                UpdateHealthText();
            }

            public override void SetTile(Tile coordinate)
            {
                this.coordinate = coordinate;
            }

            public override void SetCardType(CardType type)
            {
                cardProperties.cardType = type;
            }

            public override Move GetMove()
            {
                return this.move;
            }

            public override void SetMove(Move move)
            {
                this.move = move;
            }

            public override void SetLevel(int level)
            {
                this.cardLevel = level;
            }

            public override int GetLevel()
            {
                return this.cardLevel;
            }

            public override SpriteRenderer GetFrameColor()
            {
                return frameColor;
            }

            public override void SetCardFrameColor(Color32 color)
            {
                this.frameColor.color = color;
            }

            public override void ExecuteMove()
            {
                //mover.startMoving = true;
                mover.Move();
            }
        }
    }
}
