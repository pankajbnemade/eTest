using ERP.DataAccess.EntityData;
using ERP.Services;
using ERP.Services.Accounts;
using ERP.Services.Accounts.Interface;
using ERP.Services.Admin;
using ERP.Services.Admin.Interface;
using ERP.Services.Common;
using ERP.Services.Common.Interface;
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

            services.AddTransient<ICompany, CompanyService>();
            services.AddTransient<ICity, CityService>();
            services.AddTransient<IState, StateService>();
            services.AddTransient<ICountry, CountryService>();
            services.AddTransient<IDepartment, DepartmentService>();
            services.AddTransient<IDesignation, DesignationService>();
            services.AddTransient<IEmployee, EmployeeService>();
            services.AddTransient<IForm, FormService>();
            services.AddTransient<IModule, ModuleService>();
            services.AddTransient<IStatus, StatusService>();
            services.AddTransient<IUnitOfMeasurement, UnitOfMeasurementService>();
            services.AddTransient<IVoucherSetupDetail, VoucherSetupDetailService>();
            services.AddTransient<IVoucherSetup, VoucherSetupService>();
            services.AddTransient<IVoucherStyle, VoucherStyleService>();

            #endregion // Master

            #region Accounts
            services.AddTransient<ILedger, LedgerService>();
            services.AddTransient<ILedgerAddress, LedgerAddressService>();
            services.AddTransient<ITaxRegister, TaxRegisterService>();
            services.AddTransient<ICurrency, CurrencyService>();
            services.AddTransient<ICurrencyConversion, CurrencyConversionService>();
            services.AddTransient<IFinancialYear, FinancialYearService>();

            services.AddTransient<ISalesInvoice, SalesInvoiceService>();
            services.AddTransient<ISalesInvoiceDetail, SalesInvoiceDetailService>();
            services.AddTransient<ISalesInvoiceTax, SalesInvoiceTaxService>();
            services.AddTransient<ISalesInvoiceDetailTax, SalesInvoiceDetailTaxService>();
            services.AddTransient<IVoucherSetupDetail, VoucherSetupDetailService>();


            #endregion // Accounts

            #region Admin

            services.AddTransient<IApplicationIdentityUser, ApplicationIdentityUserService>();

            
            #endregion

            services.AddTransient<ICommon, CommonService>();
        }
    }
}
