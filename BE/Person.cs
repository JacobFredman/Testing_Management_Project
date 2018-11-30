using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BE
{
   public  class Person
    {
        private uint _id;
        public uint Id
        {
            get => _id;
             set
            {
                if (Tools.CheckID_IL(value))
                    _id = value;
                else
                    throw new Exception("Invalid id");
            }
        }

        private string _emailAddress;
        public string EmailAddress {
            get => _emailAddress;
            set
            {
                if (new EmailAddressAttribute().IsValid(value))
                    _emailAddress = value;
                else throw  new Exception("email address isn't valid");
            }
        }

        public string FirstName { set; get; }
        public string LastName { set; get; }
        public DateTime BirthDate { set; get; }
        public Gender Gender { set; get; }
        private string _phoneNumber;
        /// <summary>
        /// set only israeli phone number like 0500000000 or 020000000
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

        public Address Address { set; get; }
        public List<LicenceType> LicenseType { set; get; }
        /// <summary>
        /// a new person
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last Name</param>
        public Person(uint id,string firstName="",string lastName="",Gender g=Gender.Male)
        {
            Id = id;
            if (Id == 0)
                throw new Exception("Invalid ID");
            //birthDate = new DateTime();
            BirthDate = new DateTime();
            LicenseType = new List<LicenceType>();
            FirstName = firstName;
            LastName = lastName;
            this.Gender = g;
            _phoneNumber = "";
    }

        public Person(uint id, string firstName, string lastName, Gender gender, string emailAddress,
            DateTime birthDate, string phoneNumber, Address address, List<LicenceType> licenseTypes)
        {
            _id = id;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            _emailAddress = emailAddress;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Address = address;
            LicenseType = licenseTypes;
        }
        
        public override string ToString()
        {
            if (FirstName != "" && LastName != "")
                return "Name: " + FirstName + " " + LastName + " ,ID: " + Id + " ";
            else
                return "ID: " + Id;
        }
    }
}
