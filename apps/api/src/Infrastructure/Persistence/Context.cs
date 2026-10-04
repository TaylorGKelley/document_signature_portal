using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DocSign.Infrastructure.Persistence.Identity;

namespace DocSign.Infrastructure.Persistence;

public class Context(DbContextOptions<Context> options) : IdentityDbContext<User>(options)
{

}
