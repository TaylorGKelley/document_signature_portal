using Microsoft.AspNetCore.Identity;

namespace DocSign.Infrastructure.Persistence.Identity;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
