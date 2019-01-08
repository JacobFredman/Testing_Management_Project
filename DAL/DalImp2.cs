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
using DS;


namespace DAL
{
    partial class DalImp
    {
        private XElement _testersXML;
        private XElement _testsXML;

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

        public static void SerializeTestsToXml(List<Test> list, string path)
        {
            var file = new FileStream(Configuration.TestsXmlPathFile,FileMode.Create);
            var xmlSerializer = new XmlSerializer(list.GetType());
            xmlSerializer.Serialize(file,list);
            file.Close();
        }

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

            _testersXML = TesterToXmlElement(newTester);
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
        }


        private static XElement TesterToXmlElement(Tester tester)
        {
            var experience = new XElement("experience", tester.Experience.ToString());

            var licenseTypeTeaching = new XElement("licenseTypeTeaching");
            foreach (var item in tester.LicenseTypeTeaching)
                licenseTypeTeaching.Add(item.ToString());

            var maxDistance = new XElement("maxDistance", tester.MaxDistance.ToString(CultureInfo.CurrentCulture));
            var maxWeekExams = new XElement("maxWeekExams", tester.MaxWeekExams.ToString());
            var schedule = new XElement("schedule", tester.Schedule); // TODO implementation
            var id = new XElement("id", tester.Id.ToString());

            // address
            var address = new XElement("address");
            var building = new XElement("building", tester.Address.Building);
            var city = new XElement("city", tester.Address.City);
            var entrance = new XElement("entrance", tester.Address.Entrance);
            var street = new XElement("street", tester.Address.Street);
            address.Add(building, city, entrance, street);


            //  Use the ISO 8601 format - "O" format specifier
            var birthDate = new XElement("birthDate", tester.BirthDate.ToString("O"));
            var emailAddress = new XElement("emailAddress", tester.EmailAddress);
            var firstName = new XElement("firstName", tester.FirstName);
            var gender = new XElement("gender", tester.Gender.ToString());
            var lastName = new XElement("lastName", tester.LastName);

            var licenseType = new XElement("licenseType");
            foreach (var license in tester.LicenseType)
                licenseType.Add(license);


            var phoneNumber = new XElement("phoneNumber", tester.PhoneNumber);


            return new XElement("tester", experience, licenseTypeTeaching, maxDistance, maxWeekExams, schedule, id, address, birthDate, emailAddress, firstName, gender, lastName, licenseType, phoneNumber);
        }


        private IEnumerable<Tester> GetAllTestersFromXml()
        {
            var testers = new List<Tester>();
            foreach (var xmLTester in _testersXML.Elements())
            {
                // address
                var city = xmLTester.Element("address")?.Element("city")?.Value;
                var building = xmLTester.Element("address")?.Element("building")?.Value;
                var street = xmLTester.Element("address")?.Element("street")?.Value;
                var entrance = xmLTester.Element("address")?.Element("entrance")?.Value;

                List<LicenseType> licensesTypes = new List<LicenseType>();
                licensesTypes.AddRange( xmLTester.Element("licenseType")
                                            ?.Elements()
                                            .Select(
                                                item => (LicenseType) Enum.Parse(typeof(LicenseType), item.Value
                                                )) ?? throw new InvalidOperationException());

                List<LicenseType> licensesTypesTeaching = new List<LicenseType>();
                licensesTypesTeaching.AddRange(xmLTester.Element("licenseTypeTeaching")
                                                   ?.Elements()
                                                   .Select(
                                                       item => (LicenseType)Enum.Parse(typeof(LicenseType), item.Value
                                                       )) ?? throw new InvalidOperationException());
                
               

                        var tester = new Tester
                {
                    BirthDate = DateTime.Parse(xmLTester.Element("birthDate")?.Value),
                    Address = new Address(city, street, building, entrance),   ///////////////////////////////////////
                    EmailAddress = xmLTester.Element("emailAddress")?.Value,
                    Experience = uint.Parse(xmLTester.Element("experience")?.Value),
                    Gender = (Gender)Enum.Parse(typeof(Gender), xmLTester.Element("gender")?.Value),
                    FirstName = xmLTester.Element("firstName")?.Value,
                    Id = uint.Parse(xmLTester.Element("id")?.Value),
                    LastName = xmLTester.Element("lastName")?.Value,

                    LicenseType = licensesTypes, 
                    LicenseTypeTeaching = licensesTypesTeaching, 

                    MaxDistance = float.Parse(xmLTester.Element("maxDistance")?.Value),
                    Schedule = null,
                    MaxWeekExams = uint.Parse(xmLTester.Element("maxWeekExams")?.Value),
                    PhoneNumber = xmLTester.Element("phoneNumber")?.Value,
                };

            }

            return testers;
        }

      

        #endregion







    }
}
