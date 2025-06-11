using Microsoft.AspNetCore.Identity;

namespace CarRentalApp.Services
{
    public class RoleAssignmentService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RoleAssignmentService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task EnsureUserHasUserRoleAsync(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles == null || roles.Count == 0)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}
