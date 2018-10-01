namespace Utilities
{
    /// <summary>
    /// Возрастная группа.
    /// </summary>
    public enum AgeGroupType
    {
        /// <summary>
        /// До 3 лет.
        /// </summary>
        [Description("до 3-х лет")]
        BeforeThree = 1,
        /// <summary>
        /// От 3 до 5 лет.
        /// </summary>
        [Description("3-х-5 лет")]
        BetweenThreeAndFive = 2,
        /// <summary>
        /// Старше 5 лет.
        /// </summary>
        [Description("5-7 лет")]
        UpperFive = 3
    }
}
