using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityDemo.Models;
using IdentityDemo.Views.Account;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using IdentityDemo.Models.Entities;

namespace IdentityDemo.Controllers
{
    public class AccountController : Controller
    {
        AccountService accountService;

        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        [Authorize]
        [HttpGet("members")]
        public async Task<IActionResult> MembersAsync()
        {
            var model = await accountService.GetUserInfoAsync();
            return View(model);
        }

        [HttpGet("")]
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterVM viewModel)
        {
            if (!ModelState.IsValid)
                return View();

            // Try to register user
            var errorMessage = await accountService.TryRegisterAsync(viewModel);
            if (errorMessage != null)
            {
                // Show error
                ModelState.AddModelError(string.Empty, errorMessage);
                return View();
            }

            // Redirect user
            return RedirectToAction(nameof(MembersAsync));
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginVM viewModel)
        {
            if (!ModelState.IsValid)
                return View();

            // Check if credentials is valid (and set auth cookie)
            var success = await accountService.TryLoginAsync(viewModel);
            if (!success)
            {
                // Show error
                ModelState.AddModelError(string.Empty, "Login failed");
                return View();
            }

            // Redirect user
            return RedirectToAction(nameof(MembersAsync).Replace("Async", ""));
        }

        [HttpGet("logout")]
        public async Task<IActionResult> TryLogOutAsync()
        {
            await accountService.TryLogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
