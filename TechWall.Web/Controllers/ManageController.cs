﻿using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TechWall.Data;
using TechWall.Entities;
using TechWall.Models;

namespace TechWall.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }



        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }





        public async Task<ActionResult> ChangeAvatar()
        {
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (applicationUser != null)
            {
                return View(applicationUser);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.Error });
        }

        [HttpPost]
        
        public async Task<ActionResult> ChangeAvatar(HttpPostedFileBase file)
        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            ApplicationUser retrievedApplicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var picture = SavePicture(file, retrievedApplicationUser.LastName);
            retrievedApplicationUser.Picture = picture;
            UserManager.GetClaims(retrievedApplicationUser.Id).Clear();
            UserManager.AddClaim(retrievedApplicationUser.Id, new Claim("ProfilePicture", picture.URL));
            var result = await UserManager.UpdateAsync(retrievedApplicationUser);

            AuthenticationManager.SignOut();
            await SignInManager.SignInAsync(retrievedApplicationUser,true,false);
            //Identity.RemoveClaim(Identity.FindFirst("ProfilePicture"));
            //Identity.AddClaim(new Claim("ProfilePicture", picture.URL));
            //var authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
            //authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(Identity), new AuthenticationProperties() { IsPersistent = true });

            return RedirectToAction("Index");
        }

        private ProfilePicture SavePicture(HttpPostedFileBase file, string username)
        {
            if (file != null && file.ContentLength > 0)
            {

                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Users", Server.MapPath(@"\")));

                string pathString = System.IO.Path.Combine(originalDirectory.ToString(), username);

                var extension = Path.GetExtension(file.FileName);

                var filename = Guid.NewGuid().ToString();

                bool isExists = System.IO.Directory.Exists(pathString);

                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);

                var path = string.Format("{0}\\{1}", pathString, filename + extension);

                file.SaveAs(path);



                return new ProfilePicture { URL = "~/Images/Users/" + username + "/" + filename + extension, ModifiedOn = DateTime.Now };
            }
            return null;
        }





        public async Task<ActionResult> ChangeProfile()
        {
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (applicationUser != null)
            {
                return View(applicationUser);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.Error });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile(ApplicationUser applicationUser)
        {
            ManageMessageId manageMessageId;

            ApplicationUser retrievedApplicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            retrievedApplicationUser.FirstName = applicationUser.FirstName;
            retrievedApplicationUser.LastName = applicationUser.LastName;
            retrievedApplicationUser.Address = applicationUser.Address;
            retrievedApplicationUser.City = applicationUser.City;
            retrievedApplicationUser.State = applicationUser.State;
            retrievedApplicationUser.ZipCode = applicationUser.ZipCode;

            // Update the Profile
            var result = await UserManager.UpdateAsync(retrievedApplicationUser);
            //if (result.Succeeded)
            //{
            //    manageMessageId = ManageMessageId.ChangeProfileSuccess;

            //    // If the Email address changed, sync both Email and UserName to it
            //    if (retrievedApplicationUser.Email != applicationUser.Email)
            //    {
            //        // Update the UserName
            //        string previousUserName = retrievedApplicationUser.UserName;
            //        retrievedApplicationUser.UserName = applicationUser.Email;
            //        result = await UserManager.UpdateAsync(retrievedApplicationUser);
            //        if (result.Succeeded)
            //        {
            //            // Update the Email address
            //            result = await UserManager.SetEmailAsync(retrievedApplicationUser.Id, applicationUser.Email);
            //            if (result.Succeeded)
            //            {
            //                manageMessageId = ManageMessageId.ChangeEmailSuccess;

            //                string code = await UserManager.GenerateEmailConfirmationTokenAsync(retrievedApplicationUser.Id);

            //                var callbackUrl = Url.Action(
            //                    "ConfirmEmail",
            //                    "Account",
            //                    new { userId = retrievedApplicationUser.Id, code = code }, protocol: Request.Url.Scheme);

            //                await UserManager.SendEmailAsync(
            //                    retrievedApplicationUser.Id,
            //                    "Confirm your account",
            //                    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            //            }
            //            else
            //            {
            //                retrievedApplicationUser.UserName = previousUserName;
            //                result = await UserManager.UpdateAsync(retrievedApplicationUser);
            //                manageMessageId = ManageMessageId.Error;
            //            }
            //        }
            //        else
            //        {
            //            retrievedApplicationUser.UserName = previousUserName;
            //            result = await UserManager.UpdateAsync(retrievedApplicationUser);
            //            manageMessageId = ManageMessageId.Error;
            //        }
            //    }
            //}
            //else
            //    manageMessageId = ManageMessageId.Error;

            return RedirectToAction("Index"); //new { Message = manageMessageId }
        }
        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.ChangeProfileSuccess ? "Your profile has been changed"
                : message == ManageMessageId.ChangeEmailSuccess ? "Your email has been changed"
                : "";

            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                FullName = user.FullName,
                Email = user.Email,
                Address = user.AddressBlock
               
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            ChangeProfileSuccess,
            ChangeEmailSuccess,
            Error
        }

        #endregion
    }
}