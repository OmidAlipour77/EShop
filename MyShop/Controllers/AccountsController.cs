using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using System.Web.Security;
using Utilities;

namespace MyShop.Controllers
{
    public class AccountsController : Controller
    {
        UnitOfWork db = new UnitOfWork();
        // GET: Accounts
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Users> users = db.UserRepository.Get();
                if(!users.Any(u=>u.Email==register.Email.Trim().ToLower()))
                {
                    Users user = new Users()
                    {
                        UserName = register.UserName,
                        Email = register.Email,
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password, "MD5"),
                        ActiveCode = Guid.NewGuid().ToString(),
                        RegisterDate = DateTime.Now,
                        IsActive = false,
                        RoleID = 1
                    };
                    db.UserRepository.Insert(user);
                    db.UserRepository.Save();
                    string body = PartialToStringClass.RenderPartialView("ManageEmail", "ActivaionEmail", user);
                    SendEmail.Send(user.Email, "ایمیل فعالسازی", body);
                    return View("SuccessRegister", user);
                }
                else
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده تکراری می باشد");
                }
              
            }
            return View();
        }
        public ActionResult ActiveUser(string id)
        {
            IEnumerable<Users> users = db.UserRepository.Get();
            var user = users.SingleOrDefault(u => u.ActiveCode == id);
            if(user==null)
            {
                return HttpNotFound();
            }
            user.IsActive = true;
            user.ActiveCode = Guid.NewGuid().ToString();
            db.UserRepository.Save();
            return View();
        }
        [Route("Login")]
        public ActionResult LoginUser()
        {
            return View();
        }
        [Route("Login")]
        [HttpPost]
        public ActionResult LoginUser(LoginViewModel login,string ReturnUrl="/")
        {
            if(ModelState.IsValid)
            {
                IEnumerable<Users> users = db.UserRepository.Get();
                var hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Password, "MD5");
                var user = users.SingleOrDefault(u => u.Email == login.Email && u.Password == hashPassword);
                if(user!=null)
                {
                    if(user.IsActive)
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, login.RememberMe);
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نشده است");
                    }
                }
                else
                {                  
                        ModelState.AddModelError("Email", "کاربری با اطلاعات وارد شده یافت نشد");
                }
            }
            return View();
        }
        [Route("LogOff")]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [Route("ForgotPassword")]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if(ModelState.IsValid)
            {
                var user = db.UserRepository.Get().SingleOrDefault(u => u.Email == forgot.Email);
                if (user != null)
                {
                    if (user.IsActive)
                    {
                        var body = PartialToStringClass.RenderPartialView("ManageEmail", "RecoveryPassword", user);
                        SendEmail.Send(user.Email, "بازیابی کلمه عبور", body);
                        return View("SuccesForgotPassword", user);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده یافت نشد");
                }
            }
            return View();
        }
       
        public ActionResult RecovryPassword(string id)
        {
            return View();
        }
      
        [HttpPost]
        public ActionResult RecovryPassword(string id,RecovryPasswordViewModel recovery)
        {
            if(ModelState.IsValid)
            {
                var user = db.UserRepository.Get().SingleOrDefault(u => u.ActiveCode == id);
                if(user==null)
                {
                    return HttpNotFound();
                }
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(recovery.Password, "MD5");
                user.ActiveCode = Guid.NewGuid().ToString();
                db.UserRepository.Save();
                return Redirect("/Login?recovery=true");
            }
            return View();
        }

        [Route("ChangePassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [Route("ChangePassword")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                var user = db.UserRepository.Get().Single(u => u.UserName == User.Identity.Name);
                var oldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(change.OldPassword, "MD5");
                if (user.Password == oldPassword)
                {
                    user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(change.Password, "MD5");
                    db.UserRepository.Save();
                    ViewBag.Success = true;
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "کلمه عبور فعلی اشتباه می باشد");
                }
            }
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}