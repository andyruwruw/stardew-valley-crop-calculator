namespace CropCalculator.Options
{
    /// <summary>
    /// Saved form options.
    /// </summary>
    public record ModOptions
    {
        /// <summary>
        /// Whether to consider cross-season crops.
        /// </summary>
        public bool UseCrossSeasonCrops { get; set; } = true;

        /// <summary>
        /// Hard code number of crops.
        /// </summary>
        public int CropCount { get; set; } = 1;

        /// <summary>
        /// Override crop count to display max seeds you could buy.
        /// </summary>
        public bool UseAvailableCapital { get; set; } = true;

        /// <summary>
        /// How the crops will be processed and sold. See <see cref="Enums.ProduceType"/>.
        /// </summary>
        public int ProduceType { get; set; } = 0;

        /// <summary>
        /// Number of Preserves Jars / Kegs available for use. -1 assumes infinite.
        /// </summary>
        public int EquipmentAvailability { get; set; } = -1;

        /// <summary>
        /// How long the processed goods will be aged. See <see cref="Enums.Aged"/>.
        /// </summary>
        public int UseAging { get; set; } = 0;

        /// <summary>
        /// How to display profits. See <see cref="Enums.ProfitDisplay"/>.
        /// </summary>
        public int ProfitDisplay { get; set; } = 0;

        /// <summary>
        /// Sources of seeds. See <see cref="Enums.SeedSource"/>.
        /// </summary>
        public int[] SeedSources { get; set; } = new int[3];

        /// <summary>
        /// Whether the player will be purchasing seeds.
        /// </summary>
        public bool PayForSeeds { get; set; } = true;

        /// <summary>
        /// Whether the user plans on processing 50% of the crops back into seeds.
        /// </summary>
        public bool ProcessAndReplant { get; set; } = true;

        /// <summary>
        /// Type of fertilizer planned to be used. See <see cref="Enums.Fertilizer"/>.
        /// </summary>
        public int UseFertilizer { get; set; } = 0;

        /// <summary>
        /// Whether the player will be purchasing fertilizer.
        /// </summary>
        public bool PayForFertilizer { get; set; } = false;

        /// <summary>
        /// Whether a farming food buff will be used. See <see cref="Enums.FarmingFoodBuff"/>.
        /// </summary>
        public int UseFarmingFoodBuff { get; set; } = 0;
    }
}
