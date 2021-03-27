
[System.Serializable]
public class Stats
{
    public int maximumHealth;
    public int damage;
    public int criticChance;
    public int dodgeChance;
    public int lifeCount;
    public int totalMoveCount;
    public int instantMoveCount;
    public int lootChance;
    public bool canBlockTraps;

    public int MaximumHealth { get => maximumHealth; set => maximumHealth = value; }
    public int Damage { get => damage; set => damage = value; }
    public int CriticChance { get => criticChance; set => criticChance = value; }
    public int DodgeChance { get => dodgeChance; set => dodgeChance = value; }
    public int LifeCount { get => lifeCount; set => lifeCount = value; }
    public int TotalMoveCount { get => totalMoveCount; set => totalMoveCount = value; }
    public int InstantMoveCount { get => instantMoveCount; set => instantMoveCount = value; }
    public int LootChance { get => lootChance; set => lootChance = value; }
    public bool CanBlockTraps { get => canBlockTraps; set => canBlockTraps = value; }
    
}
