using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    public class Dal_imp : IDal
    {
        private DataSource data;
        public Dal_imp()
        {
             data= new DataSource();
        }
        public void AddTest(Test t)
        {
            throw new NotImplementedException();
        }

        public void AddTester(Tester t)
        {
            if (data.Testers.Any(x => x.ID == t.ID))
            {
                data.Testers.Add(t);
            }
            else
                throw new Exception("Tester alredy exist");
        }

        public void AddTrainee(Trainee t)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Tester> GetAllTesters()
        {
            return data.Testers.GetEnumerator();
        }

        public IEnumerator<Test> GetAllTests()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Trainee> GetAllTrainee()
        {
            throw new NotImplementedException();
        }

        public void RemoveTest(Test t)
        {
            throw new NotImplementedException();
        }

        public void RemoveTester(Tester t)
        {
            if(data.Testers.Any(x=>x.ID==t.ID))
            {
                data.Testers.RemoveAll(x => x.ID == t.ID);
            }
            else
            {
                throw new Exception("Tester doesn't exist");
            }
        }

        public void RemoveTrainee(Trainee t)
        {
            throw new NotImplementedException();
        }

        public void UpdateTest(Test t)
        {
            throw new NotImplementedException();
        }

        public void UpdateTester(Tester t)
        {
            if (data.Testers.Any(x => x.ID == t.ID))
            {
                data.Testers = data.Testers.Where(x => x.ID == t.ID).Select(x => x = t).ToList();
            }else
                throw new Exception("Tester doesn't exist");
        }

        public void UpdateTrainee(Trainee t)
        {
            throw new NotImplementedException();
        }
    }
}
