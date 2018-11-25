using System;
using System.Collections.Generic;

namespace BE
{
   public  class Person
    {
        private uint id;
        public uint ID
        {
            get => id; private set
            {
                if (Tools.CheckID_IL(value))
                    id = value;
                else
                    throw new Exception("Invalied id");
            }
        }
        private string email;
        public string Email { get { return email; } set
            {
                int index =value.IndexOf('@');
                if (index == -1)
                    throw new Exception("Invalied email address: "+value);
                index = value.IndexOf('@', index + 1);
                if(index!=-1)
                    throw new Exception("Invalied email address: "+value);
                email = value;
            }
        }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public string EmailAddress { set; get; }
        private DateTime birthDate { set; get; }
        public DateTime BirthDate { set; get; }
        public Gender Gender { set; get; }
        private string phoneNumber;
        /// <summary>
        /// set only israely phone number like 0500000000 or 020000000
        /// </summary>
        public string PhoneNumber
        {
            get => phoneNumber; set
            {
                if (value[0] != '0')
                    phoneNumber = null;
                else if (value[1] == '5' && value.Length == 10)
                    phoneNumber = value;
                else if (value[1] != '5' && value.Length == 9)
                    phoneNumber = value;
                else
                    phoneNumber = null;
            }
        }
        public Address Address { set; get; }
        public List<LicenceType> LicenceType { set; get; }
        /// <summary>
        /// a new person
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="Fn">First name</param>
        /// <param name="Ln">Last Name</param>
        public Person(uint id,string Fn="",string Ln="",Gender g=Gender.Male)
        {
            ID = id;
            if (ID == 0)
                throw new Exception("Invalied ID");
            birthDate = new DateTime();
            BirthDate = new DateTime();
            LicenceType = new List<LicenceType>();
            FirstName = Fn;
            LastName = Ln;
            this.Gender = g;
            phoneNumber = "";
    }
        
        public override string ToString()
        {
            if (FirstName != "" && LastName != "")
                return "Name: " + FirstName + " " + LastName + " ,ID: " + ID + " ";
            else
                return "ID: " + ID;
        }
    }
}
