using Application.Mapping;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<UserService, UserService>();
            services.AddTransient<DealService, DealService>();
            services.AddTransient<ReviewService, ReviewService>();
            services.AddTransient<ApartmentService, ApartmentService>();

            return services;
        }
    }
}
