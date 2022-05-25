using ERP.DataAccess.EntityData;
using ERP.Models.Accounts;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class SalesInvoiceReportService : ISalesInvoiceReport
    {
        ErpDbContext _dbContext;

        public SalesInvoiceReportService(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SalesInvoiceModel> GetSalesInvoice(int salesInvoiceId)
        {
            SalesInvoiceModel salesInvoiceModel = new SalesInvoiceModel();

            salesInvoiceModel = await _dbContext.Salesinvoices
                                    .Include(i => i.CustomerLedger)
                                    .Include(i => i.Currency)
                                    .Include(i => i.BillToAddress).ThenInclude(i => i.Country)
                                    .Include(i => i.BillToAddress).ThenInclude(i => i.State)
                                    .Include(i => i.BillToAddress).ThenInclude(i => i.City)
                                    .Where(w => w.SalesInvoiceId==salesInvoiceId)
                                    .Select(s => new SalesInvoiceModel
                                    {
                                        SalesInvoiceId = s.SalesInvoiceId,
                                        InvoiceNo = s.InvoiceNo,
                                        InvoiceDate = s.InvoiceDate,
                                        CustomerLedgerId = s.CustomerLedgerId,
                                        BillToAddressId = s.BillToAddressId,
                                        AccountLedgerId = s.AccountLedgerId,
                                        BankLedgerId = s.BankLedgerId,
                                        CustomerReferenceNo = s.CustomerReferenceNo,
                                        CustomerReferenceDate = s.CustomerReferenceDate,
                                        CreditLimitDays = s.CreditLimitDays,
                                        PaymentTerm = s.PaymentTerm,
                                        Remark = s.Remark,
                                        TaxModelType = s.TaxModelType,
                                        TaxRegisterId = s.TaxRegisterId,
                                        CurrencyId = s.CurrencyId,
                                        ExchangeRate = s.ExchangeRate,
                                        TotalLineItemAmountFc = s.TotalLineItemAmountFc,
                                        TotalLineItemAmount = s.TotalLineItemAmount,
                                        GrossAmountFc = s.GrossAmountFc,
                                        GrossAmount = s.GrossAmount,
                                        NetAmountFc = s.NetAmountFc,
                                        NetAmount = s.NetAmount,
                                        NetAmountFcInWord = s.NetAmountFcinWord,
                                        TaxAmountFc = s.TaxAmountFc,
                                        TaxAmount = s.TaxAmount,
                                        DiscountPercentageOrAmount = s.DiscountPercentageOrAmount,
                                        DiscountPerOrAmountFc = s.DiscountPerOrAmountFc,
                                        DiscountAmountFc = s.DiscountAmountFc,
                                        DiscountAmount = s.DiscountAmount,
                                        StatusId = s.StatusId,
                                        CompanyId = s.CompanyId,
                                        FinancialYearId = s.FinancialYearId,
                                        MaxNo = s.MaxNo,
                                        VoucherStyleId = s.VoucherStyleId,
                                        CustomerLedgerName = s.CustomerLedger.LedgerName,
                                        BillToAddress = s.BillToAddress.AddressDescription,
                                        BillToAddressCountryName = (null != s.BillToAddress.Country ? s.BillToAddress.Country.CountryName : ""),
                                        BillToAddressStateName = (null != s.BillToAddress.State ? s.BillToAddress.State.StateName : ""),
                                        BillToAddressCityName = (null != s.BillToAddress.City ? s.BillToAddress.City.CityName : ""),
                                        BillToAddressPhoneNo = (s.BillToAddress.PhoneNo.Trim().Length != 0 ? s.BillToAddress.PhoneNo : ""),
                                        BillToAddressFaxNo = (s.BillToAddress.FaxNo.Trim().Length != 0 ? s.BillToAddress.FaxNo : ""),
                                        BillToAddressEmailAddress = (s.BillToAddress.EmailAddress.Trim().Length != 0 ? s.BillToAddress.EmailAddress : ""),
                                        AccountLedgerName = s.AccountLedger.LedgerName,
                                        CurrencyCode = s.Currency.CurrencyCode,
                                    })
                                    .FirstOrDefaultAsync();

            return salesInvoiceModel;
        }

        public async Task<IList<SalesInvoiceDetailModel>> GetSalesInvoiceDetailList(int salesInvoiceId)
        {
            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = new List<SalesInvoiceDetailModel>();

            salesInvoiceDetailModelList = await _dbContext.Salesinvoicedetails
                                    .Include(i => i.UnitOfMeasurement)
                                    .Where(w => w.SalesInvoiceId == salesInvoiceId)
                                    .Select(s => new SalesInvoiceDetailModel
                                    {
                                        SalesInvoiceDetId  = s.SalesInvoiceDetId,
                                        SalesInvoiceId  = s.SalesInvoiceId,
                                        SrNo  = s.SrNo,
                                        Description  = s.Description,
                                        UnitOfMeasurementId  = s.UnitOfMeasurementId,
                                        Quantity  = s.Quantity,
                                        PerUnit  = s.PerUnit,
                                        UnitPrice  = s.UnitPrice,
                                        GrossAmountFc  = s.GrossAmountFc,
                                        GrossAmount  = s.GrossAmount,
                                        TaxAmountFc  = s.TaxAmountFc,
                                        TaxAmount  = s.TaxAmount,
                                        NetAmountFc  = s.NetAmountFc,
                                        NetAmount  = s.NetAmount,
                                        UnitOfMeasurementName  = s.UnitOfMeasurement.UnitOfMeasurementName
                                    })
                                    .ToListAsync();

            return salesInvoiceDetailModelList;
        }

        public async Task<IList<SalesInvoiceTaxModel>> GetSalesInvoiceTaxList(int salesInvoiceId)
        {
            IList<SalesInvoiceTaxModel> salesInvoiceTaxModelList = new List<SalesInvoiceTaxModel>();

            salesInvoiceTaxModelList = await _dbContext.Salesinvoicetaxes
                                    .Include(i => i.TaxLedger)
                                    .Where(w => w.SalesInvoiceId == salesInvoiceId)
                                    .Select(s => new SalesInvoiceTaxModel
                                    {
                                        SalesInvoiceTaxId  = s.SalesInvoiceTaxId,
                                        SalesInvoiceId  = s.SalesInvoiceId,
                                        SrNo  = s.SrNo,
                                        TaxLedgerId  = s.TaxLedgerId,
                                        TaxPercentageOrAmount  = s.TaxPercentageOrAmount,
                                        TaxPerOrAmountFc  = s.TaxPerOrAmountFc,
                                        TaxAddOrDeduct  = s.TaxAddOrDeduct,
                                        TaxAmountFc  = s.TaxAmountFc,
                                        TaxAmount  = s.TaxAmount,
                                        Remark  = s.Remark,
                                        TaxLedgerName  = s.TaxLedger.LedgerName
                                    })
                                    .ToListAsync();

            return salesInvoiceTaxModelList;
        }

        public async Task<IList<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxList(int salesInvoiceId)
        {
            IList<SalesInvoiceDetailTaxModel> salesInvoiceDetailTaxModelList = new List<SalesInvoiceDetailTaxModel>();

            salesInvoiceDetailTaxModelList = await _dbContext.Salesinvoicedetailtaxes
                                    .Include(i => i.SalesInvoiceDet)
                                    .Include(i => i.TaxLedger)
                                    .Where(w => w.SalesInvoiceDet.SalesInvoiceId == salesInvoiceId)
                                    .Select(s => new SalesInvoiceDetailTaxModel
                                    {
                                        SalesInvoiceDetTaxId  = s.SalesInvoiceDetTaxId,
                                        SalesInvoiceDetId  = s.SalesInvoiceDetId,
                                        SrNo  = s.SrNo,
                                        TaxLedgerId  = s.TaxLedgerId,
                                        TaxPercentageOrAmount  = s.TaxPercentageOrAmount,
                                        TaxPerOrAmountFc  = s.TaxPerOrAmountFc,
                                        TaxAddOrDeduct  = s.TaxAddOrDeduct,
                                        TaxAmountFc  = s.TaxAmountFc,
                                        TaxAmount  = s.TaxAmount,
                                        Remark  = s.Remark,
                                        TaxLedgerName  = s.TaxLedger.LedgerName
                                    })
                                    .ToListAsync();

            return salesInvoiceDetailTaxModelList;
        }

    }
}
