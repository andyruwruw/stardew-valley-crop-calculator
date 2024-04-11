namespace CropCalculator.Enums
{
    /// <summary>
    /// States of transitions.
    /// </summary>
    public enum TransitionState
    {
        /// <summary>
        /// Entering a transition.
        /// </summary>
        Entering = 0,

        /// <summary>
        /// In a transition.
        /// </summary>
        Present = 1,

        /// <summary>
        /// Exiting a transition.
        /// </summary>
        Exiting = 2,

        /// <summary>
        /// Not transitioning.
        /// </summary>
        Dead = 3,
    }
}
