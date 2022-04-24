namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    //Todo: Add Dbsets
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}