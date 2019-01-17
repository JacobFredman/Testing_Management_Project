using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BE;
using BE.MainObjects;

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
    public class DalImp : IDal
    {
        //the data in xml
        private readonly XElement _traineesXml;
        private readonly XElement _testersXml;
        private XElement _config;

        //lists of the data
        private  List<Trainee> _trainees = new List<Trainee>();
        private  List<Test> _tests = new List<Test>();
        private  List<Tester> _testers = new List<Tester>();

        //flags
        private bool _traineeChanged = true;
        private bool _testerChanged = true;

        /// <summary>
        ///     DalImp c-tor
        /// </summary>
        internal DalImp()
        {
            try
            {
                //initialize trainees
                if (File.Exists(Configuration.TraineesXmlFilePath))
                    _traineesXml = XElement.Load(Configuration.TraineesXmlFilePath);
                else
                    _traineesXml = new XElement("trainees");
                _trainees = XML.GetAllTraineesFromXml(_traineesXml).ToList();

                //initialize testers
                if (File.Exists(Configuration.TestersXmlFilePath))
                    _testersXml = XElement.Load(Configuration.TestersXmlFilePath);
                else
                    _testersXml = new XElement("testers");
                _testers = XML.GetAllTestersFromXml(_testersXml).ToList();

                //initialize tests
                if (File.Exists(Configuration.TestsXmlFilePath))
                    _tests = XML.DeSerializeTestFromXml().ToList();
                else
                    _tests = new List<Test>();

                //Load configurations
              _config = XML.LoadConfigurations();
            }
            catch
            {
                //do nothing
            }
        }

        #region Trainee

        /// <summary>
        ///     Add trainee
        /// </summary>
        /// <param name="newTrainee"></param>
        public void AddTrainee(Trainee newTrainee)
        {
            if (XML.GetAllTraineesFromXml(_traineesXml).Any(t => t.Id == newTrainee.Id))
                throw new Exception("The trainee already exist in the system");

            _traineesXml.Add(XML.ConvertTraineeToXml(newTrainee));
            _traineesXml.Save(Configuration.TraineesXmlFilePath);
            _traineeChanged = true;
        }

        /// <summary>
        ///     remove trainee
        /// </summary>
        /// <param name="traineeToDelete"></param>
        public void RemoveTrainee(Trainee traineeToDelete)
        {
            if (XML.GetAllTraineesFromXml(_traineesXml).All(x => x.Id != traineeToDelete.Id))
                throw new Exception("Trainee doesn't exist");

            _traineesXml.Elements().First(x => x.Element("id")?.Value == traineeToDelete.Id.ToString()).Remove();
            _traineesXml.Save(Configuration.TraineesXmlFilePath);
            _traineeChanged = true;
        }

        /// <summary>
        ///     update an existing trainee
        /// </summary>
        /// <param name="updatedTrainee"></param>
        public void UpdateTrainee(Trainee updatedTrainee)
        {
            if (XML.GetAllTraineesFromXml(_traineesXml).All(x => x.Id != updatedTrainee.Id))
                throw new Exception("Trainee doesn't exist");

            _traineesXml.Elements().First(x => x.Element("id").Value == updatedTrainee.Id.ToString()).Remove();
            _traineesXml.Add(XML.ConvertTraineeToXml(updatedTrainee));
            _traineesXml.Save(Configuration.TraineesXmlFilePath);
            _traineeChanged = true;
        }

        public void SaveConfigurations()
        {
            XML.SaveConfigurations(_config);
        }

        public XElement LoadConfigurations()
        {
           return XML.LoadConfigurations();
        }

        #endregion

        #region Tester

        /// <summary>
        ///     Add tester
        /// </summary>
        /// <param name="newTester"></param>
        public void AddTester(Tester newTester)
        {
            if (XML.GetAllTestersFromXml(_testersXml).Any(tester => tester.Id == newTester.Id))
                throw new Exception("The tester already exist in the system");

            _testersXml.Add(XML.TesterToXmlElement(newTester));
            _testersXml.Save(Configuration.TestersXmlFilePath);
            _testerChanged = true;
        }

        /// <summary>
        ///     Remove a tester
        /// </summary>
        /// <param name="testerToDelete"></param>
        public void RemoveTester(Tester testerToDelete)
        {
            if (XML.GetAllTestersFromXml(_testersXml).All(x => x.Id != testerToDelete.Id))
                throw new Exception("Tester doesn't exist");

            _testersXml.Elements().First(x => x.Element("id")?.Value == testerToDelete.Id.ToString()).Remove();
            _testersXml.Save(Configuration.TestersXmlFilePath);
            _testerChanged = true;
        }

        /// <summary>
        ///     update existing Tester
        /// </summary>
        /// <param name="testerToUpdate"></param>
        public void UpdateTester(Tester testerToUpdate)
        {
            if (XML.GetAllTestersFromXml(_testersXml).All(x => x.Id != testerToUpdate.Id))
                throw new Exception("Trainee doesn't exist");

            _testersXml.Elements().First(x => x.Element("id")?.Value == testerToUpdate.Id.ToString()).Remove();
            _testersXml.Add(XML.TesterToXmlElement(testerToUpdate));
            _testersXml.Save(Configuration.TestersXmlFilePath);
            _testerChanged = true;
        }

        #endregion

        #region Test


        /// <summary>
        ///     update an existing test
        /// </summary>
        /// <param name="updatedTest"></param>
        public void UpdateTest(Test updatedTest)
        {
            if (_tests.All(x => x.Id != updatedTest.Id))
                throw new Exception("Test doesn't exist");

            var test = _tests.Find(t => t.Id == updatedTest.Id);
            _tests.Remove(test);
            _tests.Add(updatedTest);
            XML.SerializeTestsToXml(_tests);
        }


        /// <summary>
        ///     Add a new test
        /// </summary>
        /// <param name="newTest"></param>
        public void AddTest(Test newTest)
        {
            newTest.Id = $"{Configuration.TestId:00000000}";
            Configuration.TestId++;
            XML.SaveConfigurations(_config);

            _tests.Add(newTest);
            XML.SerializeTestsToXml(_tests);
        }

        /// <summary>
        ///     remove a test
        /// </summary>
        /// <param name="testToDelete"></param>
        public void RemoveTest(Test testToDelete)
        {
            if (_tests.All(x => x.Id != testToDelete.Id))
                throw new Exception("Test doesn't exist");

            var testToRemove = _tests.Single(r => r.Id == testToDelete.Id);
            _tests.Remove(testToRemove);
            XML.SerializeTestsToXml(_tests);
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
                if (!_testerChanged) // if the testers list didn't changed don't go to xml file 
                    return _testers.Select(item => item.Clone() as Tester).OrderBy(x => x.Id).ToList(); ;
                var allTesters = XML.GetAllTestersFromXml(_testersXml).Select(item => item.Clone() as Tester).ToList();
                _testers = allTesters;
                _testerChanged = false;
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
                foreach (var item in _tests)
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
                if (!_traineeChanged) // if the testers list didn't changed don't go to xml file 
                    return _trainees.Select(item => item.Clone() as Trainee).OrderBy(x => x.Id).ToList(); ;
                var allTrainee = XML.GetAllTraineesFromXml(_traineesXml).Select(item => item.Clone() as Trainee).ToList();
                _trainees = allTrainee;
                _traineeChanged = false;
                return allTrainee.OrderBy(x => x.Id);
            }
        }

        #endregion
    }
}