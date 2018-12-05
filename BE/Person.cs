using System;
using System.Collections.Generic;

namespace BE
{
    /// <summary>
    /// A person
    /// </summary>
   public  class Person
    {
        private string id;

        /// <summary>
        /// Israel Id תעודת זהות
        /// </summary>
        public string ID
        {
            get => id; private set
            {
                if (Tools.CheckID_IL(uint.Parse(value)))
                    id = $"{uint.Parse(value):000000000}";
                else
                    throw new Exception("Invalid id");
            }
        }
        private string _email;

        /// <summary>
        /// A valid email
        /// </summary>
        public string Email { get => _email;
            set
            {
                int index =value.IndexOf('@');
                if (index == -1)
                    throw new Exception("Invalid email address: "+value);
                index = value.IndexOf('@', index + 1);
                if(index!=-1)
                    throw new Exception("Invalid email address: "+value);
                _email = value;
            }
        }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { set; get; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { set; get; }

        /// <summary>
        /// Birth date
        /// </summary>
        public DateTime BirthDate { set; get; }

        /// <summary>
        /// Gender
        /// </summary>
        public Gender Gender { set; get; }

        private string _phoneNumber;

        /// <summary>
        /// set only israel phone number like 0500000000 or 020000000
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber; set
            {
                if (value[0] != '0')
                    _phoneNumber = null;
                else if (value[1] == '5' && value.Length == 10)
                    _phoneNumber = value;
                else if (value[1] != '5' && value.Length == 9)
                    _phoneNumber = value;
                else
                    _phoneNumber = null;
            }
        }

        /// <summary>
        /// Address
        /// </summary>
        public Address Address { set; get; }

        /// <summary>
        /// license type that he/she has
        /// </summary>
        public List<LicenseType> LicenseType { set; get; }

        /// <summary>
        /// a new person
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="g">Gender</param>
        public Person(string id,string firstName="",string lastName="",Gender g=Gender.Male)
        {
            ID = id;
            if (ID == null)
                throw new Exception("Invalid ID");
            BirthDate = new DateTime();
            LicenseType = new List<LicenseType>();
            FirstName = firstName;
            LastName = lastName;
            Gender = g;
            _phoneNumber = "";
        }
        
        /// <summary>
        /// Information about the person
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (FirstName != "" && LastName != "")
                return "Name: " + FirstName + " " + LastName + " ,ID: " + ID + " ";
            else
                return "ID: " + ID;
        }
    }
}
