using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BE.Routes;

namespace BE.MainObjects
{
   public  class Person
    {
     
        private uint _id;
        /// <summary>
        /// isreal id
        /// </summary>
        public uint Id
        {
            get => _id;
             set
            {
                if (Tools.CheckID_IL(value))
                    _id = value;
                else
                    throw new Exception("Invalid Id");
            }
        }

        private string _emailAddress;
        /// <summary>
        /// email
        /// </summary>
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
        public List<LicenseType> LicenseType { set; get; }

        public Person() { }
        /// <summary>
        /// a new person
        /// </summary>
        /// <param name="Id">Id</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last Name</param>
        public Person(uint Id,string firstName="",string lastName="",Gender g=Gender.Male)
        {
            this.Id = Id;
            if (Id == 0)
                throw new Exception("Invalid Id");
            //birthDate = new DateTime();
            BirthDate = new DateTime();
            LicenseType = new List<LicenseType>();
            FirstName = firstName;
            LastName = lastName;
            this.Gender = g;
            _phoneNumber = "";
    }

        public Person(uint Id, string firstName, string lastName, Gender gender, string emailAddress,
            DateTime birthDate, string phoneNumber, Address address, List<LicenseType> licenseTypes)
        {
            _id = Id;
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
                return "Name: " + FirstName + " " + LastName + " ,Id: " + Id + " ";
            else
                return "Id: " + Id;
        }
    }
}
