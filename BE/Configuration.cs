using System.IO;
using System.Reflection;

namespace BE
{
    /// <summary>
    ///     static configurations for the program
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        ///     google maps developers API key  : This key belongs to Elisha Mayer .Don't use it without permission !!
        ///     For details contact elisja.mayer@gmail.com.To get your own key go to http://g.co/dev/maps-no-account
        /// </summary>
        public const string Key = "AIzaSyB_L-QyNS6BHPMIvzcWQZBhunwpcr_wokU";

        /// <summary>
        ///     google distance url api
        /// </summary>
        public const string GoogleDistanceUrl = "https://maps.googleapis.com/maps/api/directions/";

        public const string TraineesXmlFilePath = @"Data\Trainees.xml";
        public const string TestersXmlFilePath = @"Data\Testers.xml";

        public const string ConfigXmlFilePath = @"Data\Config.xml";
        public const string TestsXmlFilePath = @"Data\Tests.xml";

        public const string FromEmailAddress = "tests.miniproject@gmail.com";
        public const string SenderPassword = "0586300016";
        public static string Theme = "Light";
        public static string Color = "Blue";


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

        ///// <summary>
        /////     The number of days that a tester works in default
        ///// </summary>
        //public static uint NumbersOfWorkDaysInWeekTesters = 5;

        /// <summary>
        ///     Minimum Criteria to fill in a test
        /// </summary>
        public static uint MinimumCriteria = 3;

        /// <summary>
        ///     percent of criteria in order to pass the test
        /// </summary>
        public static uint PercentOfCriteriaToPassTest = 70;

        /// <summary>
        ///     default language for google
        /// </summary>
        public static string GoogleLanguage = "en"; //en . for hebrew change to iw

        /// <summary>
        ///     Max test route duration
        /// </summary>
        public static int MaxTestDurationSec = 3000;

        /// <summary>
        ///     Min test route duration in seconds
        /// </summary>
        public static int MinTestDurationSec = 800;

        /// <summary>
        ///     Administrator user name
        /// </summary>
        public static string AdminUser = "Admin";

        /// <summary>
        ///     Administrator password
        /// </summary>
        public static string AdminPassword = "admin";

        /// <summary>
        ///     If the program is opened for the first time
        /// </summary>
        public static bool FirstOpenProgram = true;

        public static int MinStartHourWork = 9;
        public static int MaxEndHourWork = 15;


        public static string[] Criteria =
        {
            "Kept Distance",
            "Parking",
            "Reverse Parking",
            "Check Mirrors",
            "Used Signal",
            "kept Right of Presidency",
            "Stopped at Red",
            "Stopped At Cross Walk",
            "Right Turn",
            "Immediate Stop"
        };

        /// <summary>
        ///     return the path of the pdf license file for any computer
        /// </summary>
        /// <returns></returns>
        public static string GetPdfFullPath()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));
            var fileName = Path.Combine(path, "license.pdf");
            return fileName;
        }
    }
}