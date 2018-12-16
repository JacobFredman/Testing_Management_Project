using System.Linq;
using BE;
using BE.MainObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using  BL;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Insert_new_tester_and_get_it_back()
        {
            // Arrange
           var bl = FactoryBl.GetObject;
        //  var bl = new BlImp();
            var tester = new Tester(037982519, "Jacov", "Fredman", Gender.Male);

            // Act
            bl.AddTester(tester);
            var result = bl.AllTesters.Any(t => t.Id == tester.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void insert_new_trainee_and_get_it_back()
        {
            // Arrange
            var bl = FactoryBl.GetObject;
            var trainee = new Trainee(037982519, Gender.Male, "Jacov", "Fredman");

            // Act
            bl.AddTrainee(trainee);
            var result = bl.AllTrainee.Any(t => t.Id == trainee.Id);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void insert_new_test_and_get_it_back()
        {
            // Arrange
            var bl = FactoryBl.GetObject;
            var test = new Test(037982519, 037982519);

            // Act
            bl.AddTest(test);
            var result = bl.AllTrainee.Any(t => t.Id.ToString() == test.Id);

            //Assert
            Assert.IsTrue(result);
        }


    }
}
