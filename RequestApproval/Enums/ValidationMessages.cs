using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequestApproval.Enums
{
    public class ValidationMessages
    {
        public string userExists = "User already exists, try different email.";

        public string wrongCredentials = "Invalid credentials, try again.";
    }
}