using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.UI;
using DungeonRush.UI.HUD;

namespace DungeonRush.Traits
{
    public interface IStatus
    {
        void Initialize(CharacterHUD canvas, StatusController statusController);
        void Execute(Card card, Tile tile = null);
        void Adjust();
    }

    public enum StatusType
    {
        BLEEDING,       // Kanama hasarı alır.
        BURNING,        // Yanma hasarı alır.
        POISONED,       // Zehir hasarı alır.
        ENTANGLED,      // Yere bağlanma durumu, sadece saldırabilir.
        DISARMED,       // Sadece ilerleyebilir.
        STUNNED,        // Eylem yapamaz.
        FROZEN,         // Buz hasarı alır.
        BLINDED,        // Görüş alanını 1 kare kısacak.
        ACID,           // Zırhı sıfırlayacak.
        SLOWED,         // Kritik/Dodge şansını düşürecek.
        ANGER,          // Sürekli saldırı halinde olacak.
        HEALING,        // Her tur iyileşme alacak.
        HASTE,          // Kritik/Dodge şansını arttırır.
        LUCKY_CHARM,    // Loot şansı artar.
        UNLUCKY_CHARM,  // Loot şansı düşer.
        LIFE_STEAL,     // Can çalmaya başlar.
        INEFFECTIVE     // İşlem gerektirmeyen statuler.
    }
}