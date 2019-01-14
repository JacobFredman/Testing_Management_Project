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

namespace DAL
{
  public static class XML
    {

        #region testXML

        public static void SerializeTestsToXml(IReadOnlyCollection<Test> list)
        {
            var file = new FileStream(Configuration.TestsXmlFilePath, FileMode.Create);
            var xmlSerializer = new XmlSerializer(list.GetType());
            xmlSerializer.Serialize(file, list);
            file.Close();
        }


        public static IEnumerable<Test> DeSerializeTestFromXml()
        {
            var file = new FileStream(Configuration.TestsXmlFilePath, FileMode.Open);
            var xmlSerializer = new XmlSerializer(typeof(List<Test>));
            var list = (List<Test>)xmlSerializer.Deserialize(file);
            file.Close();
            return list;
        }


        #endregion

        #region TreaineeXml

        public static XElement ConvertTraineeToXml(Trainee trainee)
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



        public static IEnumerable<Trainee> GetAllTraineesFromXml(XElement traineesXml)
        {
            var trainees = new List<Trainee>();
            //   if (!_traineeChanged) return trainees;
            foreach (var trainee in traineesXml.Elements())
            {
                var t = new Trainee
                {
                    Id = UInt32.Parse(trainee.Element("id")?.Value ?? throw new InvalidOperationException()),
                    FirstName = trainee.Element("firstName")?.Value,
                    LastName = trainee.Element("lastName")?.Value,
                    BirthDate = DateTime.Parse(trainee.Element("birthDate")?.Value),
                    Address = new Address(trainee.Element("address")?.Value),
                    EmailAddress = trainee.Element("emailAddress")?.Value,
                    PhoneNumber = trainee.Element("phoneNum")?.Value,
                    Gender = (Gender)Enum.Parse(typeof(Gender), trainee.Element("gender")?.Value ?? throw new InvalidOperationException()),
                    SchoolName = trainee.Element("schoolName")?.Value,
                    TeacherName = trainee.Element("teacherName")?.Value,
                    LicenseTypeLearning = new List<TrainingDetails>(),
                    LicenseType = new List<LicenseType>()
                };
                foreach (var item in trainee.Element("CollectionLicenseTypeLearning")?.Elements())
                {
                    t.LicenseTypeLearning.Add(new TrainingDetails()
                    {
                        GearType = (Gear)Enum.Parse(typeof(Gear), item.Element("gearType")?.Value ?? throw new InvalidOperationException()),
                        License = (LicenseType)Enum.Parse(typeof(LicenseType), item.Element("license")?.Value ?? throw new InvalidOperationException()),
                        ReadyForTest = Boolean.Parse(item.Element("readyForTest")?.Value ?? throw new InvalidOperationException()),
                        NumberOfLessons = Int32.Parse(item.Element("numOfLessons")?.Value ?? throw new InvalidOperationException())
                    });
                }

                trainees.Add(t);
            }

            //  _traineeChanged = false;

            return trainees;
        }


        #endregion

        #region TestersXML


        public static IEnumerable<Tester> GetAllTestersFromXml(XElement testersXml)
        {
            //if (!_testerChanged)
            //    return _testers;
            var testers = new List<Tester>();
            foreach (var xmLTester in testersXml.Elements())
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
                        schedule[theDay].Hours[i] = Boolean.Parse(hour?.Value);
                        i++;
                    }
                }


                var tester = new Tester
                {
                    BirthDate = DateTime.Parse(xmLTester.Element("birthDate")?.Value),
                    Address = new Address(xmLTester.Element("address")?.Value),
                    EmailAddress = xmLTester.Element("emailAddress")?.Value,
                    Experience = UInt32.Parse(xmLTester.Element("experience")?.Value ?? throw new InvalidOperationException()),
                    Gender = (Gender)Enum.Parse(typeof(Gender), xmLTester.Element("gender")?.Value ?? throw new InvalidOperationException()),
                    FirstName = xmLTester.Element("firstName")?.Value,
                    Id = UInt32.Parse(xmLTester.Element("id")?.Value ?? throw new InvalidOperationException()),
                    LastName = xmLTester.Element("lastName")?.Value,


                    LicenseType = licensesTypes,
                    LicenseTypeTeaching = licensesTypesTeaching,

                    MaxDistance = Single.Parse(xmLTester.Element("maxDistance")?.Value ?? throw new InvalidOperationException()),
                    Schedule = schedule,
                    MaxWeekExams = UInt32.Parse(xmLTester.Element("maxWeekExams")?.Value ?? throw new InvalidOperationException()),
                    PhoneNumber = xmLTester.Element("phoneNum")?.Value

                };
                testers.Add(tester);
            }

            //  _testerChanged = false;
            return testers;
        }




        public static XElement TesterToXmlElement(Tester tester)
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
                foreach (var hour in day.Hours)
                {
                    var hourInDay = new XElement("hour", hour);
                    dayOfWeek.Add(hourInDay);
                }

                schedule.Add(dayOfWeek);
            }


            var phoneNumber = new XElement("phoneNum", tester.PhoneNumber);


            return new XElement("tester", experience, licenseTypeTeaching, maxDistance, maxWeekExams, schedule, id,
                address, birthDate, emailAddress, firstName, gender, lastName, licenseType, phoneNumber);
        }



        #endregion

        #region ConfigurationsXMl

        public static XElement LoadConfigurations()
        {
            XElement config;
            if (File.Exists(Configuration.ConfigXmlFilePath))
            {
                config = XElement.Load(Configuration.ConfigXmlFilePath);
                Configuration.Theme = config.Element("Theme")?.Value;
                Configuration.Color = config.Element("Color")?.Value;
                Configuration.AdminPassword = config.Element("AdminPassword")?.Value;
                Configuration.AdminUser = config.Element("AdminUser")?.Value;
                Configuration.FirstOpenProgram = Boolean.Parse(config.Element("FirstOpenProgram")?.Value);
                Configuration.TestId = UInt32.Parse(config.Element("TestId")?.Value);
                Configuration.MinLessons = UInt32.Parse(config.Element("MinLessons")?.Value);
                Configuration.MinTesterAge = UInt32.Parse(config.Element("MinTesterAge")?.Value);
                Configuration.MinTraineeAge = UInt32.Parse(config.Element("MinTraineeAge")?.Value);
                Configuration.MinTimeBetweenTests = UInt32.Parse(config.Element("MinTimeBetweenTests")?.Value);
                Configuration.MinimumCriteria = UInt32.Parse(config.Element("MinimumCriteria")?.Value);
                Configuration.PercentOfCriteriaToPassTest = UInt32.Parse(config.Element("PercentOfCriteriaToPassTest")?.Value);
                Configuration.Criteria = (from item in config?.Element("Criteria").Elements()
                                          select item.Value).ToArray();
            }
            else
            {
                config = new XElement("Config");
            }

            return config;
        }




        public static void SaveConfigurations(XElement config)
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

            var criteria = new XElement("Criteria");
            foreach (var item in Configuration.Criteria)
            {
                criteria.Add(new XElement("Criterion", item));
            }
            config.RemoveAll();
            config.Add(adminPass, adminUser, firstOpen, testId, minLesson, minTesterAge, minTimeBetweenTests,
                minTraineeAge, minimumCriteria, percentOfCriteriaToPassTest, theme, color, criteria);
            config.Save(Configuration.ConfigXmlFilePath);
        }


        #endregion

    }
}
