using IdentityDemo.Models.Entities;
using IdentityDemo.Views.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemo.Models
{
    public class AccountService
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        IHttpContextAccessor accessor;
        private readonly MyContext myContext;

        public AccountService(/*IdentityDbContext identityContext,*/
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        MyContext myContext,
        IHttpContextAccessor accessor)

        {
            //identityContext.Database.EnsureCreated();
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.myContext = myContext;
            this.accessor = accessor;

        }

        public async Task<string> TryRegisterAsync(RegisterVM viewModel)
        {
            IdentityUser identityUser = new IdentityUser
            {
                UserName = viewModel.Username
            };
            IdentityResult createResult = await
                userManager.CreateAsync(identityUser, viewModel.Password);

            bool createSucceeded = createResult.Succeeded;

            if (!createSucceeded)
            {
                return createResult.Errors.First().Description;
            }
            else
            {
                myContext.Users.Add(new User
                {
                    Id = identityUser.Id,                    
                });
                await myContext.SaveChangesAsync();

                return null;
            }
        }

        public async Task<MembersVM> GetUserInfoAsync()
        {
            var userId = userManager.GetUserId(accessor.HttpContext.User);

            return await myContext.Users.Where(o => o.Id == userId).Select(o =>

            new MembersVM
            {
                Username = o.IdNavigation.UserName,
            }).SingleAsync();
        }

        public async Task<bool> TryLoginAsync(LoginVM viewModel)
        {
            SignInResult signInResult = await signInManager.PasswordSignInAsync(
            viewModel.Username,
            viewModel.Password,
            isPersistent: false,
            lockoutOnFailure: false);

            return signInResult.Succeeded;
        }

        public async Task TryLogoutAsync()
        {
            await signInManager.SignOutAsync();

        }

    }
}
