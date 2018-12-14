using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BE.Routes;

namespace BE.MainObjects
{
    /// <summary>
    /// An person
    /// </summary>
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

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { set; get; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { set; get; }

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
        /// set only israeli phone number like 0500000000 or 020000000
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber; set
            {
                if (value[0] != '0')
                   throw new Exception("Invalid phone number");
                else if (value[1] == '5' && value.Length == 10)
                    _phoneNumber = value;
                else if (value[1] != '5' && value.Length == 9)
                    _phoneNumber = value;
                else
                    throw new Exception("Invalid phone number");
            }
        }

        /// <summary>
        /// Address
        /// </summary>
        public Address Address { set; get; }

        /// <summary>
        /// license type
        /// </summary>
        public List<LicenseType> LicenseType { set; get; }

        /// <summary>
        /// A new person
        /// </summary>
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

        /// <summary>
        /// An new person
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="gender"></param>
        /// <param name="emailAddress"></param>
        /// <param name="birthDate"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="address"></param>
        /// <param name="licenseTypes"></param>
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
        
        //Basic data about the person
        public override string ToString()
        {
            if (FirstName != "" && LastName != "")
                return "Name: " + FirstName + " " + LastName + " ,Id: " + Id + " ";
            else
                return "Id: " + Id;
        }
    }
}
