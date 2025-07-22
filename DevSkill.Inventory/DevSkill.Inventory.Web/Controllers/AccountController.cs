using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace DevSkill.Inventory.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _ApplicationDbContext;

        public AccountController(SignInManager<ApplicationUser> signInManager,
            IUserStore<ApplicationUser> userStore,
            UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,ApplicationDbContext applicationDbContext)
        {
            _signInManager = signInManager;
            _userStore = userStore;
            _userManager = userManager;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _ApplicationDbContext = applicationDbContext;
        }
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string returnUrl = null)
        {
            var model = new LoginModel();
            if (!string.IsNullOrEmpty(model.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
            }
            returnUrl ??= Url.Content("~/");
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var isEmail = model.EmailOrUsername.Contains('@');
                var user = isEmail ?
                    await _userManager.FindByEmailAsync(model.EmailOrUsername) :
                    await _userManager.FindByNameAsync(model.EmailOrUsername);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        var employeeName = user.EmployeeId == null ? "DevSkill" :
                            _ApplicationDbContext.Employees
                                .Where(e => e.Id == user.EmployeeId)
                                .Select(e => e.Name)
                                .FirstOrDefault() ?? "DevSkill";

                        var joiningDate = _ApplicationDbContext.Employees
                                .Where(e => e.Id == user.EmployeeId)
                                .Select(e => e.JoiningDate)
                                .FirstOrDefault();

                        var role = user.IsOwner==true?"Owner":(await _userManager.GetRolesAsync(user)).FirstOrDefault();

                        var existingClaims = await _userManager.GetClaimsAsync(user);
                        var claimsToRemove = existingClaims.Where(c =>
                            c.Type == "ProfileName" ||
                            c.Type == "ProfileRole" ||
                            c.Type == "JoiningDate").ToList();

                        if (claimsToRemove.Any())
                        {
                            await _userManager.RemoveClaimsAsync(user, claimsToRemove);
                        }

                        var newClaims = new List<Claim>
                        {
                            new Claim("ProfileName", employeeName),
                            new Claim("ProfileRole", role),
                            new Claim("JoiningDate", user.EmployeeId==null?"":joiningDate.ToString("dd/MM/yyyy"))
                        };

                        await _userManager.AddClaimsAsync(user, newClaims);

                        await _signInManager.RefreshSignInAsync(user);

                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(model.ReturnUrl);
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            returnUrl ??= Url.Content("~/");

            return LocalRedirect(returnUrl);
        }



        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}