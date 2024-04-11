using StardewModdingAPI;

namespace CropCalculator.Utilities
{
    internal class Translations
    {
        /// <summary>
        /// Statick reference to mod helper.
        /// </summary>
        private static IModHelper? ModHelper;

        /// <summary>
        /// Retrieves mod name.
        /// </summary>
        public static string GetModName()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("mod.name");
        }

        /// <summary>
        /// Retrieves graph Y label.
        /// </summary>
        public static string GetGraphYLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("graph.y-label");
        }

        /// <summary>
        /// Retrieves days remaining label.
        /// </summary>
        public static string GetOptionsDaysRemainingLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.days-remaining.label");
        }

        /// <summary>
        /// Retrieves enable cross season label.
        /// </summary>
        public static string GetOptionsEnableCrossSeasonLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.enable-cross-season.label");
        }

        /// <summary>
        /// Retrieves enable cross season description.
        /// </summary>
        public static string GetOptionsEnableCrossSeasonDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.enable-cross-season.description");
        }

        /// <summary>
        /// Retrieves enable crop count label.
        /// </summary>
        public static string GetOptionsCropCountLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.crop-count.label");
        }

        /// <summary>
        /// Retrieves use available capital label.
        /// </summary>
        public static string GetOptionsUseAvailableCapitalLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.use-available-capital.label");
        }

        /// <summary>
        /// Retrieves use available capital description.
        /// </summary>
        public static string GetOptionsUseAvailableCapitalDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.use-available-capital.description");
        }

        /// <summary>
        /// Retrieves produce type label.
        /// </summary>
        public static string GetOptionsProduceTypeLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.produce-type.label");
        }

        /// <summary>
        /// Retrieves produce type label.
        /// </summary>
        public static IList<string> GetOptionsProduceTypeValues()
        {
            IList<string> values = new List<string>();

            if (ModHelper != null)
            {
                values.Add(Translations.ModHelper.Translation.Get("options.produce-type.value.1"));
                values.Add(Translations.ModHelper.Translation.Get("options.produce-type.value.2"));
                values.Add(Translations.ModHelper.Translation.Get("options.produce-type.value.3"));
                values.Add(Translations.ModHelper.Translation.Get("options.produce-type.value.4"));
            }

            return values;
        }

        /// <summary>
        /// Retrieves produce type description.
        /// </summary>
        public static string GetOptionsProduceTypeDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.produce-type.description");
        }

        /// <summary>
        /// Retrieves available equipment label.
        /// </summary>
        public static string GetOptionsAvailableEquipmentLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.available-equipment.label");
        }

        /// <summary>
        /// Retrieves available equipment description.
        /// </summary>
        public static string GetOptionsAvailableEquipmentDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.available-equipment.description");
        }

        /// <summary>
        /// Retrieves aging label.
        /// </summary>
        public static string GetOptionsAgingLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.use-aging.label");
        }

        /// <summary>
        /// Retrieves aging label.
        /// </summary>
        public static IList<string> GetOptionsAgingValues()
        {
            IList<string> values = new List<string>();

            if (ModHelper != null)
            {
                values.Add(Translations.ModHelper.Translation.Get("options.use-aging.value.1"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-aging.value.2"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-aging.value.3"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-aging.value.4"));
            }

            return values;
        }

        /// <summary>
        /// Retrieves aging description.
        /// </summary>
        public static string GetOptionsAgingDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.use-aging.description");
        }

        /// <summary>
        /// Retrieves profit display label.
        /// </summary>
        public static string GetOptionsProfitDisplayLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.profit-display.label");
        }

        /// <summary>
        /// Retrieves profit display values.
        /// </summary>
        public static IList<string> GetOptionsProfitDisplayValues()
        {
            IList<string> values = new List<string>();

            if (ModHelper != null)
            {
                values.Add(Translations.ModHelper.Translation.Get("options.profit-display.value.1"));
                values.Add(Translations.ModHelper.Translation.Get("options.profit-display.value.2"));
                values.Add(Translations.ModHelper.Translation.Get("options.profit-display.value.3"));
                values.Add(Translations.ModHelper.Translation.Get("options.profit-display.value.4"));
            }

            return values;
        }

        /// <summary>
        /// Retrieves profit display description.
        /// </summary>
        public static string GetOptionsProfitDisplayDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.profit-display.description");
        }

        /// <summary>
        /// Retrieves seed sources label.
        /// </summary>
        public static string GetOptionsSeedSourcesLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.seed-sources.label");
        }

        /// <summary>
        /// Retrieves seed sources values.
        /// </summary>
        public static IList<string> GetOptionsSeedSourcesValues()
        {
            IList<string> values = new List<string>();

            if (ModHelper != null)
            {
                values.Add(Translations.ModHelper.Translation.Get("options.seed-sources.value.1"));
                values.Add(Translations.ModHelper.Translation.Get("options.seed-sources.value.2"));
                values.Add(Translations.ModHelper.Translation.Get("options.seed-sources.value.3"));
            }

            return values;
        }

        /// <summary>
        /// Retrieves seed sources description.
        /// </summary>
        public static string GetOptionsSeedSourcesDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.seed-sources.description");
        }

        /// <summary>
        /// Retrieves pay for seeds label.
        /// </summary>
        public static string GetOptionsPayForSeedsLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.pay-for-seeds.label");
        }

        /// <summary>
        /// Retrieves pay for seeds description.
        /// </summary>
        public static string GetOptionsPayForSeedsDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.pay-for-seeds.description");
        }

        /// <summary>
        /// Retrieves process & replant label.
        /// </summary>
        public static string GetOptionsProcessAndReplantLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.process-and-replant.label");
        }

        /// <summary>
        /// Retrieves process & replant description.
        /// </summary>
        public static string GetOptionsProcessAndReplantDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.process-and-replant.description");
        }

        /// <summary>
        /// Retrieves use fertilizer label.
        /// </summary>
        public static string GetOptionsUseFertilizerLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.use-fertilizer.label");
        }

        /// <summary>
        /// Retrieves use fertilizer values.
        /// </summary>
        public static IList<string> GetOptionsUseFertilizerValues()
        {
            IList<string> values = new List<string>();

            if (ModHelper != null)
            {
                values.Add(Translations.ModHelper.Translation.Get("options.use-fertilizer.value.1"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-fertilizer.value.2"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-fertilizer.value.3"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-fertilizer.value.4"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-fertilizer.value.5"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-fertilizer.value.6"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-fertilizer.value.7"));
            }

            return values;
        }

        /// <summary>
        /// Retrieves use fertilizer description.
        /// </summary>
        public static string GetOptionsUseFertilizerDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.use-fertilizer.description");
        }

        /// <summary>
        /// Retrieves pay for fertilizer label.
        /// </summary>
        public static string GetOptionsPayForFertilizerLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.pay-for-fertilizier.label");
        }

        /// <summary>
        /// Retrieves pay for fertilizer description.
        /// </summary>
        public static string GetOptionsPayForFertilizerDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.pay-for-fertilizier.description");
        }

        /// <summary>
        /// Retrieves use-food-buff label.
        /// </summary>
        public static string GetOptionsUseFoodBuffLabel()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.use-food-buff.label");
        }

        /// <summary>
        /// Retrieves use-food-buff values.
        /// </summary>
        public static IList<string> GetOptionsUseFoodBuffValues()
        {
            IList<string> values = new List<string>();

            if (ModHelper != null)
            {
                values.Add(Translations.ModHelper.Translation.Get("options.use-food-buff.value.1"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-food-buff.value.2"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-food-buff.value.3"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-food-buff.value.4"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-food-buff.value.5"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-food-buff.value.6"));
                values.Add(Translations.ModHelper.Translation.Get("options.use-food-buff.value.7"));
            }

            return values;
        }

        /// <summary>
        /// Retrieves use-food-buff description.
        /// </summary>
        public static string GetOptionsUseFoodBuffDescription()
        {
            if (ModHelper == null)
            {
                return "";
            }
            return Translations.ModHelper.Translation.Get("options.use-food-buff.description");
        }

        /// <summary>
        /// Sets static reference to <see cref="IModHelper"/>.
        /// </summary>
        /// <param name="modHelper">Reference to <see cref="IModHelper"/>.</param>
        public static void SetModHelper(IModHelper modHelper)
        {
            Translations.ModHelper = modHelper;
        }
    }
}
