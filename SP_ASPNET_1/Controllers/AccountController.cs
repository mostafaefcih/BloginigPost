using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SP_ASPNET_1.Models;
using SP_ASPNET_1.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SP_ASPNET_1.Controllers
{
    //[RoutePrefix("Account")]  

    public class AccountController : Controller
    {
        private   ApplicationRoleManager _roleManager;
        private    ApplicationUserManager _userManager;
        private  ApplicationSignInManager _signInManager;
        public AccountController()
        {

        }
        public AccountController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
             ApplicationRoleManager roleManager)
        {
            ///Make an instance of the user manager in the controller to avoid null reference exception
            _userManager = userManager;
            _signInManager = signInManager;
            ///Make an instance of the role manager in the constructor to avoid null reference exception
            _roleManager = roleManager;
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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        
        
        
        // GET: Account
        //[Authorize(Roles ="Admin")]
        public ActionResult Index(int page = 1, int pageSize = 2)
        {
            var result = new PagedResult<ApplicationUser>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = UserManager.Users.Count();
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (page - 1) * pageSize;

             result.Results = UserManager.Users.OrderBy(c => c.Email)
                .Skip(skip).Take(pageSize)
                .Include(c => c.Roles).AsNoTracking().ToList();
            var roles = RoleManager.Roles.Include(r => r.Users).ToList();

            var userIndexVm = new UserIndexVM()
            {
                Users = result,
                Roles = roles
            };
            return View(userIndexVm);
        }
        [HttpGet]
      public  ActionResult ManageUserRoles(string userId) {
            //  get all roles

            var roles = RoleManager.Roles.ToList();
            var user = UserManager.Users.Where(u => u.Id == userId).FirstOrDefault();

            var editUserVM = new EditUserVM()
            {
                Roles = roles,
                User = user
            };
            return PartialView("_ManageUserRoles",editUserVM);
        }
        [HttpPost]
        public JsonResult AssignRoleToUser(string userId ,string[] rolesIds)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || rolesIds.Length == 0)
                    throw new Exception();
                var rolesName = RoleManager.Roles.Where(c => rolesIds.Contains(c.Id)).Select(r=>r.Name).ToArray();
                UserManager.AddToRoles(userId, rolesName);
            return Json(true, JsonRequestBehavior.AllowGet);

                // return RedirectToAction("Index", "Account");
            }
            catch (Exception ex)
            {

                throw;
                
            }
          
        }



        // GET: /account/login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            // Confirm user is not logged in

            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
                return RedirectToAction("index","Home");

            // Return view
            return View(new LoginUserVM());
        }
        // POST: /account/login
        [HttpPost]
        public async Task<ActionResult> Login(LoginUserVM model, string returnUrl)
        {
            model.Username = model.Email; 
            // Check model state
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            try
            {
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)

                {
                    case SignInStatus.Success:
                        FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                        return Redirect(returnUrl ?? Url.Action("Index", "Blog"));

                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");

                        return View(model);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            // Check if the user is valid

       
        }
        // GET: /account/Logout
        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //return RedirectToAction("Index", "Home");
            //FormsAuthentication.SignOut();
            return RedirectToAction("Login");


        }


        // GET: /account/Register
        //[ActionName("Register")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View(new UserVM());
        }

        [AllowAnonymous]
        //[Route("Register")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Register(UserVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }
         

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email ,
                                                Surname=model.Surename,Name=model.Name };



            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if(user.Id!=null)
                UserManager.AddToRole(user.Id, "AUTHOR");


                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Home");
            }
            
            // Redirect
            return View();
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("user/{id:guid}")]
        public ActionResult DeleteUser(string id)
        {
            var userToDelete = _userManager.FindById(id);
            if (userToDelete == null)
            {
                return HttpNotFound();
            }
            _userManager.Delete(userToDelete);
            return RedirectToAction("Index");

        }





        //[Authorize]
        //public ActionResult UserNavPartial()
        //{
        //    // Get username
        //    string username = User.Identity.Name;

        //    // Declare model
        //    UserNavPartialVM model;

        //    using (DefaultCn ctx = new DefaultCn())
        //    {
        //        // Get the user
        //        UserDTO dto = ctx.Users.FirstOrDefault(x => x.Username == username);

        //        // Build the model
        //        model = new UserNavPartialVM()
        //        {
        //            FirstName = dto.FirstName,
        //            LastName = dto.LastName
        //        };
        //    }

        //    // Return partial view with model
        //    return PartialView(model);
        //}

        // GET: /account/user-profile
        //[HttpGet]
        //[ActionName("user-profile")]
        //[Authorize]
        //public ActionResult UserProfile()
        //{
        //    // Get username
        //    string username = User.Identity.Name;

        //    // Declare model
        //    UserProfileVM model;

        //    using (DefaultCn ctx = new DefaultCn())
        //    {
        //        // Get user
        //        UserDTO dto = ctx.Users.FirstOrDefault(x => x.Username == username);

        //        // Build model
        //        model = new UserProfileVM(dto);
        //    }

        //    // Return view with model
        //    return View("UserProfile", model);
        //}

        //// POST: /account/user-profile
        //[HttpPost]
        //[ActionName("user-profile")]
        //[Authorize]
        //public ActionResult UserProfile(UserProfileVM model)
        //{
        //    // Check model state
        //    if (!ModelState.IsValid)
        //    {
        //        return View("UserProfile", model);
        //    }

        //    // Check if passwords match if need be
        //    if (!string.IsNullOrWhiteSpace(model.Password))
        //    {
        //        if (!model.Password.Equals(model.ConfirmPassword))
        //        {
        //            ModelState.AddModelError("", "Passwords do not match.");
        //            return View("UserProfile", model);
        //        }
        //    }

        //    using (DefaultCn ctx = new DefaultCn())
        //    {
        //        // Get username
        //        string username = User.Identity.Name;

        //        // Make sure username is unique
        //        if (ctx.Users.Where(x => x.Id != model.Id).Any(x => x.Username == username))
        //        {
        //            ModelState.AddModelError("", "Username " + model.Username + " already exists.");
        //            model.Username = "";
        //            return View("UserProfile", model);
        //        }

        //        // Edit DTO
        //        UserDTO dto = ctx.Users.Find(model.Id);

        //        dto.FirstName = model.FirstName;
        //        dto.LastName = model.LastName;
        //        dto.EmailAddress = model.EmailAddress;
        //        dto.Username = model.Username;

        //        if (!string.IsNullOrWhiteSpace(model.Password))
        //        {
        //            dto.Password = model.Password;
        //        }

        //        // Save
        //        ctx.SaveChanges();
        //    }

        //    // Set TempData message
        //    TempData["SM"] = "You have edited your profile!";

        //    // Redirect
        //    return Redirect("~/account/user-profile");
        //}
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}