﻿using System;
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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDTO request)
        {
            string password = Helper.Encrypt(request.Password);
            var checkLogin = db.Credentials.FirstOrDefault(x => x.Email ==request.Email && x.Password ==password);            if (checkLogin != null)
            {
                if (checkLogin.IsActive ==  true)
                {
                    Session["Permission"] = "Access-granted";
                }
                UserDetail user = db.UserDetails.FirstOrDefault(x => x.Id == checkLogin.UId);
                string FullName = user.FirstName + " " + user.LastName;
                Session["UserName"] = FullName.ToString();
                return RedirectToAction("Index", User);
            }
            else
            {
                ViewBag.Notification = errorObj.wrongCredentials;
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            UserDetail obj = db.UserDetails.FirstOrDefault(x =>x.Id ==id);
            Credential credential = db.Credentials.FirstOrDefault(x => x.UId == id);
            UserDTO user = new UserDTO();
            user.FirstName = obj.FirstName;
            user.LastName = obj.LastName;
            user.Phone = obj.Phone;
            user.Address = obj.Address;
            user.RoleId = (int)obj.RoleId;
            user.Id = id;
            user.Email = credential.Email;
            user.Password = credential.Password;
            user.IsActive = (bool)credential.IsActive;

            return View(user);
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
                userDetail.RoleId = user.RoleId;

                credential.Email = user.Email;
                credential.Password = user.Password;
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
        }
    }
}