using Logic.Interfaces.EmailAccountInterface;
using Logic.Interfaces.EmailCleaner;
using Logic.Interfaces.EmailCleanerInterface;
using Logic.Interfaces.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Interfaces
{
    public static class InterfaceServiceRegistration
    {
        public static void RegisterInterfaceServices(this IServiceCollection services) 
        { 
            services.AddScoped<IEmailAccountInterfaceModule, EmailAccountInterfaceModule>();
            services.AddScoped<IEmailCleanerInterfaceModule, EmailCleanerInterfaceModule>();
            services.AddScoped<IEmailCleanerImportModule, EmailCleanerImportModule>();
            services.AddScoped<IEmailCleanerConfigurationModule, EmailCleanerConfigurationModule>();
        }
    }
}
