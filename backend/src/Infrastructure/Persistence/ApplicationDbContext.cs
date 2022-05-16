using Application.Common.Interfaces;
using Application.Utils;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ICurrentUserService _currentUserService;

    #region Constructor

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    #endregion

    #region Overrides

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = DateUtil.GetCurrentDate();
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = DateUtil.GetCurrentDate();
                    break;
            }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = DateUtil.GetCurrentDate();
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = DateUtil.GetCurrentDate();
                    break;
            }

        return base.SaveChanges();
    }

    #endregion

    #region Dbsets

    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    public DbSet<AttributeType> AttributeTypes { get; set; }
    public DbSet<AttributeValue> AttributeValues { get; set; }

    #endregion
}