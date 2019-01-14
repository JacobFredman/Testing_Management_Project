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
    public partial class DalImp
    {
        private bool _testsChanged = true;

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
            SerializeTestsToXml(_tests);
        }










        /// <summary>
        ///     Add a new test
        /// </summary>
        /// <param name="newTest"></param>
        public void AddTest(Test newTest)
        {
            newTest.Id = $"{Configuration.TestId:00000000}";
            Configuration.TestId++;
            SaveConfigurations();

           _tests.Add(newTest);
            SerializeTestsToXml(_tests);
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
            SerializeTestsToXml(_tests);
        }

        ///// <summary>
        /////     update an existing test
        ///// </summary>
        ///// <param name="testToUpdate"></param>
        //public void UpdateTest(Test testToUpdate)
        //{
        //    if (DataSource.Tests.All(x => x.Id != testToUpdate.Id))
        //        throw new Exception("Test doesn't exist");

        //    var test = DataSource.Tests.Find(t => t.Id == testToUpdate.Id);
        //    DataSource.Tests.Remove(test);
        //    DataSource.Tests.Add(testToUpdate);

        //    _testsXML.Elements().First(x => x.Element("id")?.Value == testToUpdate.Id.ToString()).Remove();
        //    // serialize the testToUpdate and save it
        //    SerializeTestsToXml(new List<Test>{testToUpdate});
        //}

        private static void SerializeTestsToXml(IReadOnlyCollection<Test> list)
        {
            var file = new FileStream(Configuration.TestsXmlFilePath, FileMode.Create);
            var xmlSerializer = new XmlSerializer(list.GetType());
            xmlSerializer.Serialize(file, list);
            file.Close();
        }


        private static IEnumerable<Test> DeSerializeTestFromXml()
        {
            var file = new FileStream(Configuration.TestsXmlFilePath,FileMode.Open);
            var xmlSerializer = new XmlSerializer(typeof(List<Test>));
            var list = (List<Test>) xmlSerializer.Deserialize(file);
            file.Close();
            return list;
        }

        #endregion

    }
}