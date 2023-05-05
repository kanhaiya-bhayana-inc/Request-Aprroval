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

            RequestApprovalEntities db = new RequestApprovalEntities();
            UserDetail admin = db.UserDetails.FirstOrDefault(x => x.RoleId == 1);
            if (admin == null)
            {
                    string password = "David@1234";
                    string adminPassword = Helper.Encrypt(password);
                /*List<>*/
                db.CreateAdminCredentials("David","Dragan","6987012354","South Delhi","david@dragan.com", adminPassword);
            }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}