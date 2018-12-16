namespace BE
{
    /// <summary>
    ///     static configurations for the program
    /// </summary>
    public class Configuration
    {
        /// <summary>
        ///     Id for tests
        /// </summary>
        public static uint TestId = 1;

        /// <summary>
        ///     Minimum lessons to take a test
        /// </summary>
        public static uint MinLessons = 16;

        /// <summary>
        ///     Minimum tester age
        /// </summary>
        public static uint MinTesterAge = 40;

        /// <summary>
        ///     Minimum trainee age
        /// </summary>
        public static uint MinTraineeAge = 17;

        /// <summary>
        ///     Minimum days between tests
        /// </summary>
        public static uint MinTimeBetweenTests = 14;

        /// <summary>
        ///     The number of days that a tester works in default
        /// </summary>
        public static uint NumbersOfWorkDaysInWeekTesters = 5;

        /// <summary>
        ///     Minimum criterion to fill in a test
        /// </summary>
        public static uint MinimumCriterions = 3;

        /// <summary>
        ///     percent of criterions in order to pass the test
        /// </summary>
        public static uint PercentOfCritirionsToPassTest = 70;

        /// <summary>
        ///     google maps developers API key  : This key belongs to Elisha Mayer .Don't use it without permision !!
        ///     For details contact elisja.mayer@gmail.com.To get your own key go to http://g.co/dev/maps-no-account
        /// </summary>
        public static string Key = "AIzaSyB_L-QyNS6BHPMIvzcWQZBhunwpcr_wokU";

        /// <summary>
        ///     google distance url api
        /// </summary>
        public static string GoogleDistanceUrl = "https://maps.googleapis.com/maps/api/directions/";

        /// <summary>
        ///     default language for google
        /// </summary>
        public static string GoogleLanguage = "en"; //en . for hebrew change to iw

        /// <summary>
        ///     Max test route duration
        /// </summary>
        public static int MaxTestDurationSec = 3000;

        /// <summary>
        ///     Min test route duration
        /// </summary>
        public static int MinTestDurationSec = 800;
    }
}