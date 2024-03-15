using Authorization.IdentityServer.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.IdentityServer.Controllers
{
    [Route("[controller]")]
    public class AuthController: Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param> куда надо вернуться при успешной аутентификакации
        /// <returns></returns>
        [Route("[action]")]
        public IActionResult Login(string returnUrl)
        {
           
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("UserName", "User not found");
                return View(model);
            }

            var signinResult =  await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (signinResult.Succeeded)
            {
                return Redirect(model.ReturnUrl);
            }

            ModelState.AddModelError("UserName","Something went wrong");
            return View(model);
            
        }
    }
}
