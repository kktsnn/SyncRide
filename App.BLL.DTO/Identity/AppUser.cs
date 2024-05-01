using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.BLL.DTO.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public string FullName => $"{FirstName} {LastName}";

    public string? About { get; set; }
    
    public string? ProfilePictureExtension { get; set; }
}