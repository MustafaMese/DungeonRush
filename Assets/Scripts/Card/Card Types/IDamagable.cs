namespace DungeonRush.Cards
{
    public interface IDamagable
    {
        void SetCurrentHealth(int amount);
        void SetMaxHealth(int amount);
        int GetMaxHealth();
        int GetHealth();
        void IncreaseMaxHealth(int h);
        void DecreaseMaxHealth(int h);
        void IncreaseHealth(int h);
        void DecreaseHealth(int h);
    }
}
