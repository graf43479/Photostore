using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Domain;
using Domain.Entities;
using Domain.Infrasructure.Abstract;
using PhotoStore.Models;

namespace PhotoStore.Controllers
{
    [Authorize]
  //  [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private IAuthProvider auth;
        private DataManager dataManager;

        public AccountController(IAuthProvider auth, DataManager dataManager)
        {
            this.auth = auth;
            this.dataManager = dataManager;
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //if (User.Identity.IsAuthenticated)
            //{
              
            //        var p1 = Roles.IsUserInRole(User.Identity.Name, "admin");
            //        var p2 = Roles.IsUserInRole(User.Identity.Name, "User");
            //        var p3 = Roles.IsUserInRole(User.Identity.Name, "ContentManager");
              
            //    Roles.IsUserInRole(User.Identity.Name);
            //    bool t = User.IsInRole("admin");
            //    bool t2 = User.IsInRole("User");
            //}

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
       // [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (Session["attempt"] == null)
            {
                Session["attempt"] = 0;
            }


            //validate captcha
            if ((Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha) &&
                (int)Session["attempt"] > 3)
            {
                ModelState.AddModelError("Captcha", "Сумма введена неверно! Пожалуйста, повторите ещё раз!");
                Session["attempt"] = (int)Session["attempt"] + 1;

                return View(model);
            }
            if (ModelState.IsValid)
            {
                try
                {
                   User user = dataManager.UsersRepository.UsersInfo.FirstOrDefault(x => x.Login == model.UserName);

                    if (dataManager.Provider.ValidateUser(model.UserName,
                                                          CreatePasswordHash(model.Password, user.PasswordSalt)))
                    {

                        if (user.IsActivated != true)
                        {
                            ModelState.AddModelError("", "Учетная запись не активирована");
                            return View(model);
                        }

                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);


                        dataManager.UsersRepository.GetMembershipUserByName(model.UserName);
                        Session["attempt"] = 0;
                        
                        return RedirectToAction("About", "Home");
                    }
                    ModelState.AddModelError("", "Неудачная попытка входа на сайт");
                    Session["attempt"] = (int)Session["attempt"] + 1;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Неудачная попытка входа на сайт");
                    Session["attempt"] = (int)Session["attempt"] + 1;
                    return View(model);
                }
            }
            return View(model);
           
        }

        //
        // POST: /Account/LogOff

      //  [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //WebSecurity.Logout();
            FormsAuthentication.SignOut();
            //Session["UserName"] = null;
            //Session["UserID"] = null;
            HttpContext.Session.Clear();
            

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
              
                model.PasswordSalt = CreateSalt();
                string pwd = CreatePasswordHash(model.Password, model.PasswordSalt);

                

                int userRoleId = dataManager.UsersRepository.Roles.FirstOrDefault(x => x.RoleName.ToLower() == "user").RoleID;

                MembershipCreateStatus status = dataManager.Provider.CreateUser(model.Login, pwd,
                                                                                    model.Email, 
                                                                                    model.IsActivated, 
                                                                                    model.PasswordSalt,
                                                                                    userRoleId);
                    
                    if (status == MembershipCreateStatus.Success)
                    {
                        //FormsAuthentication.SetAuthCookie(model.Login, false);

                        dataManager.UsersRepository.GetMembershipUserByName(model.Login);

                        User userInfo = dataManager.UsersRepository.UsersInfo
                                                   .FirstOrDefault(p => p.Login == model.Login);

                        userInfo.Password = model.Password;


                        // userInfo.Password = originPassword; //ViewBag.originPassword;
                        string host = Request.Url.Host;
                        dataManager.DeliveryProcessor.EmailActivation(userInfo, host);

                      //  return RedirectToAction("UserRole", new { login = model.Login });
                        // dataManager.UsersRepository.AddUserToRole(model.Login, "User");

                        return RedirectToAction("Index","Home");

                        
                    }
                    else if (status == MembershipCreateStatus.DuplicateEmail)
                    {
                        RedirectToAction("Login", "Account");
                    }

                    ModelState.AddModelError("", GetMembershipCreateStatusResultText(status));
                }
            
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult AdminSetup()
        {
            if (dataManager.UsersRepository.AdminExists())
                return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AdminSetup(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(model.Password);
              //  string originPassword = sb.ToString();
                string salt = CreateSalt();
                string pwd = CreatePasswordHash(model.Password, model.PasswordSalt);
                //model.Password = CreatePasswordHash(model.Password, model.PasswordSalt);
                    dataManager.UsersRepository.CreateRole("admin");

                int adminRoleId = dataManager.UsersRepository.Roles.FirstOrDefault(x => x.RoleName.ToLower() == "admin").RoleID;


                MembershipCreateStatus status = dataManager.Provider.CreateUser(model.Login, 
                                                                                pwd, model.Email, 
                                                                                model.IsActivated, salt, adminRoleId);

                if (status == MembershipCreateStatus.Success)
                {

                    dataManager.UsersRepository.GetMembershipUserByName(model.Login);

                    User userInfo = dataManager.UsersRepository.UsersInfo
                                               .FirstOrDefault(p => p.Login == model.Login);

                    userInfo.Password = model.Password;
                    string host = Request.Url.Host;

                    dataManager.DeliveryProcessor.EmailActivation(userInfo, host);
                    

                    //dataManager.UsersRepository.AddUserToRole(model.Login, "Admin");
                    
                    return View("Success", model);
                }
                ModelState.AddModelError("", GetMembershipCreateStatusResultText(status));
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgottenPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgottenPassword(User user)
        {

            if (dataManager.UsersRepository.GetUserNameByEmail(user.Email) != "")
            {

                User userInfo = dataManager.UsersRepository.UsersInfo.FirstOrDefault(
                    x => x.Email == user.Email);
                string pwdOrigin = CreatePassword(6);
                userInfo.Password = CreatePasswordHash(pwdOrigin, userInfo.PasswordSalt);
                dataManager.UsersRepository.SaveUser(userInfo);
                userInfo.Password = pwdOrigin;
                string host = Request.Url.Host;
                dataManager.DeliveryProcessor.EmailRecovery(userInfo, host);


                ViewBag.UserInfo = "На указанный адрес был выслан пароль";
                return View("Success");
                //return Content(Boolean.TrueString);
                //return Json(JsonStandardResponse.SuccessResponse("На указанный адрес был выслан пароль"),
                //            JsonRequestBehavior.DenyGet);
            }

            else
            {
                if (user.Email == null)
                {
                    Session["UserEmail"] = "";
                    ModelState.AddModelError("", "Неудачная попытка изменения пароля");
                    //return Json(JsonStandardResponse.ErrorResponse("Пожалуйста проверьте форму"),
                                //JsonRequestBehavior.DenyGet);
                    //return Content("Пожалуйста проверьте форму");
                    return View(user);
                }
                else
                {
                    ModelState.AddModelError("", "Неудачная попытка изменения пароля");
                    //Session["UserEmail"] = "Пользователя с таким email не существует!";
                    //return Content("Пользователя с таким email не существует!");
                    //return Json(JsonStandardResponse.ErrorResponse("Пользователя с таким email не существует!"),
                                //JsonRequestBehavior.DenyGet);
                    return View(user);
                }


            }
        }

        public ActionResult ChangePassword()
        {
            if (User.Identity.IsAuthenticated)
            {

                User user = dataManager.UsersRepository.UsersInfo.FirstOrDefault(p => p.Login == User.Identity.Name);


                RegisterModel viewModel = new RegisterModel()
                {
                    Login = user.Login,
                    Password = "", //user.Password,
                    ConfirmPassword = "", // user.Password,
                    Email = user.Email,
                    UserID = user.UserID,
                    IsActivated = user.IsActivated,
                };
                //if (Request.IsAjaxRequest())
                //{
                return View(viewModel);
                //}

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(RegisterModel model)
        {
            if (ModelState.IsValid && (model.Password == model.ConfirmPassword))
            {
                User userInfo = dataManager.UsersRepository.UsersInfo.FirstOrDefault(p => p.UserID == model.UserID);
                string tmp = CreatePasswordHash(model.OldPassword, userInfo.PasswordSalt);
                if (userInfo.Password != CreatePasswordHash(model.OldPassword, userInfo.PasswordSalt) &&
                    (userInfo.Password.Length > 35))
                {
                    //return PartialView(model); 
                    ModelState.AddModelError("", "Неудачная попытка изменения учетной записи");
                    //return Content("Пожалуйста проверьте форму");
                    return View(model); 
                    //return Json(JsonStandardResponse.ErrorResponse("Пожалуйста проверьте форму"),
                    //            JsonRequestBehavior.DenyGet);
                }

                if (userInfo.Password != model.OldPassword && userInfo.Password.Length < 35)
                {
                    //return PartialView(model);
                    ModelState.AddModelError("", "Неудачная попытка изменения учетной записи");
                    //return Json(JsonStandardResponse.ErrorResponse("Пожалуйста проверьте форму"),
                    //            JsonRequestBehavior.DenyGet);
                    return View(model); 
                }

                userInfo.Password = CreatePasswordHash(model.Password, userInfo.PasswordSalt);
                FormsAuthentication.SetAuthCookie(model.Login, false);

                dataManager.UsersRepository.SaveUser(userInfo);
                TempData["Message"] = string.Format("Пароль для учетной записи {0} изменен", model.Login);
                TempData["messageType"] = "information-msg";
                //Session["UserName"] = model.UserName;
                //return RedirectToAction("List", "Product");
                //return Content(Boolean.TrueString); ; //JavaScript("window.location.replace('http://localhost:57600/Account/UserAccountEdit');");
                //return Json(JsonStandardResponse.SuccessResponse(true), JsonRequestBehavior.DenyGet);
                return RedirectToAction("Login", "Account");
            }
            else
                ModelState.AddModelError("", "Неудачная попытка изменения пароля");
            //return Json(JsonStandardResponse.ErrorResponse("Пожалуйста проверьте форму"), JsonRequestBehavior.DenyGet);
            //return Content("Пожалуйста проверьте форму"); //Content("Пожалуйста проверьте форму");
             return View(model);
            //  return PartialView(model);
        }
       
        
        
        
        #region Вспомогательные методы
        [AllowAnonymous]
        public ActionResult Activate(string username, string key)
        {

            if (dataManager.UsersRepository.ActivateUser(username, key) == false)
            {
                TempData["message"] =
                    string.Format("При активации произошла проблема. Возможно вы уже активировались ранее!");
                TempData["messageType"] = "warning-msg";
                return RedirectToAction("Register", "Account");
            }
            else
            {
                TempData["message"] =
                    string.Format(
                        "Поздравляем! Активация прошла успешно! Введите свой логин и пароль, чтобы авторизироваться на сайте! ");
                TempData["messageType"] = "confirmation-msg";
                //TempData.Keep("message");
                //logger.Info("Пользователь " + username + " активировался");
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult UserRole(string login)
        {
            dataManager.UsersRepository.AddUserToRole(login, "User");
            return View("Success");

        }


        public string GetMembershipCreateStatusResultText(MembershipCreateStatus status)
        {
            if (status == MembershipCreateStatus.DuplicateUserName)
                return "Пользователь с таким логином уже существует";
            if (status == MembershipCreateStatus.DuplicateEmail)
                return "Пользователь с таким email уже существует";
            return "Неизвестная ошибка";
        }

        //генерация хэша
        private static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
            return hashedPwd;
        }

        //Генерация соли
        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        //генерация пароля
        public string CreatePassword(int length)
        {
            string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string res = "";
            Random rnd = new Random();
            while (0 < length--)
                res += valid[rnd.Next(valid.Length)];
            return res;
        }

        //Капча
        [AllowAnonymous]
        public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Session["Captcha" + prefix] = a + b;

            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                            (rand.Next(0, 255)),
                            (rand.Next(0, 255)),
                            (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }


        [AllowAnonymous]
        public ActionResult RegNumImage(string prefix, bool noisy = true)
        {
            //var rand = new Random((int)DateTime.Now.Ticks);

            //generate new question
            //int a = rand.Next(10, 99);
            //int b = rand.Next(0, 9);

            //var captcha = string.Format("{0} / {1}", "У702ЕН", 50);

            string[] number = { "У", "702", "ЕН", "50" };

            //store answer
            //Session["Captcha" + prefix] = a + b;
            int h = 40;
            int w = 170;
            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(w, h))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                //gfx.Clear(Color.Red);
                //gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                //gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
                //var pen = new Pen(Color.Red);
                //Point[] points = new Point[2];
                //points[0] = new Point(5, 125);
                //points[1] = new Point(125, 10);

                //pen.Brush.
                //gfx.DrawLine(pen, 5,125,125,10);
                Image image = new Bitmap(System.IO.Path.Combine(Server.MapPath(Url.Content("~/Content")),
                                                                            "images",
                                                                            "template.jpg")); // new Bitmap("~/Content/images/template.jpg");
                gfx.DrawImage(image, 0, 0,w,h);
                //gfx
                //add noise
                //if (noisy)
                //{
                //    int i, r, x, y;
                //    var pen = new Pen(Color.Yellow);
                //    for (i = 1; i < 10; i++)
                //    {
                //        pen.Color = Color.FromArgb(
                //            (rand.Next(0, 255)),
                //            (rand.Next(0, 255)),
                //            (rand.Next(0, 255)));

                //        r = rand.Next(0, (130 / 3));
                //        x = rand.Next(0, 130);
                //        y = rand.Next(0, 30);

                //        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                //    }
                //}

                //add question
                gfx.DrawString(number[0], new Font("Tahoma", 17), Brushes.Black, 16, 7);
                gfx.DrawString(number[1], new Font("Tahoma", 22), Brushes.Black, 28, 1);
                gfx.DrawString(number[2], new Font("Tahoma", 17), Brushes.Black, 77, 7);
                if (number[3].Length==2)
                {
                    gfx.DrawString(number[3], new Font("Tahoma", 15), Brushes.Black, 120, 2);    
                }
                else
                {
                    gfx.DrawString(number[3], new Font("Tahoma", 15), Brushes.Black, 115, 2);    
                }
                

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Полный список кодов состояния см. по адресу http://go.microsoft.com/fwlink/?LinkID=177550
            //.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Имя пользователя уже существует. Введите другое имя пользователя.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Имя пользователя для данного адреса электронной почты уже существует. Введите другой адрес электронной почты.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Указан недопустимый пароль. Введите допустимое значение пароля.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Указан недопустимый адрес электронной почты. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Указан недопустимый ответ на вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Указан недопустимый вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Указано недопустимое имя пользователя. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.ProviderError:
                    return "Поставщик проверки подлинности вернул ошибку. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                case MembershipCreateStatus.UserRejected:
                    return "Запрос создания пользователя был отменен. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                default:
                    return "Произошла неизвестная ошибка. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";
            }
        }
        #endregion
    }
}
