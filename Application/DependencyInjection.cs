using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using Application.Interfaces;
using Application.Services;
using Application.Mappings;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Mappings.MappingProfile));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register Services
        services.AddScoped<IAuthService, Services.AuthService>();
        services.AddScoped<IPharmacyService, Services.PharmacyService>();
        services.AddScoped<IMedicineService, Services.MedicineService>();
        services.AddScoped<ISupplierService, Services.SupplierService>();
        services.AddScoped<IOrderService, Services.OrderService>();

        
        return services;
    }
}
