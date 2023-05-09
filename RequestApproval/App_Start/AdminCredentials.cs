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

                RequestApprovalEntities5 db = new RequestApprovalEntities5();
                const string password = "Admin@1234";
                string adminPassword = Helper.Encrypt(password);
                var admin = db.UserDetails.Any(x => x.Email == "admin@requestapproval.com");
                if (!admin)
                {
                    
                    db.CreateAdminCredentials("Admin", "", "6987012354", "South Delhi", "admin@requestapproval.com", adminPassword);
                }
                else
                {
                    UserDetail userDetail = db.UserDetails.FirstOrDefault(x => x.Email == "admin@requestapproval.com");
                    /*LoginDetail loginDetail = db.LoginDetails.FirstOrDefault(x => x.)*/
                    int id = (int)userDetail.LoginId;
                    db.UpdateAdminCredentials(id, "Admin", "", "6987012354", "South Delhi", adminPassword);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}