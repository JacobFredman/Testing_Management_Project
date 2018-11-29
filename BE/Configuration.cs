namespace BE
{
    /// <summary>
    /// static configurations for the program
    /// </summary>
   public  class Configuration
    {
        public static uint TestID = 1;
        public static uint MinLessons;
        public static uint MinTesterAge;
        public static uint MinTraineeAge;
        public static uint MinTimeBetweenTests;
        public static uint NumbersOfWorkDaysInWeekTesters = 5; //for testers
        public static uint MinimumCritirions = 3;
        public static uint PersentOfCritirionsToPassTest = 70;
        /*google maps developers API key  : This key belongs to Elisha Mayer .Don't use it without permision !! 
        For details contact elisja.mayer@gmail.com . To get your own key go to http://g.co/dev/maps-no-account */
        public static string Key = "AIzaSyB_L-QyNS6BHPMIvzcWQZBhunwpcr_wokU";   
        public static string GoogleDistanceURL = "https://maps.googleapis.com/maps/api/directions/json?";
    }
}
