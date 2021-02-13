public class Stats
{
    private int maximumHealth;
    private int criticChance;
    private int dodgeChance;
    private int lifeCount;
    private int totalMoveCount;
    private int instantMoveCount;
    private int lootChance;
    private bool canBlockTraps;

    public int MaximumHealth { get => maximumHealth; set => maximumHealth = value; }
    public int CriticChance { get => criticChance; set => criticChance = value; }
    public int DodgeChance { get => dodgeChance; set => dodgeChance = value; }
    public int LifeCount { get => lifeCount; set => lifeCount = value; }
    public int TotalMoveCount { get => totalMoveCount; set => totalMoveCount = value; }
    public int InstantMoveCount { get => instantMoveCount; set => instantMoveCount = value; }
    public int LootChance { get => lootChance; set => lootChance = value; }
    public bool CanBlockTraps { get => canBlockTraps; set => canBlockTraps = value; }
}
