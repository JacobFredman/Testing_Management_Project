using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BE.Routes;

namespace BE.MainObjects
{
    /// <summary>
    ///     An person
    /// </summary>
    public class Person
    {
        private string _emailAddress;

        private uint _id;

        private string _phoneNumber;

        /// <summary>
        ///     A new person
        /// </summary>
        protected Person()
        {
            LicenseType = new List<LicenseType>();
        }

        /// <summary>
        ///     Valid id
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

        /// <summary>
        ///     email
        /// </summary>
        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                if (value != "")
                {
                    if (new EmailAddressAttribute().IsValid(value))
                        _emailAddress = value;
                    else throw new Exception("email address isn't valid");
                }

                else
                {
                    _emailAddress = "";
                }
            }
        }

        /// <summary>
        ///     First name
        /// </summary>
        public string FirstName { set; get; }

        /// <summary>
        ///     Last name
        /// </summary>
        public string LastName { set; get; }

        /// <summary>
        ///     Birth date
        /// </summary>
        public DateTime BirthDate { set; get; }

        /// <summary>
        ///     Gender
        /// </summary>
        public Gender Gender { set; get; }

        /// <summary>
        ///     set only israeli phone number like 0500000000 or 020000000
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (value != "")
                {
                    if (value[0] != '0')
                        throw new Exception("Invalid phone number");
                    if (value[1] == '5' && value.Length == 10)
                        _phoneNumber = value;
                    else if (value[1] != '5' && value.Length == 9)
                        _phoneNumber = value;
                    else
                        throw new Exception("Invalid phone number");
                }
                else
                {
                    _phoneNumber = "";
                }
            }
        }

        /// <summary>
        ///     Address
        /// </summary>
        public Address Address { set; get; }

        /// <summary>
        ///     license type
        /// </summary>
        public List<LicenseType> LicenseType { set; get; }

        //Basic data about the person
        public override string ToString()
        {
            if (FirstName != "" && LastName != "")
                return "Name: " + FirstName + " " + LastName + " ,Id: " + Id + " ";
            return "Id: " + Id;
        }
    }
}