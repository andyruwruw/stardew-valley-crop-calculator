using StardewValley;
using StardewValley.GameData.Crops;

namespace CropCalculator.Helpers
{
    /// <summary>
    /// Retrieves game data relevant to calculations.
    /// </summary>
    internal class GameData
    {
        /// <summary>
        /// Whether UI Info Suite is present.
        /// </summary>
        public static bool UiInfoSuiteEnabled = false;

        /// <summary>
        /// Retrieves the current <see cref="Season"/> from the game.
        /// </summary>
        /// <returns>Current in-game <see cref="Season"/>.</returns>
        public static Season GetSeason()
        {
            return Game1.season;
        }

        /// <summary>
        /// Retrieves the current day of the season.
        /// </summary>
        /// <returns>Current day of the season.</returns>
        public static int GetCurrentDay()
        {
            return Game1.dayOfMonth;
        }

        /// <summary>
        /// Retrieves the player's farming level.
        /// </summary>
        /// <returns>Main player's farming level.</returns>
        public static int GetFarmingLevel()
        {
            if (Game1.player != null)
            {
                return Game1.player.farmingLevel.Value;
            }
            return 0;
        }

        /// <summary>
        /// Whether the main player has the tiller profession.
        /// </summary>
        public static bool IsFarmerTiller()
        {
            return Game1.player.professions.Contains(Farmer.tiller);
        }

        /// <summary>
        /// Whether the main player has the agriculturist profession.
        /// </summary>
        public static bool IsFarmerAgriculturist()
        {
            return Game1.player.professions.Contains(Farmer.agriculturist);
        }

        /// <summary>
        /// Whether the main player has the artisan profession.
        /// </summary>
        public static bool IsFarmerArtisan()
        {
            return Game1.player.professions.Contains(Farmer.artisan);
        }

        /// <summary>
        /// Retrieves the player's foraging level.
        /// </summary>
        /// <returns>Main player's foraging level.</returns>
        public static int GetForagingLevel()
        {
            if (Game1.player != null)
            {
                return Game1.player.foragingLevel.Value;
            }
            return 0;
        }

        /// <summary>
        /// Whether the main player has the gatherer profession.
        /// </summary>
        public static bool IsFarmerGatherer()
        {
            return Game1.player.professions.Contains(Farmer.gatherer);
        }

        /// <summary>
        /// Whether the main player has the botanist profession.
        /// </summary>
        public static bool IsFarmerBotanist()
        {
            return Game1.player.professions.Contains(Farmer.botanist);
        }

        /// <summary>
        /// Retrieves a list of <see cref="CropData"/>.
        /// </summary>
        /// <returns>All in-game <see cref="CropData"/>.</returns>
        public static ICollection<CropData> GetCrops()
        {
            return Game1.cropData.Values;
        }

        /// <summary>
        /// Retrieves valid crops for this season.
        /// </summary>
        /// <returns>List of crops.</returns>
        public static IList<CropData> GetSeasonalCrops()
        {
            ICollection<CropData> crops = GameData.GetCrops();
            IList<CropData> seasonalCrops = new List<CropData>();

            foreach (CropData crop in crops)
            {
                if (crop.Seasons.Contains(GameData.GetSeason()))
                {
                    seasonalCrops.Add(crop);
                }
            }

            return seasonalCrops;
        }
    }
}
