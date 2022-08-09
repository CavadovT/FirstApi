using FirstApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FirstApi.Extentions
{
    public static class ApllicationServiceExtentions
    {
        public static IServiceCollection AddApplications(this IServiceCollection services, IConfiguration config) 
        {
            services.AddDbContext<AppDbContext>(opt => {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }

    }
}
