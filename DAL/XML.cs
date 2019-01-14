using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using BE;
using BE.MainObjects;
using BE.Routes;

namespace DAL
{
  public static class XML
    {

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
                    Id = uint.Parse(trainee.Element("id")?.Value ?? throw new InvalidOperationException()),
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
                        ReadyForTest = bool.Parse(item.Element("readyForTest")?.Value ?? throw new InvalidOperationException()),
                        NumberOfLessons = int.Parse(item.Element("numOfLessons")?.Value ?? throw new InvalidOperationException())
                    });
                }

                trainees.Add(t);
            }

          //  _traineeChanged = false;

            return trainees;
        }


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




    }
}
