using GoGym.Application.Common.Interfaces;
using GoGym.Domain.TrainerAggregate;
using Microsoft.EntityFrameworkCore;

namespace GoGym.Infrastructure.Persistence.Repositories;

public class TrainersRepository : ITrainersRepository
{
    private readonly GoGymDbContext _dbContext;

    public TrainersRepository(GoGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddTrainerAsync(Trainer trainer)
    {
        await _dbContext.Trainers.AddAsync(trainer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Trainer?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Trainers.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task RemoveAsync(Trainer trainer)
    {
        _dbContext.Trainers.Remove(trainer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Trainer trainer)
    {
        _dbContext.Trainers.Update(trainer);
        await _dbContext.SaveChangesAsync();
    }
}