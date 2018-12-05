namespace BL
{
    /// <summary>
    /// Get instance of BlImp
    /// </summary>
    public static class FactoryBl
    {
        private static BlImp _bl = null;
        /// <summary>
        /// Get the object
        /// </summary>
        public static BlImp GetObject => _bl ?? (_bl = new BlImp());
    }
}
