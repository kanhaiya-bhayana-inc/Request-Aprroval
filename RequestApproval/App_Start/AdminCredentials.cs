using RequestApproval.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RequestApproval.App_Start
{
    public class AdminCredentials
    {
        public static void init()
        {
            try
            {

                RequestApprovalEntities4 db = new RequestApprovalEntities4();
                var admin = db.LoginDetails.Any(x => x.RoleId == 1 && x.IsActive == false && x.DeletedFlag == true);
                if (admin)
                {
                    string password = "Admin@1234";
                    string adminPassword = Helper.Encrypt(password);
                    db.CreateAdminCredentials("Admin", "", "6987012354", "South Delhi", "admin@requestapproval.com", adminPassword);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}