using System;
using System.Collections.Generic;
using  BE;
using  DS;

namespace DAL
{

    public class AccessTraineesData
    {
        private Data data = new Data();
        //    private List <Trainee> _trainees = new List<Trainee>();

        public void Update(Trainee updatedTrainee)
        {
            var trainee = da _trainees.Find(t => t.ID == updatedTrainee.ID);
            _trainees.Remove(trainee);
            _trainees.Add(updatedTrainee);
        }

        public void Delete(Trainee traineeToDelete)
        {
            _trainees.Remove(traineeToDelete);
        }

    }








}
