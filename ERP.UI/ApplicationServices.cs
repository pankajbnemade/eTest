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
            services.AddTransient<ITaxRegisterDetail, TaxRegisterDetailService>();
            services.AddTransient<ICurrency, CurrencyService>();
            services.AddTransient<ICurrencyConversion, CurrencyConversionService>();
            services.AddTransient<IFinancialYear, FinancialYearService>();
            services.AddTransient<IFinancialYearCompanyRelation, FinancialYearCompanyRelationService>();
            services.AddTransient<ILedgerFinancialYearBalance, LedgerFinancialYearBalanceService>();

            services.AddTransient<ISalesInvoice, SalesInvoiceService>();
            services.AddTransient<ISalesInvoiceDetail, SalesInvoiceDetailService>();
            services.AddTransient<ISalesInvoiceTax, SalesInvoiceTaxService>();
            services.AddTransient<ISalesInvoiceDetailTax, SalesInvoiceDetailTaxService>();

            services.AddTransient<IPurchaseInvoice, PurchaseInvoiceService>();
            services.AddTransient<IPurchaseInvoiceDetail, PurchaseInvoiceDetailService>();
            services.AddTransient<IPurchaseInvoiceTax, PurchaseInvoiceTaxService>();
            services.AddTransient<IPurchaseInvoiceDetailTax, PurchaseInvoiceDetailTaxService>();

            services.AddTransient<ICreditNote, CreditNoteService>();
            services.AddTransient<ICreditNoteDetail, CreditNoteDetailService>();
            services.AddTransient<ICreditNoteTax, CreditNoteTaxService>();
            services.AddTransient<ICreditNoteDetailTax, CreditNoteDetailTaxService>();

            services.AddTransient<IDebitNote, DebitNoteService>();
            services.AddTransient<IDebitNoteDetail, DebitNoteDetailService>();
            services.AddTransient<IDebitNoteTax, DebitNoteTaxService>();
            services.AddTransient<IDebitNoteDetailTax, DebitNoteDetailTaxService>();

            services.AddTransient<IPaymentVoucher, PaymentVoucherService>();
            services.AddTransient<IPaymentVoucherDetail, PaymentVoucherDetailService>();

            services.AddTransient<IReceiptVoucher, ReceiptVoucherService>();
            services.AddTransient<IReceiptVoucherDetail, ReceiptVoucherDetailService>();

            services.AddTransient<IContraVoucher, ContraVoucherService>();
            services.AddTransient<IContraVoucherDetail, ContraVoucherDetailService>();

            services.AddTransient<IJournalVoucher, JournalVoucherService>();
            services.AddTransient<IJournalVoucherDetail, JournalVoucherDetailService>();

            services.AddTransient<IAdvanceAdjustment, AdvanceAdjustmentService>();
            services.AddTransient<IAdvanceAdjustmentDetail, AdvanceAdjustmentDetailService>();

            services.AddTransient<IVoucherSetupDetail, VoucherSetupDetailService>();
            services.AddTransient<IOutstandingInvoice, OutstandingInvoiceService>();



            //Reports

            services.AddTransient<IGeneralLedger, GeneralLedgerService>();

            


            #endregion // Accounts

            #region Admin

            services.AddTransient<IApplicationIdentityUser, ApplicationIdentityUserService>();


            #endregion

            services.AddTransient<ICommon, CommonService>();
        }
    }
}
