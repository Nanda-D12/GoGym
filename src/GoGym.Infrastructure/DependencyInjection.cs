using GoGym.Application.Common.Interfaces;
using GoGym.Domain.Common.Interfaces;
using GoGym.Infrastructure.BackgroundJobs;
using GoGym.Infrastructure.Persistence;
using GoGym.Infrastructure.Persistence.Interceptors;
using GoGym.Infrastructure.Persistence.Repositories;
using GoGym.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GoGym.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStructure(this IServiceCollection services)
    {
        services
            .AddPersistence()
            .AddServices();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<PublishDomainEventsInterceptor>();

        services
            .AddDbContext<GoGymDbContext>((sp, options) =>{
                    var interceptor = sp.GetRequiredService<PublishDomainEventsInterceptor>();

                    options.UseSqlServer("Server=localhost,1433;Database=GoGymDb;User Id=sa;Password=SuperStrong!Passw0rd1;TrustServerCertificate=True;")
                            .AddInterceptors(interceptor);
            });

        services.AddScoped<IAdminsRepository, AdminsRepository>();
        services.AddScoped<IGymsRepository, GymsRepository>();
        services.AddScoped<IParticipantsRepository, ParticipantsRepository>();
        services.AddScoped<IRoomsRepository, RoomsRepository>();
        services.AddScoped<ISessionsRepository, SessionsRepository>();
        services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
        services.AddScoped<ITrainersRepository, TrainersRepository>();

        services.AddHostedService<ProcessOutboxMessagesJob>();

        return services;
    }
}