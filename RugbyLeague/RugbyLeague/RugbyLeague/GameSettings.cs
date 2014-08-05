namespace RugbyLeague
{
    public class GameSettings
    {
        #region Fields

        public string Name;
        public float DefaultAmount;
        public float MinAmount;
        public float MaxAmount;
        public float Increment;
        public float GameValue;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gameValue"></param>
        /// <param name="defaultAmount"></param>
        /// <param name="minAmount"></param>
        /// <param name="maxAmount"></param>
        /// <param name="increment"></param>
        public GameSettings(string name,
                            float gameValue,
                            float defaultAmount,
                            float minAmount,
                            float maxAmount,
                            float increment)
        {
            Name = name;
            DefaultAmount = defaultAmount;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            Increment = increment;
            GameValue = gameValue;

        }
    }
}
