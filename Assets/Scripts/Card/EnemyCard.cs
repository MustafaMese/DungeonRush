using DungeonRush.Element;
using DungeonRush.Moves;
using UnityEngine;

namespace DungeonRush
{
    namespace Cards
    {
        public class EnemyCard : Card
        {
            public override void ExecuteMove()
            {
                mover.startMoving = true;
            }

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
                return cardProperties.cardName;
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
        }
    }
}