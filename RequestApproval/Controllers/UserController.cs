using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RequestApproval.DTO;
using RequestApproval.Enums;
using RequestApproval.Models;

namespace RequestApproval.Controllers
{
    public class UserController : Controller
    {
        RequestApprovalEntities db = new RequestApprovalEntities();

        private readonly ValidationMessages errorObj = new ValidationMessages();
        
        // GET: User
        public ActionResult Index()
        {
            List<UserDTO> users = new List<UserDTO>();
            List<UserDetail> userDetail = new List<UserDetail>();
            List<Credential> credential = new List<Credential>();
            userDetail = db.UserDetails.ToList();
            credential = db.Credentials.ToList();
            foreach (var x in userDetail)
            {
                var obj = db.Credentials.FirstOrDefault(c =>c.UId == x.Id);
                UserDTO user = new UserDTO();
                user.Id = x.Id;
                user.FirstName = x.FirstName;
                user.LastName = x.LastName;
                user.Address = x.Address;
                user.Phone = x.Phone;
                user.RoleId = (int)x.RoleId;
                user.Password = obj.Password;
                user.Email = obj.Email;
                user.IsActive = (bool)obj.IsActive;

                users.Add(user);
            }
            return View(users);
        }

        [HttpGet]
        public ActionResult SignUp() {
            return View();
        }


        [HttpPost]
        public ActionResult SignUp(UserDTO request)
        {
            if (db.Credentials.Any(x =>x.Email == request.Email))
            {
                ViewBag.Notification = errorObj.userExists;
                return View();
            }
            else
            {
                var password = Helper.Encrypt(request.Password);
                UserDetail user = new UserDetail();
                Credential credential1 = new Credential();

                user.Address = request.Address;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Phone = request.Phone;
                user.RoleId = 2;

                db.UserDetails.Add(user);
                db.SaveChanges();

                var UserId = user.Id;

                credential1.UId = UserId;
                credential1.Password = password;
                credential1.Email = request.Email;
                credential1.IsActive = false;

                db.Credentials.Add(credential1);
                db.SaveChanges();

                return RedirectToAction("Index");

            }
            
        }
    }
}