using System;
using System.Collections.Generic;
using System.Linq;
using  BE;
using  DS;

namespace DAL
{

    public  class AccessTraineesData
    {       
        //    private List <Trainee> _trainees = new List<Trainee>();

        public void Update(Trainee updatedTrainee)
        {
            var trainee = DataSource.Trainees.Find(t => t.ID == updatedTrainee.ID);
            DataSource.Trainees.Remove(trainee);
            DataSource.Trainees.Add(updatedTrainee);
        }

        public void Delete(Trainee traineeToDelete)
        {   
           DataSource.Trainees.Remove(traineeToDelete);
        }

        public void Insert(Trainee traineeToAdd)
        {
            try
            {
                if (DataSource.Trainees.Any(t => t.ID == traineeToAdd.ID))
                  throw new Exception("the trainee already exist in the system");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            DataSource.Trainees.Add(traineeToAdd);
        }

    }








}
