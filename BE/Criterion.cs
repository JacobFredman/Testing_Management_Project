using System;

namespace BE
{
    /// <summary>
    ///     A criterion for passing the test
    /// </summary>
    public class Criterion : ICloneable
    {
        /// <summary>
        ///     new Criteria
        /// </summary>
        /// <param name="t">the criterion</param>
        /// <param name="p">pass or not</param>
        public Criterion(string t, bool p = false)
        {
            Type = t;
            Pass = p;
        }

        /// <summary>
        ///     the type of the criterion
        /// </summary>
        public string Type { set; get; }

        /// <summary>
        ///     Passed the criterion
        /// </summary>
        public bool Pass { set; get; }

        public object Clone()
        {
            return new Criterion(Type, Pass);
        }

        /// <summary>
        ///     type the criterion
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Type: " + Type + " ,Passed: " + (Pass ? "yes" : "no");
        }
    }
}