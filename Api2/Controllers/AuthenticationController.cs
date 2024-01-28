using Api2.ApiModels;
using Infra.Data.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api2.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize]
        [Route("getUser")]
        public async Task<IActionResult> GetUserDataAsync()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            var userRoles = await _userManager.GetRolesAsync(user);

            return new JsonResult(new UserDTO(user.Id, user.FirstName, user.LastName, user.CompanyId, user.UserName, userRoles));
        }

        [HttpPost]
        [Route("addAdmin")]
        public async Task<IActionResult> AddAdmin(string username)
        {
            var user = new ApplicationUser
            {
                Email = "admin@licenta.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = username,
                FirstName = "Andra",
                LastName = "Donca",
                CompanyId = 2,
            };

            var role = new IdentityRole
            {
                Name = "Admin"
            };

            var customer = new IdentityRole
            {
                Name = "Customer"
            };

            var roleManagerResult = await _roleManager.CreateAsync(role);
            var roleManager2Result = await _roleManager.CreateAsync(customer);

            var res = await _userManager.CreateAsync(user, "Admin@123");
            var roleRes = await _userManager.AddToRoleAsync(user, "Admin");

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized("Parola nu este corecta!");
            var authClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            authClaims.AddRange(userClaims);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                authClaims.AddRange(roleClaims);
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YVBy0OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SBM="));

            var validity = 1;
            if (model.RememberMe) validity = 7;
            var token = new JwtSecurityToken(
                "http://localhost:57678/",
                "http://localhost:57678/",
                expires: DateTime.Now.AddDays(validity),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterinAsync([FromBody] RegisterDTO model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null) return Unauthorized("Username already exists! Try to choose another one.");
            var appUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CompanyId = model.CompanyId,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var user = await _userManager.CreateAsync(appUser, model.Password);

            var role = await _userManager.AddToRoleAsync(appUser, "Customer");

            return Created();
        }

        [HttpPatch("changePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);

            var changePassword = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (changePassword.Succeeded)
                return Ok();

            return BadRequest(changePassword.Errors.FirstOrDefault()?.Code);
        }

        [HttpGet]
        [Authorize]
        [Route("getUsersFromCompany")]
        public IActionResult GetUsersFromCompanyAsync(int companyId)
        {
            var companyUsers = _userManager.Users.Where(u => u.CompanyId == companyId);
            return Ok(companyUsers);
        }
    }
}
