namespace BE
{
    /// <summary>
    ///     Gender
    /// </summary>
    public enum Gender
    {
        /// <summary>
        ///     Male
        /// </summary>
        Male,

        /// <summary>
        ///     Female
        /// </summary>
        Female
    }

    /// <summary>
    ///     License types
    /// </summary>
    public enum LicenseType
    {
        /// <summary>
        ///     Car license
        /// </summary>
        B,

        /// <summary>
        ///     Motorcycle license until 125
        /// </summary>
        A2,

        /// <summary>
        ///     Motorcycle license until 500
        /// </summary>
        A1,

        /// <summary>
        ///     Motorcycle license
        /// </summary>
        A,

        /// <summary>
        ///     Truck until 12 ton
        /// </summary>
        C1,

        /// <summary>
        ///     Truck
        /// </summary>
        C,

        /// <summary>
        ///     Bus
        /// </summary>
        D,

        /// <summary>
        ///     Public transport until 5 ton and 16 passengers
        /// </summary>
        D1,

        /// <summary>
        ///     Public bus until 5 ton
        /// </summary>
        D2,

        /// <summary>
        ///     Private bus until 5 ton
        /// </summary>
        D3,

        /// <summary>
        ///     Truck + dragged
        /// </summary>
        E,

        /// <summary>
        ///     Tractor
        /// </summary>
        _1
    }

    /// <summary>
    ///     Vehicle gear
    /// </summary>
    public enum Gear
    {
        /// <summary>
        ///     Automatic gear
        /// </summary>
        Automatic,

        /// <summary>
        ///     Manual gear
        /// </summary>
        Manual
    }
}