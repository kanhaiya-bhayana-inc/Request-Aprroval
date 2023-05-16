using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RequestApproval.DTO
{
    public class UserDTO
    {
        public UserDTO() { }

        public UserDTO(int Id, string FirstName, string LastName, string Phone, string Address, int RoleId, string Email, bool IsActive, string UserType) {
        
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Phone = Phone;
            this.Address = Address;
            this.RoleId = RoleId;
            this.Email = Email;
            this.IsActive = IsActive;
            this.UserType = UserType;
        }
        public UserDTO(int Id, string FirstName, string LastName, string Phone, string Address, int RoleId, string Email, string UserType)
        {

            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Phone = Phone;
            this.Address = Address;
            this.RoleId = RoleId;
            this.Email = Email;
            this.UserType = UserType;
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [RegularExpression(@"^[A-Z][a-z]{3,}$", ErrorMessage = "First letter must be capital and minimum length is 4, try again.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Not a valid phone number!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Address")]
        public string Address { get; set; }


        public int RoleId { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$#!%*?&])[A-Za-z\d@$#!%*?&]{8,16}$", ErrorMessage = "Password must contain minimum 8 and maximum 16 characters, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character!")]
        public string Password { get; set; }

        [Display(Name ="Status")]
        public Boolean IsActive { get; set; }

        public Boolean DeletedFlag { get; set; }

        public string UserType { get; set; }

        /*public SelectList Clubs { get; set; }*/
    }
    /*public enum IsActive
    {
        Active,
        InActive
    }*/
}