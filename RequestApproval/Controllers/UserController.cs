using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using RequestApproval.DTO;
using RequestApproval.Enums;
using RequestApproval.Models;

namespace RequestApproval.Controllers
{
    public class UserController : Controller
    {
        RequestApprovalEntities5 db = new RequestApprovalEntities5();

        
        // GET: User
        public ActionResult Index()
        {
            try
            {


                List<UserDTO> users = (from ud in db.UserDetails
                                       from ld in db.LoginDetails
                                       from rd in db.Roles
                                       where ld.Id == ud.LoginId
                                       where rd.RoleId == ld.RoleId
                                       && ld.RoleId == 2 && ld.DeletedFlag == false
                                       select new
                                       {
                                           Id = (int)ld.Id,
                                           FirstName = ud.FirstName,
                                           LastName = ud.LastName,
                                           Phone = ud.Phone,
                                           Address = ud.Address,
                                           RoleId = (int)ld.RoleId,
                                           Email = ud.Email,
                                           IsActive = (bool)ld.IsActive,
                                           UserType = rd.RoleName
                                       }
                               ).AsEnumerable().Select(x => new UserDTO(
                                   x.Id,
                              x.FirstName,
                              x.LastName,
                              x.Phone,
                              x.Address,
                              x.RoleId,
                              x.Email,
                              x.IsActive,
                              x.UserType
                                   )).ToList();

                return View(users);
            }
            catch { return View(); }
        }

        public ActionResult Display()
        {
            List<UserDTO> users = (from ud in db.UserDetails
                        from ld in db.LoginDetails
                        from rd in db.Roles
                        where ld.Id == ud.LoginId
                        where rd.RoleId == ld.RoleId
                                   && ld.RoleId==2 && ld.DeletedFlag == false
                                   select new 
                        {
                            Id = (int)ld.Id,
                            FirstName = ud.FirstName,
                            LastName = ud.LastName,
                            Phone = ud.Phone,
                            Address = ud.Address,
                            RoleId = (int)ld.RoleId,
                            Email = ud.Email,
                            IsActive = (bool)ld.IsActive,
                            UserType = rd.RoleName
                        }
                              ).AsEnumerable().Select(x => new UserDTO(
                                  x.Id,
                             x.FirstName,
                             x.LastName,
                             x.Phone,
                             x.Address,
                             x.RoleId,
                             x.Email,
                             x.IsActive,
                             x.UserType
                                  )).ToList();
          
            return View(users);
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

                var checkExistance = db.UserDetails.Any(x => x.Email == request.Email);
                if (checkExistance)
                {
                    List<UserDetail> emailsExistCheck = new List<UserDetail>();
                    emailsExistCheck = db.UserDetails.ToList();
                    List<int> activeEmails = new List<int>();
                    bool exist = false;
                    foreach (var users in emailsExistCheck)
                    {
                        if(users.Email == request.Email)
                        {
                            var checkFlag = db.LoginDetails.FirstOrDefault(x => x.Id == users.LoginId);
                            if(checkFlag.DeletedFlag == false)
                            {
                                exist = true; 
                                break;
                            }
                        }
                    }
                    if (exist)
                    {
                        ViewBag.Notification = ValidationMessages.userExists;
                        return View();
                    }
                    else
                    {
                        if (Session["Permission"] != null)
                        {
                            request.Password = "Test@1234";
                        }
                        var password = Helper.Encrypt(request.Password);
                        UserDetail user = new UserDetail();
                        LoginDetail loginDetail = new LoginDetail();


                        loginDetail.Password = password;
                        loginDetail.IsActive = false;
                        loginDetail.RoleId = 2;
                        loginDetail.DeletedFlag = false;


                        db.LoginDetails.Add(loginDetail);
                        db.SaveChanges();

                        var UserId = loginDetail.Id;

                        user.LoginId = UserId;
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
                else
                {
                    if (Session["Permission"] != null)
                    {
                        request.Password = "Test@1234";
                    }
                    var password = Helper.Encrypt(request.Password);
                    UserDetail user = new UserDetail();
                    LoginDetail loginDetail = new LoginDetail();


                    loginDetail.Password = password;
                    loginDetail.IsActive = false;
                    loginDetail.RoleId = 2;
                    loginDetail.DeletedFlag = false;


                    db.LoginDetails.Add(loginDetail);
                    db.SaveChanges();

                    var UserId = loginDetail.Id;

                    user.LoginId = UserId;
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
            catch{
                return View();  
            }

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
                    List<UserDTO> userInfo = (from ud in db.UserDetails
                                           from ld in db.LoginDetails
                                           from rd in db.Roles
                                           where ld.Id == ud.LoginId
                                           where rd.RoleId == ld.RoleId
                                                      where ld.Id == id
                                           select new
                                           {
                                               Id = (int)ld.Id,
                                               FirstName = ud.FirstName,
                                               LastName = ud.LastName,
                                               Phone = ud.Phone,
                                               Address = ud.Address,
                                               RoleId = (int)ld.RoleId,
                                               Email = ud.Email,
                                               IsActive = (bool)ld.IsActive,
                                               UserType = rd.RoleName
                                           }
                              ).AsEnumerable().Select(x => new UserDTO(
                                  x.Id,
                             x.FirstName,
                             x.LastName,
                             x.Phone,
                             x.Address,
                             x.RoleId,
                             x.Email,
                             x.IsActive,
                             x.UserType
                                  )).ToList();

                  

                return View(userInfo);
                }
                else
                {
                    ViewBag.Notification = ValidationMessages.wentWrong;
                    return View();
                }
            }
            catch { return View(); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDTO request)
        {
            try
            {
                var password = Helper.Encrypt(request.Password);
                var users = (from ud in db.UserDetails
                                       from ld in db.LoginDetails
                                       where ld.Id == ud.LoginId
                                        && ud.Email == request.Email  && ld.DeletedFlag == false
                                        select new {ID = (int) ld.Id}
                                        ).ToList();
                int userID = 0;
                bool exist = false;
                if(users.Count > 0 )
                {
                    exist = true;
                     userID = (int)users[0].ID;
                }
                if(exist) 
                {  
                    var ob = db.LoginDetails.Find(userID);
                    if (ob != null && ob.Password == password)
                    {
                        // if user is not active
                        if (ob.IsActive == false)
                        {
                            ViewBag.Notification = ValidationMessages.notActive;
                            return View();
                        }
                        // if user is active
                        else if (ob.IsActive == true)
                        {
                            if (ob.RoleId == 1)
                            {
                                Session["Permission"] = "Access-granted";
                            }
                            UserDetail user = db.UserDetails.FirstOrDefault(x => x.LoginId == ob.Id);
                            string FullName = user.FirstName + " " + user.LastName;
                            Session["RoleID"] = ob.RoleId;
                            Session["Id"] = user.LoginId;
                            Session["UserName"] = FullName.ToString();

                            var roleID = (int)Session["RoleID"];
                            // if user is trying to sign-in
                            if (roleID == 2)
                            {
                                return RedirectToAction("UserDashboard", User);
                            }
                            // if admin is trying to sign-in
                            else
                            {
                                return RedirectToAction("Index", User);
                            }
                        }
                        else
                        {
                            // you do not have 
                            ViewBag.Notification = ValidationMessages.notActive;
                            return View();
                        }
                    }
                    else
                    {
                        // wrong credentials
                        ViewBag.Notification = ValidationMessages.wrongCredentials;
                        return View();
                    }
                }
                else
                {
                    ViewBag.Notification = ValidationMessages.userDoesnotExists;
                    return View();
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
        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {

            UserDetail obj = db.UserDetails.FirstOrDefault(x =>x.LoginId ==id);
            LoginDetail loginDetails = db.LoginDetails.FirstOrDefault(x => x.Id == id);
            UserDTO user = new UserDTO();
            user.FirstName = obj.FirstName;
            user.LastName = obj.LastName;
            user.Phone = obj.Phone;
            user.Address = obj.Address;
            user.Email = obj.Email;
            user.Id = id;
            user.IsActive = (bool)loginDetails.IsActive;
            Role role = db.Roles.FirstOrDefault(x =>x.RoleId == loginDetails.RoleId);
            user.UserType = role.RoleName;

            return View(user);
            }
            catch { return View(); }
        }

        [HttpPost]
        public ActionResult Edit(UserDTO user,int id)
        {
            try
            {
                LoginDetail loginDetail = db.LoginDetails.FirstOrDefault(x =>x.Id == id);
                UserDetail userDetail = db.UserDetails.FirstOrDefault(x =>x.LoginId == loginDetail.Id);

                userDetail.FirstName = user.FirstName;
                userDetail.LastName = user.LastName;
                userDetail.Address = user.Address;
                userDetail.Phone = user.Phone;

                loginDetail.IsActive = user.IsActive;

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
                LoginDetail loginDetail = db.LoginDetails.FirstOrDefault(x =>x.Id==id);
                loginDetail.IsActive = false;
                loginDetail.DeletedFlag = true;
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
        }
    }
}