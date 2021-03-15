using ERP.DataAccess.EntityData;
using ERP.Services;
using ERP.Services.Master;
using ERP.Services.Master.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.UI
{
    public static class ApplicationServices
    {
        public static void Register(ref IServiceCollection services)
        {
            #region Registering DBContext for Dependency Injection

            services.AddTransient<ErpDbContext>();
            services.AddScoped<DbContext, ErpDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            ///MySqlServiceCollectionExtensions.AddEntityFrameworkMySql

            #endregion Registering DBContext for Dependency Injection

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<IAuthorizationHandler, AnonymousHandler>();
            //services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            //services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidatiomAttributeAdapterProvider>();
            //services.AddSingleton<ISharedResource, SharedResource>();

            #region Master

            services.AddTransient<ICity, CityService>();
            services.AddTransient<IState, StateService>();
            services.AddTransient<ICountry, CountryService>();

            #endregion // Master
        }
    }
}
