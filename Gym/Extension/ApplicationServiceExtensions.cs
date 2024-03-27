using Gym.Data;
using Gym.Repository.IRepository;
using Gym.Repository;
using Microsoft.EntityFrameworkCore;

namespace Gym.Extension
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GymDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ICoachRepository, CoachRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

            return services;
        }
    }
}
