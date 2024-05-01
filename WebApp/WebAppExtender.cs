using App.DAL.EF;
using App.Domain.Enums;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp;

public static class WebAppExtender
{
    public static void SetupAddData(this WebApplication app)
    {
        using var serviceScope = app.Services
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    
        context.Database.Migrate();

        using var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        using var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        IdentityResult res;
        
        foreach (var r in Enum.GetValues<EAppRole>())
        {
            res = roleManager.CreateAsync(new AppRole
            {
                Name = r.ToString()
            }).Result;
            if (!res.Succeeded) Console.WriteLine(res.ToString());
        }
        
        var user = new AppUser
        {
            Email = "admin@syncride.ee",
            UserName = "admin@syncride.ee",
            FirstName = "Admin",
            LastName = "Account"
        };
        res = userManager.CreateAsync(user, "House.Master2").Result;
        if (!res.Succeeded) Console.WriteLine(res.ToString());

        res = userManager.AddToRoleAsync(user, nameof(EAppRole.Admin)).Result;
        if (!res.Succeeded) Console.WriteLine(res.ToString());
    }
}