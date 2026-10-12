using System.Reflection;
using GoGym.Domain.AdminAggregate;
using GoGym.Domain.GymAggregate;
using GoGym.Domain.ParticipantAggregate;
using GoGym.Domain.RoomAggregate;
using GoGym.Domain.SessionAggregate;
using GoGym.Domain.SubscriptionAggregate;
using GoGym.Domain.TrainerAggregate;
using Microsoft.EntityFrameworkCore;

namespace GoGym.Infrastructure.Persistence;

public class GoGymDbContext : DbContext
{
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Gym> Gyms { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public GoGymDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}