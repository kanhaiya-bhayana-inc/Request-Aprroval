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
        RequestApprovalEntities4 db = new RequestApprovalEntities4();

        
        // GET: User
        public ActionResult Index()
        {
            try
            {


                List<UserDTO> users = new List<UserDTO>();
                /*List<UserDetail> userDetail1 = new List<UserDetail>();*/
                List<UserDetail> userDetail = new List<UserDetail>();
                List<LoginDetail> loginDetails = new List<LoginDetail>();
                List<LoginDetail> activeUsers = new List<LoginDetail>();
                loginDetails = db.LoginDetails.ToList();
                /*userDetail = db.UserDetails.ToList();*/
                foreach(LoginDetail x in loginDetails)
                {
                    if (x.IsActive == true && x.DeletedFlag == false)
                    {
                        activeUsers.Add(x);
                    }
                }
                /*credential = db.Credentials.ToList();*/
                foreach (var x in activeUsers)
                {
                    var obj = db.UserDetails.FirstOrDefault(c => c.LoginId == x.Id);
                    var roleType = db.Roles.FirstOrDefault(c =>c.RoleId == x.RoleId);
                    UserDTO user = new UserDTO();
                    user.Id = x.Id;
                    user.FirstName = obj.FirstName;
                    user.LastName = obj.LastName;
                    user.Address = obj.Address;
                    user.Phone = obj.Phone;
                    /*user.RoleId = (int)x.RoleId;*/
                    /*                user.Password = obj.Password;
                    */
                    user.Email = obj.Email;
                    user.IsActive = (bool)x.IsActive;
                    user.DeletedFlag = (bool)x.DeletedFlag;
                    user.UserType = roleType.RoleName;

                    users.Add(user);
                }
                return View(users);
            }
            catch { return View(); }
        }

        [HttpGet]
        public ActionResult SignUp() {
            return View();
        }


        [HttpPost]
        public ActionResult SignUp(UserDTO request)
        {
            try
            {


                if (db.UserDetails.Any(x => x.Email == request.Email))
                {
                    ViewBag.Notification = ValidationMessages.userExists;
                    return View();
                }
                else
                {
                    var password = Helper.Encrypt(request.Password);
                    UserDetail user = new UserDetail();
                    LoginDetail loginDetail = new LoginDetail();

                    
                    loginDetail.Password = password;
                    loginDetail.IsActive = true;
                    loginDetail.RoleId = 2;
                    loginDetail.DeletedFlag = false;


                    db.LoginDetails.Add(loginDetail);
                    db.SaveChanges();

                    var UserId = loginDetail.Id;

                    user.LoginId= UserId;
                    user.Address = request.Address;
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.Phone = request.Phone;
                    user.Email = request.Email;

                    db.UserDetails.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
            }
            catch { return View(); }

        }
        

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserDashboard() 
        {
            try
            {
                if (Session["Id"] != null)
                {
                    var id =(int) Session["Id"];
                    var user = db.UserDetails.FirstOrDefault(x => x.LoginId == id);
                    UserDTO userDTO = new UserDTO();
                    userDTO.FirstName = user.FirstName;
                    userDTO.LastName = user.LastName;
                        return View("");
                }
                return View("");
            }
            catch { return View(); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDTO request)
        {
            try
            {
                var check = db.UserDetails.FirstOrDefault(x => x.Email == request.Email);
                if (check != null)
                {

                    string password = Helper.Encrypt(request.Password);
                   /* if(check.Email == "david@dragan.com")
                    {
                        Session["RoleName"] = "Admin";
                        password = password.Substring(0, 30);
                    }*/
                    var checkLogin = db.UserDetails.FirstOrDefault(x => x.Email == request.Email); 
                    var checkLogin2 = db.LoginDetails.FirstOrDefault(x => x.Password == password);
                    if ((checkLogin != null && checkLogin2 != null) && (checkLogin.LoginId == checkLogin2.Id))
                    {

                        if (checkLogin2.RoleId == 1)
                        {
                            Session["Permission"] = "Access-granted";
                        }
                        UserDetail user = db.UserDetails.FirstOrDefault(x => x.LoginId == checkLogin2.Id);
                        string FullName = user.FirstName + " " + user.LastName;
                        Session["Id"] = user.LoginId;
                        Session["UserName"] = FullName.ToString();
                        return RedirectToAction("UserDashboard", User);
                    }

                    else
                    {
                        ViewBag.Notification = ValidationMessages.wrongCredentials;
                        return View();
                    }

                }
                else
                {
                    ViewBag.Notification = ValidationMessages.userDoesnotExists; return View();
                }
            }
            catch { return View(); }
        }

        public ActionResult Logout()
        {
            try
            {

            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
            }
            catch { return View(); }
        }
        /*
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {

            UserDetail obj = db.UserDetails.FirstOrDefault(x =>x.Id ==id);
            Credential credential = db.Credentials.FirstOrDefault(x => x.UId == id);
            UserDTO user = new UserDTO();
            user.FirstName = obj.FirstName;
            user.LastName = obj.LastName;
            user.Phone = obj.Phone;
            user.Address = obj.Address;
            user.Email = credential.Email;
            user.Id = id;
            user.IsActive = (bool)credential.IsActive;

            return View(user);
            }
            catch { return View(); }
        }

        [HttpPost]
        public ActionResult Edit(UserDTO user,int id)
        {
            try
            {
                UserDetail userDetail = db.UserDetails.Find(id);
                Credential credential = db.Credentials.FirstOrDefault(x =>x.UId == id);

                userDetail.FirstName = user.FirstName;
                userDetail.LastName = user.LastName;
                userDetail.Address = user.Address;
                userDetail.Phone = user.Phone;

                credential.IsActive = user.IsActive;

                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                db.UserDetails.Remove(db.UserDetails.Find(id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch { return View(); }
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch { return View(); }
        }*/
    }
}