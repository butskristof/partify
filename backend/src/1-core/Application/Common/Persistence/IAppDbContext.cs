using Microsoft.EntityFrameworkCore;
using Partify.Domain.Entities;

namespace Partify.Application.Common.Persistence;

public interface IAppDbContext
{
    DbSet<SpotifyTokens> SpotifyTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
