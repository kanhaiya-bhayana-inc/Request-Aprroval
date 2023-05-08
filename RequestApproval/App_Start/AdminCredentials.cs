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
            /*try
            {

            RequestApprovalEntities4 db = new RequestApprovalEntities4();
            UserDetail admin = db.UserDetails.FirstOrDefault(x => x.RoleId == 1);
            if (admin == null)
            {
                    string password = "Admin@1234";
                    string adminPassword = Helper.Encrypt(password);
                *//*List<>*//*
                db.CreateAdminCredentials("Admin","","6987012354","South Delhi","admin@requestapproval.com", adminPassword);
            }
            }
            catch (Exception ex)
            {
                throw ex;
            }*/
        }
    }
}