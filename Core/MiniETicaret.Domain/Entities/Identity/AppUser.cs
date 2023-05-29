using Microsoft.AspNetCore.Identity;

namespace MiniETicaret.Domain.Entities.Identity;

public class AppUser : IdentityUser<string>
{
    public string? NameSurname { get; set; }
}