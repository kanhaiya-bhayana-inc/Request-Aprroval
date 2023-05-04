using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RequestApproval.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "This field is required!")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "This field is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}