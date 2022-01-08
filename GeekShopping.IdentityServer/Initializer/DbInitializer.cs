using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MySqlContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(MySqlContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            if (_role.FindByNameAsync(IdentityConfiguration.ADMIN).Result != null) return;

            _role.CreateAsync(new IdentityRole(IdentityConfiguration.ADMIN)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.CLIENT)).GetAwaiter().GetResult();

            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "savio-admin",
                Email = "savio-admin@barbosa.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (11) 12345-6789",
                FirstName = "Sávio",
                LastName = "Barbosa"
            };

            _user.CreateAsync(admin, "Savio123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin, IdentityConfiguration.ADMIN).GetAwaiter().GetResult();

            var adminClaims = _user.AddClaimsAsync(admin, new Claim[] { 
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, $"{admin.FirstName}"),
                new Claim(JwtClaimTypes.FamilyName, $"{admin.LastName}"),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.ADMIN)
            }).Result;

            ApplicationUser client = new ApplicationUser()
            {
                UserName = "savio-client",
                Email = "savio-client@barbosa.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (11) 12345-6789",
                FirstName = "Sávio",
                LastName = "Barbosa"
            };

            _user.CreateAsync(client, "Savio123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(client, IdentityConfiguration.CLIENT).GetAwaiter().GetResult();

            var clientClaims = _user.AddClaimsAsync(client, new Claim[] {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, $"{client.FirstName}"),
                new Claim(JwtClaimTypes.FamilyName, $"{client.LastName}"),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.CLIENT)
            }).Result;
        }
    }
}
