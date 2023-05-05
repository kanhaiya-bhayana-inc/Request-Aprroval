using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequestApproval.Enums
{
    public class ValidationMessages
    {
        public const string userExists = "User already exists, try different email.";

        public const string wrongCredentials = "Invalid credentials, try again.";

        public const string userDoesnotExists = "User does not exists.";
    }
}