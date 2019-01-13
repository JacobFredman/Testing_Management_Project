using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using BE;
using BE.MainObjects;
using BE.Routes;
using DS;

namespace DAL
{
    /// <summary>
    ///     Get Instance of DAL
    /// </summary>
    public static class FactoryDal
    {
        //todo: hell
        private static DalImp _dal;

        /// <summary>
        ///     Get the object
        /// </summary>
        public static DalImp GetObject => _dal ?? (_dal = new DalImp());
    }

    /// <inheritdoc />
    /// <summary>
    ///     DAL implantation
    /// </summary>
    public partial class DalImp : IDal
    {
        private XElement _traineesXml;
        //   private XElement _testersXml;
        private XElement _testersXML;
        private XElement _testsXML;


        private List<Trainee> _trainees=new List<Trainee>();
        private List<Test> _tests = new List<Test>();
        private List<Tester> _testers = new List<Tester>();

        private bool _traineeChanged = true;
        private bool _testerChanged = true;

        private XElement _config;
        /// <summary>
        ///     deny access to c-tor
        /// </summary>
        internal DalImp()
        {
            try
            {
                if (File.Exists(Configuration.SaveTraineesXmlPath))
                {
                    _traineesXml = XElement.Load(Configuration.SaveTraineesXmlPath);
                }
                else
                {
                    _traineesXml=new XElement("trainees");
                }
                GetAllTraineesXml();

                if (File.Exists(Configuration.SaveTestersXmlPath))
                {
                    _testersXML = XElement.Load(Configuration.TestersXmlPathFile);
                }
                else
                {
                    _testersXML = new XElement("testers");
                }
              _testers =  GetAllTestersXml();

                if (File.Exists(Configuration.SaveTestsXmlPath))
                {
                   // _tests = LoadFromXML<List<Test>>(Configuration.SaveTestsXmlPath);
                   _tests = LoadTestsFromXML();
                }
                else
                {
                    _tests = new List<Test>();
                }

                LoadConfigurations();
            }
            catch
            {
               //do nothing
            }
        }

     

        #region Tester

        /// <summary>
        ///     Add tester
        /// </summary>
        /// <param name="newTester"></param>
        //public void AddTester(Tester newTester)
        //{
        //    if (_testers.Any(tester => tester.Id == newTester.Id))
        //        throw new Exception("The tester already exist in the system");

        //    _testersXml.Add(TesterToXml(newTester));
        //    _testersXml.Save(Configuration.SaveTestersXmlPath);
        //    _testerChanged = true;
        //}

        ///// <summary>
        /////     Remove a tester
        ///// </summary>
        ///// <param name="testerToDelete"></param>
        //public void RemoveTester(Tester testerToDelete)
        //{
        //    if (_testers.All(x => x.Id != testerToDelete.Id))
        //        throw new Exception("Tester doesn't exist");

        //    _testersXml.Elements().First(x => x.Element("id").Value == testerToDelete.Id.ToString()).Remove();
        //    _testersXml.Save(Configuration.SaveTestersXmlPath);
        //    _testerChanged = true;
        //}

        ///// <summary>
        /////     update existing Tester
        ///// </summary>
        ///// <param name="updatedTester"></param>
        //public void UpdateTester(Tester updatedTester)
        //{
        //    if (_testers.All(x => x.Id != updatedTester.Id))
        //        throw new Exception("Tester doesn't exist");

        //    _testersXml.Elements().First(x => x.Element("id").Value == updatedTester.Id.ToString()).Remove();
        //    _testersXml.Add(TesterToXml(updatedTester));
        //    _testersXml.Save(Configuration.SaveTestersXmlPath);
        //    _testerChanged = true;

        //}

        #endregion

        #region Trainee

        /// <summary>
        ///     Add trainee
        /// </summary>
        /// <param name="newTrainee"></param>
        public void AddTrainee(Trainee newTrainee)
        {
            if (GetAllTraineesXml().Any(t => t.Id == newTrainee.Id))
                throw new Exception("The trainee already exist in the system");

            _traineesXml.Add(TraineeToXml(newTrainee));
            _traineesXml.Save(Configuration.SaveTraineesXmlPath);
            _traineeChanged = true;
        }

        /// <summary>
        ///     remove trainee
        /// </summary>
        /// <param name="traineeToDelete"></param>
        public void RemoveTrainee(Trainee traineeToDelete)
        {
            if (GetAllTraineesXml().All(x => x.Id != traineeToDelete.Id))
                throw new Exception("Trainee doesn't exist");


            _traineesXml.Elements().First(x=>x.Element("id").Value == traineeToDelete.Id.ToString()).Remove();
            _traineesXml.Save(Configuration.SaveTraineesXmlPath);
            _traineeChanged = true;
        }

        /// <summary>
        ///     update an existing trainee
        /// </summary>
        /// <param name="updatedTrainee"></param>
        public void UpdateTrainee(Trainee updatedTrainee)
        {
            if (GetAllTraineesXml().All(x => x.Id != updatedTrainee.Id))
                throw new Exception("Trainee doesn't exist");

            _traineesXml.Elements().First(x => x.Element("id").Value == updatedTrainee.Id.ToString()).Remove();
            _traineesXml.Add(TraineeToXml(updatedTrainee));
            _traineesXml.Save(Configuration.SaveTraineesXmlPath);
            _traineeChanged = true;
        }

        #endregion

        #region Return lists

        /// <summary>
        ///     return a copy of all testers
        /// </summary>
        public IEnumerable<Tester> AllTesters
        {
            get
            {
                var allTesters = GetAllTestersFromXml().Select(item => item.Clone() as Tester).ToList();
                return allTesters.OrderBy(x => x.Id);
            }
        }

        /// <summary>
        ///     return a copy of all tests
        /// </summary>
        public IEnumerable<Test> AllTests
        {
            get
            {
                var allTest = new List<Test>();
                foreach (var item in DeSerializeTestFromXml())
                    allTest.Add(item.Clone() as Test);
                return allTest.OrderBy(x => x.Id);
            }
        }

        /// <summary>
        ///     return a copy of all trainees
        /// </summary>
        public IEnumerable<Trainee> AllTrainee
        {
            get
            {
                var allTrainee = new List<Trainee>();
                foreach (var item in GetAllTraineesXml())
                    allTrainee.Add(item.Clone() as Trainee);
                return allTrainee.OrderBy(x => x.Id);
                ;
            }
        }

        #endregion

        #region Trainee XML

        private XElement TraineeToXml(Trainee trainee)
        {
            var id = new XElement("id", trainee.Id);
            var firstName = new XElement("firstName", trainee.FirstName);
            var lastName = new XElement("lastName", trainee.LastName);
            var gender = new XElement("gender", trainee.Gender);
            var address = new XElement("address", trainee.Address);
            var schoolName = new XElement("schoolName", trainee.SchoolName);
            var teacherName = new XElement("teacherName", trainee.TeacherName);
            var birthDate = new XElement("birthDate", trainee.BirthDate);
            var emailAddress = new XElement("emailAddress", trainee.EmailAddress);
            var phoneNum = new XElement("phoneNum", trainee.PhoneNumber);

            var collectionLicenseTypeLearning = new XElement("CollectionLicenseTypeLearning");

            foreach (var item in trainee.LicenseTypeLearning)
            {
                var gearType = new XElement("gearType", item.GearType);
                var license = new XElement("license", item.License);
                var numOfLessons = new XElement("numOfLessons", item.NumberOfLessons);
                var readyForTest = new XElement("readyForTest", item.ReadyForTest);
                var licenseTypeLearning = new XElement("licenseTypeLearning", gearType, license, numOfLessons, readyForTest);
                collectionLicenseTypeLearning.Add(licenseTypeLearning);
            }
            return new XElement("trainee", id, firstName, lastName, gender, address, birthDate, emailAddress, phoneNum, teacherName, schoolName, collectionLicenseTypeLearning);

        }

        private List<Trainee> GetAllTraineesXml()
        {
            if (_traineeChanged)
            {
                _trainees = new List<Trainee>();
                foreach (var trainee in _traineesXml.Elements())
                {
                    var t = new Trainee()
                    {
                        Id = uint.Parse(trainee.Element("id")?.Value),
                        FirstName = trainee.Element("firstName")?.Value,
                        LastName = trainee.Element("lastName")?.Value,
                        BirthDate = DateTime.Parse(trainee.Element("birthDate")?.Value),
                        Address = new Address(trainee.Element("address")?.Value),
                        EmailAddress = trainee.Element("emailAddress")?.Value,
                        PhoneNumber = trainee.Element("phoneNum")?.Value,
                        Gender = (Gender)Enum.Parse(typeof(Gender), trainee.Element("gender")?.Value),
                        SchoolName = trainee.Element("schoolName")?.Value,
                        TeacherName = trainee.Element("teacherName")?.Value,
                        LicenseTypeLearning = new List<LessonsAndType>(),
                        LicenseType = new List<LicenseType>()
                    };
                    foreach (var item in trainee.Element("CollectionLicenseTypeLearning").Elements())
                    {
                        t.LicenseTypeLearning.Add(new LessonsAndType()
                        {
                            GearType = (Gear)Enum.Parse(typeof(Gear), item.Element("gearType")?.Value),
                            License = (LicenseType)Enum.Parse(typeof(LicenseType), item.Element("license")?.Value),
                            ReadyForTest = bool.Parse(item.Element("readyForTest")?.Value),
                            NumberOfLessons = int.Parse(item.Element("numOfLessons")?.Value)
                        });
                    }

                    _trainees.Add(t);
                }

                _traineeChanged = false;
            }

            return _trainees;
        }

        #endregion

        #region Tester XML



        private IEnumerable<Tester> GetAllTestersFromXml()
        {
            var testers = new List<Tester>();
            foreach (var xmLTester in _testersXML.Elements())
            {
                var licensesTypes = new List<LicenseType>();
                licensesTypes.AddRange(xmLTester.Element("CollectionLicenseType")
                                           ?.Elements()
                                           .Select(
                                               item => (LicenseType)Enum.Parse(typeof(LicenseType), item.Value
                                               )) ?? throw new InvalidOperationException());

                var licensesTypesTeaching = new List<LicenseType>();
                licensesTypesTeaching.AddRange(xmLTester.Element("CollectionLicenseTypeTeaching")
                                                   ?.Elements()
                                                   .Select(
                                                       item => (LicenseType)Enum.Parse(typeof(LicenseType), item.Value
                                                       )) ?? throw new InvalidOperationException());


                var schedule = new WeekSchedule();
                foreach (var day in xmLTester.Element("schedule")?.Elements())
                {
                    var theDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day.Name.ToString());
                    var i = 0;
                    foreach (var hour in day.Elements())
                    {
                        schedule[theDay].Hours[i] = bool.Parse(hour?.Value);
                        i++;
                    }
                }


                var tester = new Tester
                {
                    BirthDate = DateTime.Parse(xmLTester.Element("birthDate")?.Value),
                    Address = new Address(xmLTester.Element("address")?.Value),
                    EmailAddress = xmLTester.Element("emailAddress")?.Value,
                    Experience = uint.Parse(xmLTester.Element("experience")?.Value ?? throw new InvalidOperationException()),
                    Gender = (Gender)Enum.Parse(typeof(Gender), xmLTester.Element("gender")?.Value ?? throw new InvalidOperationException()),
                    FirstName = xmLTester.Element("firstName")?.Value,
                    Id = uint.Parse(xmLTester.Element("id")?.Value ?? throw new InvalidOperationException()),
                    LastName = xmLTester.Element("lastName")?.Value,


                    LicenseType = licensesTypes,
                    LicenseTypeTeaching = licensesTypesTeaching,

                    MaxDistance = float.Parse(xmLTester.Element("maxDistance")?.Value ?? throw new InvalidOperationException()),
                    Schedule = schedule,
                    MaxWeekExams = uint.Parse(xmLTester.Element("maxWeekExams")?.Value ?? throw new InvalidOperationException()),
                    PhoneNumber = xmLTester.Element("phoneNum")?.Value
                };
                testers.Add(tester);
            }

            return testers;
        }


        private static XElement TesterToXmlElement(Tester tester)
        {
            var experience = new XElement("experience", tester.Experience.ToString());

            var licenseTypeTeaching = new XElement("CollectionLicenseTypeTeaching");
            foreach (var license in tester.LicenseTypeTeaching)
            {
                var xmllicense = new XElement("license", license);
                licenseTypeTeaching.Add(xmllicense);
            }

            var maxDistance = new XElement("maxDistance", tester.MaxDistance.ToString(CultureInfo.CurrentCulture));
            var maxWeekExams = new XElement("maxWeekExams", tester.MaxWeekExams.ToString());
            var id = new XElement("id", tester.Id.ToString());

            var address = new XElement("address", tester.Address);

            //  Use the ISO 8601 format - "O" format specifier
            var birthDate = new XElement("birthDate", tester.BirthDate);
            var emailAddress = new XElement("emailAddress", tester.EmailAddress);
            var firstName = new XElement("firstName", tester.FirstName);
            var gender = new XElement("gender", tester.Gender.ToString());
            var lastName = new XElement("lastName", tester.LastName);


            var licenseType = new XElement("CollectionLicenseType");
            if (!tester.LicenseType.Any())
                foreach (var license in tester.LicenseType)
                {
                    var xmllicense = new XElement("license", license.ToString());
                    licenseType.Add(xmllicense);
                }


            var schedule = new XElement("schedule");
            foreach (var day in tester.Schedule.days)
            {
                var dayOfWeek = new XElement(day.TheDay.ToString(), day.TheDay);
              //  int counter = 0;
                foreach (var hour in day.Hours)
                {
                    var hourInDay = new XElement("hour", hour);
                   // if(counter == 13 || counter == 14 || counter == 15 || counter == 16)
                      //  hourInDay.SetValue("true");
                    dayOfWeek.Add(hourInDay);
                 //   counter++;
                }

                schedule.Add(dayOfWeek);
            }


            var phoneNumber = new XElement("phoneNum", tester.PhoneNumber);


            return new XElement("tester", experience, licenseTypeTeaching, maxDistance, maxWeekExams, schedule, id,
                address, birthDate, emailAddress, firstName, gender, lastName, licenseType, phoneNumber);
        }

        //private XElement TesterToXml(Tester tester)
        //{
        //    var id = new XElement("id", tester.Id);
        //    var firstName = new XElement("firstName", tester.FirstName);
        //    var lastName = new XElement("lastName", tester.LastName);
        //    var gender = new XElement("gender", tester.Gender);
        //    var address = new XElement("address", tester.Address);
        //    var experience = new XElement("experience", tester.Experience);
        //    var maxDistance = new XElement("maxDistance", tester.MaxDistance);
        //    var maxWeekExams = new XElement("maxWeekExams", tester.MaxWeekExams);
        //    var birthDate = new XElement("birthDate", tester.BirthDate);
        //    var emailAddress = new XElement("emailAddress", tester.EmailAddress);
        //    var phoneNum = new XElement("phoneNum", tester.PhoneNumber);

        //    var collectionLicenseTypeTeaching = new XElement("CollectionLicenseTypeTeaching");

        //    foreach (var item in tester.LicenseTypeTeaching)
        //    {
        //        var license = new XElement("license", item);

        //        collectionLicenseTypeTeaching.Add(license);
        //    }

        //    var schedule = new XElement("schedule");
        //    foreach (var day in tester.Schedule.days)
        //    {
        //        var dayOfWeek = new XElement(day.TheDay.ToString(), day.TheDay);
        //        foreach (var hour in day.Hours)
        //        {
        //            var hourInDay = new XElement("hour", hour);
        //            dayOfWeek.Add(hourInDay);
        //        }
        //        schedule.Add(dayOfWeek);
        //    }

        //    return new XElement("tester", id, firstName, lastName, gender, address, maxWeekExams, birthDate, emailAddress, maxDistance, phoneNum, schedule, experience, collectionLicenseTypeTeaching);

        //}

        private List<Tester> GetAllTestersXml()
        {
            if (_testerChanged)
            {
                _testers = new List<Tester>();
                foreach (var tester in _testersXML.Elements())
                {
                    var t = new Tester()
                    {
                        Id = uint.Parse(tester.Element("id")?.Value),
                        FirstName = tester.Element("firstName")?.Value,
                        LastName = tester.Element("lastName")?.Value,
                        BirthDate = DateTime.Parse(tester.Element("birthDate")?.Value),
                        Address = new Address(tester.Element("address")?.Value),
                        EmailAddress = tester.Element("emailAddress")?.Value,
                        PhoneNumber = tester.Element("phoneNum")?.Value,
                        Gender = (Gender)Enum.Parse(typeof(Gender), tester.Element("gender")?.Value),
                        MaxWeekExams = uint.Parse(tester.Element("maxWeekExams")?.Value),
                        MaxDistance = uint.Parse(tester.Element("maxDistance")?.Value),
                        Experience = uint.Parse(tester.Element("experience")?.Value),
                        LicenseTypeTeaching = new List<LicenseType>(),
                        LicenseType = new List<LicenseType>()
                    };
                    foreach (var item in tester.Element("CollectionLicenseTypeTeaching").Elements())
                    {
                        t.LicenseTypeTeaching.Add((LicenseType)Enum.Parse(typeof(LicenseType),
                            item?.Value));
                    }

                    t.Schedule = new WeekSchedule();
                    foreach (var day in tester.Element("schedule")?.Elements())
                    {
                        var theDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day.Name.ToString());
                        int i = 0;
                        foreach (var hour in day.Elements())
                        {
                            t.Schedule[theDay].Hours[i] = bool.Parse(hour?.Value);
                            i++;
                        }

                    }


                    _testers.Add(t);
                }

                _testerChanged = false;
            }

            return _testers;
        }

        #endregion

        #region Conf XML

        public void SaveConfigurations()
        {
            var adminPass = new XElement("AdminPassword", Configuration.AdminPassword);
            var theme = new XElement("Theme", Configuration.Theme);
            var color = new XElement("Color", Configuration.Color);
            var adminUser = new XElement("AdminUser", Configuration.AdminUser);
            var firstOpen = new XElement("FirstOpenProgram", Configuration.FirstOpenProgram);
            var testId = new XElement("TestId", Configuration.TestId);
            var minLesson = new XElement("MinLessons", Configuration.MinLessons);
            var minTesterAge = new XElement("MinTesterAge", Configuration.MinTesterAge);
            var minTraineeAge = new XElement("MinTraineeAge", Configuration.MinTraineeAge);
            var minTimeBetweenTests = new XElement("MinTimeBetweenTests", Configuration.MinTimeBetweenTests);
            var minimumCriteria = new XElement("MinimumCriteria", Configuration.MinimumCriteria);
            var percentOfCriteriaToPassTest =
                new XElement("PercentOfCriteriaToPassTest", Configuration.PercentOfCriteriaToPassTest);

            var criteria=new XElement("Criteria");
            foreach (var item in Configuration.Criteria)
            {
                criteria.Add(new XElement("Criterion",item));
            }
            _config.RemoveAll();
            _config.Add(adminPass, adminUser, firstOpen, testId, minLesson, minTesterAge, minTimeBetweenTests,
                minTraineeAge, minimumCriteria, percentOfCriteriaToPassTest,theme,color,criteria);
            _config.Save(Configuration.SaveConfigXmlPath);
        }

        public void LoadConfigurations()
        {
            if (File.Exists(Configuration.SaveConfigXmlPath))
            {
                _config=XElement.Load(Configuration.SaveConfigXmlPath);
                Configuration.Theme = _config.Element("Theme")?.Value;
                Configuration.Color = _config.Element("Color")?.Value;
                Configuration.AdminPassword = _config.Element("AdminPassword")?.Value;
                Configuration.AdminUser = _config.Element("AdminUser")?.Value;
                Configuration.FirstOpenProgram = bool.Parse(_config.Element("FirstOpenProgram")?.Value);
                Configuration.TestId = uint.Parse(_config.Element("TestId")?.Value);
                Configuration.MinLessons = uint.Parse(_config.Element("MinLessons")?.Value);
                Configuration.MinTesterAge= uint.Parse(_config.Element("MinTesterAge")?.Value);
                Configuration.MinTraineeAge= uint.Parse(_config.Element("MinTraineeAge")?.Value);
                Configuration.MinTimeBetweenTests= uint.Parse(_config.Element("MinTimeBetweenTests")?.Value);
                Configuration.MinimumCriteria= uint.Parse(_config.Element("MinimumCriteria")?.Value);
                Configuration.PercentOfCriteriaToPassTest= uint.Parse(_config.Element("PercentOfCriteriaToPassTest")?.Value);
                Configuration.Criteria = (from item in _config?.Element("Criteria").Elements()
                    select item.Value).ToArray();
            }
            else
            {
                _config=new XElement("Config");
            }
        }
 
        #endregion

        #region Generic Serializer

        private static void SaveToXML<T>(T source, string path)
        {
            var file = new FileStream(path, FileMode.Create);
            var xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source); file.Close();
        }

        private static T LoadFromXML<T>(string path)
        {
            var file = new FileStream(path, FileMode.Open);
            var xmlSerializer = new XmlSerializer(typeof(T));
            var result = (T)xmlSerializer.Deserialize(file); file.Close();
            return result;
        }

        #endregion











        
  

        #region Test

        


        #endregion

        #region Tester

        /// <summary>
        ///     Add tester
        /// </summary>
        /// <param name="newTester"></param>
        public void AddTester(Tester newTester)
        {
           if (GetAllTestersFromXml().Any(tester => tester.Id == newTester.Id))
                throw new Exception("The tester already exist in the system");

            _testersXML.Add( TesterToXmlElement(newTester));
            _testersXML.Save(Configuration.TestersXmlPathFile);
        }

        /// <summary>
        ///     Remove a tester
        /// </summary>
        /// <param name="testerToDelete"></param>
        public void RemoveTester(Tester testerToDelete)
        {
            if (GetAllTestersFromXml().All(x => x.Id != testerToDelete.Id))
                throw new Exception("Tester doesn't exist");

            _testersXML.Elements().First(x => x.Element("id")?.Value == testerToDelete.Id.ToString()).Remove();
            _testersXML.Save(Configuration.TestersXmlPathFile);
            _testerChanged = true;
        }

        /// <summary>
        ///     update existing Tester
        /// </summary>
        /// <param name="testerToUpdate"></param>
        public void UpdateTester(Tester testerToUpdate)
        {
            if (GetAllTestersFromXml().All(x => x.Id != testerToUpdate.Id))
                throw new Exception("Trainee doesn't exist");

            _testersXML.Elements().First(x => x.Element("id")?.Value == testerToUpdate.Id.ToString()).Remove();
            _testersXML.Add(TesterToXmlElement(testerToUpdate));
            _testersXML.Save(Configuration.TestersXmlPathFile);
            _testerChanged = true;
        }

        public static List<Test> LoadTestsFromXML()
        {
            var file = new FileStream(Configuration.TestsXmlPathFile, FileMode.Open);
            var xmlSerializer = new XmlSerializer(typeof(List<Test>));
            var testsList = (List<Test>)xmlSerializer.Deserialize(file);
            file.Close();
            return testsList;
        }


      
    



        #endregion



















    }
}