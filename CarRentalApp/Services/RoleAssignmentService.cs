using Microsoft.AspNetCore.Identity;

namespace CarRentalApp.Services
{
    public class RoleAssignmentService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleAssignmentService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task EnsureUserRoleAsync(IdentityUser user)
        {
            
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return;

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            if (!await _userManager.IsInRoleAsync(user, "User"))
                await _userManager.AddToRoleAsync(user, "User");
        }
    }
}
