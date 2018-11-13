using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
   public  class Criterion
    {
        public string Type {private set; get; }
        public bool Pass { set; get; }
        /// <summary>
        /// new Criterion
        /// </summary>
        /// <param name="t">the criterion</param>
        /// <param name="p">pass or not</param>
        public Criterion(string t,bool p= false)
        {
            Type = t;
            Pass = p;
        }
        public override string ToString()
        {
            return "Type: " + Type + " ,Pass: " + (Pass ? "yes" : "no");
        }
    }
}
