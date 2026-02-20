namespace Game.Core
{
    /// <summary>
    /// Defines fish rarity tiers.
    /// Used for spawn probability and visual indicators.
    /// Order matters: Common should be first (lowest value).
    /// </summary>
    public enum FishRarity
    {
        Common = 0,
        Rare = 1,
        Epic = 2
        // Future: Legendary = 3, Mythic = 4, etc.
    }
}
