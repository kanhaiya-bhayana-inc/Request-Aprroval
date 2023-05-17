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

        private readonly int adminID = (int)RolesData.Admin;
        private readonly int userID = (int)RolesData.User;
        private readonly string userRoleName = Enum.GetName(typeof(RolesData),2);

        // GET: User
        /*[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]*/

        public ActionResult Index()
        {
            try
            {
                //if (Session["Id"] != null)
               // {
                    List<UserDTO> users = (from ud in db.UserDetails
                                           from ld in db.LoginDetails
                                           from rd in db.Roles
                                           where ld.Id == ud.LoginId
                                           where rd.RoleId == ld.RoleId
                                           && ld.RoleId == userID && ld.DeletedFlag == false
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
                //}
                //return RedirectToAction("Login");
            }
            catch { return View(); }
        }

        public ActionResult Display()
        {
            // LINQ Inner Join
            /*List<UserDTO> users = (from ud in db.UserDetails
                                   from ld in db.LoginDetails
                                   from rd in db.Roles
                                   where ld.Id == ud.LoginId
                                   where rd.RoleId == ld.RoleId
                                              && ld.RoleId == userID && ld.DeletedFlag == false
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
*/

            // LINQ LINQ login Usrtbl  Role - left join
            List<UserDTO> query = (from ld in db.LoginDetails
                                   join ud in db.UserDetails on ld.Id equals ud.LoginId into ab
                                   join rd in db.Roles on ld.RoleId equals rd.RoleId into bc
                                   from x in ab.DefaultIfEmpty()
                                   from y in bc.DefaultIfEmpty()
                                   select new
                                   {
                                       Id = (int)ld.Id,
                                       FirstName = (x.FirstName == null ? "NA" : x.FirstName),
                                       LastName = (x.LastName == null ? "NA" : x.LastName),
                                       Phone = (x.Phone == null ? "NA" : x.Phone),
                                       Address = (x.Address == null ? "NA" : x.Address),
                                       Email = (x.Email == null ? "NA" : x.Email),
                                       IsActive = (bool)ld.IsActive,
                                       RoleId = (int)ld.RoleId,
                                       UserType = y.RoleName

                                   }).AsEnumerable()
                        .Select(it => new UserDTO(
                            it.Id,
                            it.FirstName,
                            it.LastName,
                            it.Phone,
                            it.Address,
                            it.RoleId,
                            it.Email,
                            it.IsActive,
                            it.UserType

                            )).ToList();




            // LINQ login Usrtbl  Role - Right join
            List<UserDTO> response = (from ud in db.UserDetails
                            join ld in db.LoginDetails on ud.LoginId equals ld.Id into q1
                            from x in q1.DefaultIfEmpty()
                            where x.RoleId == 2 || x.RoleId == null && x.DeletedFlag == false || x.DeletedFlag == null
                            select new
                            {
                                Id = (int?)ud.LoginId ?? 0,
                                FirstName = ud.FirstName ?? "NA",
                                LastName = ud.LastName ?? "NA",
                                Address = ud.Address ?? "NA",
                                Phone = ud.Phone ?? "NA",
                                Email = ud.Email ?? "NA",
                                RoleId = (x.RoleId) ?? 0,
                                IsActive = x.IsActive == null ? false : (bool)x.IsActive,
                                RoleName = x.RoleId == 2 ? userRoleName : "NA"
                            }
                            ).AsEnumerable()
                            .Select( it => new UserDTO(
                                it.Id,
                                it.FirstName,
                                it.LastName,
                                it.Address,
                                it.Phone,
                                (int)it.RoleId, 
                                it.Email,
                                it.IsActive,
                                it.RoleName

                                )).ToList() ;



            // inner join Lambda Expression

            /* var res = db.UserDetails.Join(db.LoginDetails,
                 usr => usr.LoginId,
                 lgd => lgd.Id,
                 (usr, lgd) => new
                 {
                     Id = (int)lgd.Id,
                     FirstName = usr.FirstName,
                     LastName = usr.LastName,
                     Phone = usr.Phone,
                     Address = usr.Address,
                     RoleId = (int)lgd.RoleId,
                     Email = usr.Email,
                     //IsActive = (bool)lgd.IsActive,
                     DeletedFlag = (bool)lgd.DeletedFlag
                 }
                 )
                 .Join(
                 db.Roles,
                 logidData => logidData.RoleId,
                 rdata => rdata.RoleId,
                 (logidData, rdata) => new
                 {
                     Id = (int)logidData.Id,
                     FirstName = logidData.FirstName,
                     LastName = logidData.LastName,
                     Phone = logidData.Phone,
                     Address = logidData.Address,
                     RoleId = (int)logidData.RoleId,
                     Email = logidData.Email,
                     //IsActive = (bool)logidData.IsActive,
                     DeletedFlag = (bool)logidData.DeletedFlag,
                     UserType = rdata.RoleName
                 }
                 ).Where(x => x.RoleId == userID && x.DeletedFlag == false).Select(x => new UserDTO(
                                   x.Id,
                              x.FirstName,
                              x.LastName,
                              x.Phone,
                              x.Address,
                              x.RoleId,
                              x.Email,
                              //x.IsActive,
                              x.UserType
                                   )).ToList();
            */
            // Left join Lambda Expression

            var leftJoinOutput = db.UserDetails.GroupJoin(db.LoginDetails,
                usr => usr.LoginId,
                lgd => lgd.Id,
                (usr, loginGrp) => new
                {
                    FirstName = usr.FirstName,
                    LastName = usr.LastName,
                    Address = usr.Address,
                    Phone = usr.Phone,
                    Email = usr.Email,
                    Password = (loginGrp.FirstOrDefault(x => x.Id == usr.LoginId) == null ? "NA" : loginGrp.FirstOrDefault(x => x.Id == usr.LoginId).Password),
                    RoleID = (loginGrp.FirstOrDefault(x => x.Id == usr.LoginId)==null?0: loginGrp.FirstOrDefault(x => x.Id == usr.LoginId).RoleId),
                    IsActive = (loginGrp.FirstOrDefault(x => x.Id == usr.LoginId) == null ? false : loginGrp.FirstOrDefault(x => x.Id == usr.LoginId).IsActive),

                }
                ).ToList();

            // Left join Lambda Expression

            var rightJoinOutput = db.LoginDetails.GroupJoin(db.UserDetails,
                lgd => lgd.Id,
                usr => usr.LoginId,
                (lgd, userGrp) => new
                {
                    FirstName = (userGrp.FirstOrDefault(x =>x.LoginId == lgd.Id) == null?"NA": userGrp.FirstOrDefault(x => x.LoginId == lgd.Id).FirstName),
                    LastName = (userGrp.FirstOrDefault(x => x.LoginId == lgd.Id) == null ? "NA" : userGrp.FirstOrDefault(x => x.LoginId == lgd.Id).LastName),
                    Address = (userGrp.FirstOrDefault(x => x.LoginId == lgd.Id) == null ? "NA" : userGrp.FirstOrDefault(x => x.LoginId == lgd.Id).Address),
                    Phone = (userGrp.FirstOrDefault(x => x.LoginId == lgd.Id) == null ? "NA" : userGrp.FirstOrDefault(x => x.LoginId == lgd.Id).Phone),
                    Email = (userGrp.FirstOrDefault(x => x.LoginId == lgd.Id) == null ? "NA" : userGrp.FirstOrDefault(x => x.LoginId == lgd.Id).Email),
                    Password =  lgd.Password,
                    RoleID = lgd.RoleId,
                    IsActive = lgd.IsActive,
                }
                ).ToList();

            var fullOuterJoin = leftJoinOutput.Union(rightJoinOutput);


            var list = query.Union(response);
            var activeCount = list.Count(x => x.IsActive == true);

            var groupByLastName = from d in list
                                  group d by d.LastName into newGroup
                                  orderby newGroup.Key
                                  select newGroup;

            return View(list);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignUp()
        {
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
                        if (users.Email == request.Email)
                        {
                            var checkFlag = db.LoginDetails.FirstOrDefault(x => x.Id == users.LoginId);
                            if (checkFlag.DeletedFlag == false)
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
                        loginDetail.RoleId = userID;
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
                    loginDetail.RoleId = userID;
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
            catch
            {
                return View();
            }

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        /*[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]*/
        public ActionResult UserDashboard()
        {
            try
            {
                if (Session["Id"] != null)
                {
                    var id = (int)Session["Id"];
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

                    if (userInfo.Count > 0)
                    {
                        return View(userInfo[0]);
                    }
                    else
                    {
                        ViewBag.Notification = ValidationMessages.wentWrong;
                        return View();
                    }

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
        public ActionResult Login(LoginDTO request, EventArgs e)
        {
            try
            {
                var password = Helper.Encrypt(request.Password);
                var users = (from ud in db.UserDetails
                             from ld in db.LoginDetails
                             where ld.Id == ud.LoginId
                              && ud.Email == request.Email && ld.DeletedFlag == false
                             select new { ID = (int)ld.Id }
                                        ).ToList();
                int userID1 = 0;
                bool exist = false;
                if (users.Count > 0)
                {
                    exist = true;
                    userID1 = (int)users[0].ID;
                }
                if (exist)
                {
                    var ob = db.LoginDetails.Find(userID1);
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
                            if (ob.RoleId == adminID)
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
                            if (roleID == userID)
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

        /*[OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]*/
        public ActionResult Logout()
        {
            try
            {

                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Login");
            }
            catch { return View(); }
        }

        [HttpGet]
        //[Authorize]
        public ActionResult Edit(int id)
        {
            try
            {

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

                if (userInfo.Count > 0)
                {
                    return View(userInfo[0]);
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
        //[Authorize]
        public ActionResult Edit(UserDTO user, int id)
        {
            try
            {
                LoginDetail loginDetail = db.LoginDetails.FirstOrDefault(x => x.Id == id);
                UserDetail userDetail = db.UserDetails.FirstOrDefault(x => x.LoginId == loginDetail.Id);

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
                LoginDetail loginDetail = db.LoginDetails.FirstOrDefault(x => x.Id == id);
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