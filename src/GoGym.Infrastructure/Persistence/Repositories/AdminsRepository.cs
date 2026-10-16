using GoGym.Application.Common.Interfaces;
using GoGym.Application.Profiles.Common;
using GoGym.Domain.AdminAggregate;
using GoGym.Domain.Profiles;
using Microsoft.EntityFrameworkCore;

namespace GoGym.Infrastructure.Persistence.Repositories;

public class AdminsRepository : IAdminsRepository
{
    private readonly GoGymDbContext _dbContext;

    public AdminsRepository(GoGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAdminAsync(Admin admin)
    {
        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Admin?> GetByIdAsync(Guid id)
    {
        var admin = await _dbContext.Admins
                        .FirstOrDefaultAsync(a => a.Id == id);

        return admin;
    }

    public async Task<Profile?> GetProfileByUserIdAsyc(Guid userId)
    {
        var admin = await _dbContext.Admins
                        .AsNoTracking()
                        .FirstOrDefaultAsync(admin => admin.UserId == userId);

        return admin is null ? null : new Profile(admin.Id, ProfileType.Admin);
    }

    public async Task UpdateAsync(Admin admin)
    {
        _dbContext.Admins.Update(admin);
        await _dbContext.SaveChangesAsync();
    }
}