using Microsoft.EntityFrameworkCore;

namespace Partify.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }
}
