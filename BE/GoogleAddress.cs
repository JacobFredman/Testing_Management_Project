using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// A google maps address
    /// </summary>
    public class GoogleAddress
    {
        /// <summary>
        /// Address name
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// Address google place id
        /// </summary>
        public string ID { set; get; }
    }
}
