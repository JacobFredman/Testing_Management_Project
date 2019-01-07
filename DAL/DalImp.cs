using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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
    public class DalImp : IDal
    {
        private XElement _traineesXml;
        private List<Trainee> _trainees=new List<Trainee>();
        private bool _traineeChanged = true;
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

                LoadConfigurations();
            }
            catch(Exception e)
            {
               
            }
        }

        #region Test

        /// <summary>
        ///     Add a new test
        /// </summary>
        /// <param name="newTest"></param>
        public void AddTest(Test newTest)
        {
            newTest.Id = $"{Configuration.TestId:00000000}";
            Configuration.TestId++;
            SaveConfigurations();
            DataSource.Tests.Add(newTest);
        }

        /// <summary>
        ///     remove a test
        /// </summary>
        /// <param name="testToDelete"></param>
        public void RemoveTest(Test testToDelete)
        {
            if (DataSource.Tests.All(x => x.Id != testToDelete.Id))
                throw new Exception("Test doesn't exist");

            DataSource.Tests.RemoveAll(x => x.Id == testToDelete.Id);
        }

        /// <summary>
        ///     update an existing test
        /// </summary>
        /// <param name="updatedTest"></param>
        public void UpdateTest(Test updatedTest)
        {
            if (DataSource.Tests.All(x => x.Id != updatedTest.Id))
                throw new Exception("Test doesn't exist");

            var test = DataSource.Tests.Find(t => t.Id == updatedTest.Id);
            DataSource.Tests.Remove(test);
            DataSource.Tests.Add(updatedTest);
        }

        #endregion

        #region Tester

        /// <summary>
        ///     Add tester
        /// </summary>
        /// <param name="newTester"></param>
        public void AddTester(Tester newTester)
        {
            if (DataSource.Testers.Any(tester => tester.Id == newTester.Id))
                throw new Exception("The tester already exist in the system");

            DataSource.Testers.Add(newTester);
        }

        /// <summary>
        ///     Remove a tester
        /// </summary>
        /// <param name="testerToDelete"></param>
        public void RemoveTester(Tester testerToDelete)
        {
            if (DataSource.Testers.All(x => x.Id != testerToDelete.Id))
                throw new Exception("Tester doesn't exist");

            DataSource.Testers.RemoveAll(x => x.Id == testerToDelete.Id);
        }

        /// <summary>
        ///     update existing Tester
        /// </summary>
        /// <param name="updatedTester"></param>
        public void UpdateTester(Tester updatedTester)
        {
            if (DataSource.Testers.All(x => x.Id != updatedTester.Id))
                throw new Exception("Tester doesn't exist");

            var tester = DataSource.Testers.Find(t => t.Id == updatedTester.Id);
            DataSource.Testers.Remove(tester);
            DataSource.Testers.Add(updatedTester);
        }

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
                var allTesters = new List<Tester>();
                foreach (var item in DataSource.Testers)
                    allTesters.Add(item.Clone() as Tester);
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
                foreach (var item in DataSource.Tests)
                    allTest.Add(item.Clone() as Test);
                return allTest.OrderBy(x => x.Id);
                ;
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

        private XElement TraineeToXml(Trainee trainee)
        {
            var id=new XElement("id",trainee.Id);
            var firstName=new XElement("firstName",trainee.FirstName);
            var lastName = new XElement("lastName", trainee.LastName);
            var gender=new XElement("gender",trainee.Gender);
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
                var licenseTypeLearning = new XElement("licenseTypeLearning", gearType,license,numOfLessons,readyForTest);
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
                        Gender = (Gender) Enum.Parse(typeof(Gender), trainee.Element("gender")?.Value),
                        SchoolName = trainee.Element("schoolName")?.Value,
                        TeacherName = trainee.Element("teacherName")?.Value,
                        LicenseTypeLearning = new List<LessonsAndType>(),
                        LicenseType = new List<LicenseType>()
                    };
                    foreach (var item in trainee.Element("CollectionLicenseTypeLearning").Elements())
                    {
                        t.LicenseTypeLearning.Add(new LessonsAndType()
                        {
                            GearType = (Gear) Enum.Parse(typeof(Gear), item.Element("gearType")?.Value),
                            License = (LicenseType) Enum.Parse(typeof(LicenseType), item.Element("license")?.Value),
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
    }
}