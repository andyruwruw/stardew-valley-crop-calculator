namespace CropCalculator.Enums
{
    /// <summary>
    /// Methods of displaying profits.
    /// </summary>
    public enum ProfitDisplay
    {
        /// <summary>
        /// The total and final profit.
        /// </summary>
        TotalProfit = 0,

        /// <summary>
        /// The total profit divided by time to achieve.
        /// </summary>
        DailyProfit = 1,

        /// <summary>
        /// The total return on investment.
        /// </summary>
        TotalReturnOnInvestment = 2,

        /// <summary>
        /// The daily return on investment.
        /// </summary>
        DailyReturnOnInvestment = 3,
    }
}
