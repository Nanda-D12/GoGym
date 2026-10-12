using GoGym.Domain.AdminAggregate;

namespace GoGym.Application.Common.Interfaces;

public interface IAdminsRepository
{
    Task<Admin?> GetByIdAsync(Guid id);
    Task AddAdminAsync(Admin admin);
    Task UpdateAsync(Admin admin);
}