using Microsoft.EntityFrameworkCore;

namespace Web.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }
}
